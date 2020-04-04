using FriscoDev.Data.PMGDataPacketProtocol;
using FriscoTab;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FriscoDev.Data
{

    public abstract class DataConnection
    {
        protected Queue msgPMGQueue = null;
        protected Queue statsRecordQueue = null;
        protected Queue mpptMsgQueue = null;

        protected Object msgLock = null;

        // We may open a serial port connection to non PMG device.
        // this flag will indicate whether we have receive any 
        // AC message
        //
        public Boolean isConnectedToPMG = false;

        private Thread dataReadingThread = null;
        protected Boolean stopReadingThread = false;

        // Buffer and index for receiving data from device
        public const int RCV_BUFFER_SIZE = 1024 * 10;
        public const int RCV_BATCH_SIZE = 800;
        public const int MAX_MESSAGE_SIZE = 1024;

        protected byte[] rcvBuffer = new byte[RCV_BUFFER_SIZE + RCV_BATCH_SIZE + 200];
        protected int rcvBufferIndex = 0;
        protected int rcvBufferDataStartIndex = 0;

        public string errMsg = string.Empty;

        public uint messageReceived = 0;
        public uint messageSent = 0;
        public uint totalByteSent = 0;
        public uint totalByteReceived = 0;

        public DataConnection(ref Queue msgPMGQueueIn, ref Queue statsRecordQueueIn,
                              ref Queue mpptMsgQueueIn, ref Object msgLockIn)
        {
            msgPMGQueue = msgPMGQueueIn;
            statsRecordQueue = statsRecordQueueIn;
            mpptMsgQueue = mpptMsgQueueIn;

            msgLock = msgLockIn;

            // Serial Port Data Reading Thread
            dataReadingThread = new Thread(new ThreadStart(ReadingThreadFunction));
        }

        // for serial port connectin, the address is com port name
        // for ble connection, the address is the 6 bytes hex BLE PMG address
        public abstract Boolean OpenConnection(string address);
        public abstract Boolean CloseConnection();
        public abstract Boolean IsConnectionOpened();
        public abstract bool SendData(byte[] data);
        public abstract void ReadingThreadFunction();

        public void StartReadingData()
        {
            stopReadingThread = false;

            if (!dataReadingThread.IsAlive)
                dataReadingThread.Start();
        }
        public void StopReadData()
        {
            stopReadingThread = true;
        }
        public void ResetReceiveBuffer()
        {
            rcvBufferDataStartIndex = 0;
            rcvBufferIndex = 0;
        }

    }

    public class SerialPortConnection : DataConnection
    {
        public event EventHandler connectionLostEventHandler = null;
        public SerialPort serialPort = new SerialPort();

        public SerialPortConnection(ref Queue msgPMGQueueIn, ref Queue statsRecordQueueIn,
                                    ref Queue mpptMsgQueueIn, ref Object msgLockIn) :
                                    base(ref msgPMGQueueIn, ref statsRecordQueueIn,
                                    ref mpptMsgQueueIn, ref msgLockIn)
        {
        }

        public override Boolean OpenConnection(string address)
        {
            if (serialPort.IsOpen)
                return false;

            // Set the read/write timeouts
            try
            {
                //
                // Set the port's settings
                //
                serialPort.BaudRate = 115200;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;
                serialPort.Parity = Parity.None;
                serialPort.PortName = address;
                serialPort.WriteTimeout = 5000;

                Console.WriteLine("Opening Serial Port!\n");

                // Open the port
                serialPort.Open();

                serialPort.DiscardInBuffer();

                StartReadingData();
            }
            catch (Exception ex)
            {
                errMsg = "Could not open the COM port. " + ex.Message + "COM Port Unavalible";
                return false;
            }

            return true;
        }

        public override Boolean CloseConnection()
        {
            //stopReadingThread = true;
            isConnectedToPMG = false;

            if (!serialPort.IsOpen)
                return false;
            else
            {
                Thread.Sleep(500);

                serialPort.Close();


                return true;
            }
        }

        public override Boolean IsConnectionOpened()
        {
            if (serialPort.IsOpen)
                return true;
            else
                return false;
        }

        public override void ReadingThreadFunction()
        {
            Console.WriteLine("Enter SerialPort ReadingThreadFunction!");

            while (serialPort != null)
            {
                if (stopReadingThread)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                if (!serialPort.IsOpen)
                {
                    if (connectionLostEventHandler != null)
                    {
                        connectionLostEventHandler?.Invoke(null, null);
                    }

                    Thread.Sleep(200);
                    continue;
                }

                int bytes = serialPort.BytesToRead;

                if (bytes == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                SerialPortReceivedDataHandler();
            }

            Console.WriteLine("Exiting SerialPort ReadingThreadFunction!");
        }

        public override Boolean SendData(byte[] data)
        {
            if (!serialPort.IsOpen)
                return false;

            serialPort.Write(data, 0, data.Length);

            return true;
        }

        // =========================================================================
        // Following Function specific to Serial Port Data Handling. BLE and 3G will
        // have different implementation
        private void SerialPortReceivedDataHandler()
        {
            int i;

            // If the com port has been closed, do nothing
            if (!serialPort.IsOpen || stopReadingThread)
                return;

            // Obtain the number of bytes waiting in the port's buffer
            int bytes = serialPort.BytesToRead;

            if (bytes > RCV_BATCH_SIZE)
                bytes = RCV_BATCH_SIZE;

            // This is to prevent unprocessed data exceed buffer size
            if (rcvBufferIndex - rcvBufferDataStartIndex >= (RCV_BUFFER_SIZE >> 1))
            {
                rcvBufferDataStartIndex = rcvBufferIndex - 1000;
            }

            //
            // If we exceed buffer size, we copy the data from end segment to starting
            // and rotate back
            //
            if (rcvBufferIndex + bytes >= RCV_BUFFER_SIZE)
            {
                int index = 0;
                for (i = rcvBufferDataStartIndex; i < rcvBufferIndex; i++)
                {
                    rcvBuffer[index] = rcvBuffer[i];
                    index++;
                }

                rcvBufferDataStartIndex = 0;
                rcvBufferIndex = index;
            }

            try
            {
                // Read the data from the port and store it in our buffer
                bytes = serialPort.Read(rcvBuffer, rcvBufferIndex, bytes);

                rcvBufferIndex += bytes;
            }
            catch (Exception)
            {
                return;
            }

            ProcessDataFromPMG();
        }

        // Read data and put in different message queue
        private void ProcessDataFromPMG()
        {
            int i;

            // No data
            if (rcvBufferDataStartIndex == rcvBufferIndex)
                return;

            ReadMessage:

            int actualDataLength = 0;
            int payloadLength = 0;
            int msgLen = 0;

            // Check PMG data
            for (i = rcvBufferDataStartIndex; i < rcvBufferIndex; i++)
            {
                //
                // Search for AC message identifier
                //
                if ((rcvBuffer[i] == 0x41) && (rcvBuffer[i + 1] == 0x43))
                {
                    actualDataLength = rcvBufferIndex - i;

                    //
                    // Min size of message is 12 bytes
                    //
                    if (actualDataLength < 12)
                    {
                        rcvBufferDataStartIndex = i;
                        return;
                    }

                    payloadLength = rcvBuffer[i + 8] + (rcvBuffer[i + 9] << 8);

                    //
                    // 10 bytes for header portion
                    // 2 bytes for checksum
                    //
                    msgLen = 10 + payloadLength + 2;

                    // Pad 0 if necessary
                    //if (payloadLength % 2 != 0)
                    //    msgLen += 1;

                    // We have bad data. The message size should never exceed 512 limit
                    if (msgLen > MAX_MESSAGE_SIZE)
                    {
                        rcvBufferDataStartIndex = 0;
                        rcvBufferIndex = 0;
                        return;
                    }

                    //
                    // Make sure we recevie all data
                    //
                    if (actualDataLength >= msgLen)
                    {
                        byte[] data = new byte[msgLen];

                        Buffer.BlockCopy(rcvBuffer, i, data, 0, msgLen);

                        PMGMessage msg = new PMGMessage(data);
                        msg.timestamp = DateTime.Now;

                        PMGErrorCode errorCode = PMGErrorCode.Success;

                        if (msg.IsValidMessage(ref errorCode))
                        {
                            lock (msgLock)
                            {
                                PMGCommandID commandId = msg.GetCommandID();

                                if (commandId == PMGCommandID.StatsRecordCommand)
                                    statsRecordQueue.Enqueue(msg);
                                else if (commandId == PMGCommandID.PowerMonitor ||
                                         commandId == PMGCommandID.PowerSpecs)
                                    mpptMsgQueue.Enqueue(msg);
                                else
                                    msgPMGQueue.Enqueue(msg);

                            
                                messageReceived++;
                            }
                        }
                        else
                        {
                           
                        }

                        //
                        // Continue to search next message
                        //
                        rcvBufferDataStartIndex = i + msgLen;
                        i = rcvBufferDataStartIndex;

                        goto ReadMessage;
                    }
                    else
                    {
                        // We return and wait for more data coming
                        return;
                    }
                }
            }

            // 
            // We only keep limited amount of data in case we only
            // receive junk data (this will happen when baud rate is not set correctly)
            //
            if (rcvBufferIndex - rcvBufferDataStartIndex > MAX_MESSAGE_SIZE)
                rcvBufferDataStartIndex = rcvBufferIndex - MAX_MESSAGE_SIZE;
        }
    }

    public class PMGFriscoConnection
    {
        private const int StatsHeaderRecordSize = 256;
        private const int StatsRecordSize = 32;

        public enum ConnectionType
        {
            SerialPort = 0,
            Bluetooth,
            Cellular,
            Unknown
        }

        public class StatsStatusInfo
        {
            public enum StatsTransferState
            {
                Idle,
                Transfering
            }

            public UInt32 numStatsStored = 0;
            public UInt32 numStatsFree = 0;
            public UInt32 timestampOfLastStatsOffload = 0;
            public byte percentageFull = 200;
            public byte percentageFree = 0;

            // Transfer State
            public UInt32 numStatsTransferred = 0;
            public StatsTransferState statsTransferState = StatsTransferState.Idle;
            public StatsRecordType lastReceivedRecordType = StatsRecordType.Unknown;
            public Boolean downloadCanceled = false;
            public DateTime downloadStartTime = DateTime.Now;

            //
            // When Stats Received message is sent, the counter will be set to 4.
            // Every callback of ProcessReceivedStatsRecords(), this counter will
            // be deducted by 1 until 0, which will results send last 'Stats Received'
            // again. 
            //
            public int lastStatsReceviedMsgSentCount = -1;
            public byte lastStatsReceivedmsgSendSeqNo = 1;

            public StatsStatusInfo() { }
            public void Reset()
            {
                numStatsStored = 0;
                percentageFull = 200;
                percentageFree = 0;
                numStatsFree = 0;
                timestampOfLastStatsOffload = 0;

                numStatsTransferred = 0;
                statsTransferState = StatsTransferState.Idle;
                lastReceivedRecordType = StatsRecordType.Unknown;
            }
        };

        public Object msgLock = new Object();

        // Regular AC Message Queue
        public Queue msgPMGQueue = new Queue();

        // Stats Message Queue
        public Queue statsRecordQueue = new Queue();
        public StatsStatusInfo currentStatsStatusOnPMG = new StatsStatusInfo();

        // MPPT Messsage Queue
        public Queue mpptMsgQueue = new Queue();

        private BinaryWriter currentOpendStatsFileWriter = null;

        private int timeoutCounter = 0;

        public const int UNDEFINED_CLOCK_TICK = -10000;

        public SerialPortConnection serialPortConnectionToPMG = null;
        public BLEConnection bleConnectionToPMG = null;
        public ServerConnection serverConnection = null;
        public ConnectionType currentConnectionType = ConnectionType.SerialPort;

        // 
        // This flag will be set to True if PCA is configuring or reloading
        // data from PMG
        //
        public Boolean isActivelyTalkingToPMG = false;

        public Boolean enableDebugMessageInfiniteWait = false;

        // =========================================
        public DateTime lastRequestParameterMapTime = DateTime.MinValue;
        public DateTime lastRequestClockTime = DateTime.MinValue;
        public DateTime lastRequestScheduledOperationTime = DateTime.MinValue;

        public DateTime lastWriteParameterMapTime = DateTime.MinValue;
        public DateTime lastWriteClockTime = DateTime.MinValue;
        public DateTime lastWriteScheduledOperationTime = DateTime.MinValue;

        public byte[] NewConfiguration;
        public string NewConfigurationTime;

        public PMDDisplaySize currentPMGDisplayType = PMDDisplaySize.TwelveInchPMD;
        public string currentPMGFirmwareVersion = string.Empty;
        public string currentPMGName = string.Empty;
        public long currentPMGClock = UNDEFINED_CLOCK_TICK;
        // This is the time difference between
        // PMG Clock and our local machine's clock

        public PMGSystemInfo currentPMGInfo = null;

        public List<PageInfo> currentPageFileListOnPMG = new List<PageInfo>();

        public List<ScheduledOperation> currentScheduledOperationFilesOnPMG =
                                                    new List<ScheduledOperation>();

        public List<byte> currentPageFileListOnPMGGroupDataReceived = new List<byte>();


        public int currentPageFileNumberReported = 0;




        public Boolean IsConnectedToPMG()
        {
            if (currentConnectionType == ConnectionType.SerialPort)
            {
                if (serialPortConnectionToPMG.IsConnectionOpened())
                    return true;
                else
                    return false;
            }
            else if (currentConnectionType == ConnectionType.Bluetooth)
            {
                //if (bleConnectionToPMG.IsConnectionOpened())
                //    return true;
                //else
                //    return false;
            }
            else if (currentConnectionType == ConnectionType.Cellular)
            {
                //// PENDING999: IsConnectedToPMG
                //if (serverConnection.IsConnectionOpened() &&
                //    serverConnection.currentPMGId != 0)
                //    return true;
                //else
                //    return false;
            }

            return false;
        }

        public void CloseConnections()
        {
            serialPortConnectionToPMG.CloseConnection();
            //bleConnectionToPMG.CloseConnection();
        }


        public EthernetStatus ProcessReceivedEthernetStatusMessage(PMGMessage msg, ref string errorReason)
        {
            #region
            byte[] payloadData = null;

            if (msg.GetCommandID() != PMGCommandID.EthernetStatus)
                return null;

            payloadData = msg.GetPayloadDataArray();

            if (payloadData == null || payloadData.Length < 14)
            {
                errorReason = "Wrong Ethenert Status Message Format!";
                return null;
            }

            EthernetStatus ethenetStatus = new EthernetStatus();

            ethenetStatus.connectionStatus = payloadData[0];
            ethenetStatus.ipAddress = Util.GetUint32(payloadData, 2);
            ethenetStatus.netMask = Util.GetUint32(payloadData, 6);
            ethenetStatus.gateway = Util.GetUint32(payloadData, 10);

            return ethenetStatus;

            #endregion
        }

        public WiFiStatus ProcessReceivedWiFiStatusMessage(PMGMessage msg, ref string errorReason)
        {
            #region
            byte[] payloadData = null;

            if (msg.GetCommandID() != PMGCommandID.WiFiStatus)
                return null;

            payloadData = msg.GetPayloadDataArray();

            //
            // MAC (6) + IP (4) + Net Mask (4) + Gateway (4) + Number Of Clients (1)
            //
            if (payloadData == null || payloadData.Length < 19)
            {
                errorReason = "Wrong WiFi Status Message Format!";
                return null;
            }

            WiFiStatus wifiStatus = new WiFiStatus();
            int idx = 0;

            wifiStatus.macAddress = Util.GetByteArray(payloadData, ref idx, 6);
            wifiStatus.ipAddress = Util.GetUint32(payloadData, idx); idx += 4;
            wifiStatus.netMask = Util.GetUint32(payloadData, idx); idx += 4;
            wifiStatus.gateway = Util.GetUint32(payloadData, idx); idx += 4;
            wifiStatus.numClient = payloadData[idx];

            return wifiStatus;

            #endregion
        }


        public void ResetStatsRecordsQueue()
        {
            statsRecordQueue.Clear();
        }

        public void CloseAndDeleteStatsFileWriter()
        {
            if (currentOpendStatsFileWriter == null)
                return;

            string filenameWithPath = (currentOpendStatsFileWriter.BaseStream as FileStream).Name;

            currentOpendStatsFileWriter.Close();
            currentOpendStatsFileWriter = null;

            Thread.Sleep(200); // Wait for file to be closed
            File.Delete(filenameWithPath);
        }


        // 
        // This function will be called every 100 mili-second
        //
      

        public Boolean IsPageExistOnPMG(PageTag tag)
        {
            string filename = tag.getPageFilename(false);

            if (GetPageInfoOnPMG(filename) != null)
                return true;
            else
                return false;
        }

        public Boolean IsPageExistOnPMG(string filename)
        {
            if (GetPageInfoOnPMG(filename) != null)
                return true;
            else
                return false;
        }

        public PageInfo GetPageInfoOnPMG(string pageFilename)
        {

            for (int i = 0; i < currentPageFileListOnPMG.Count; i++)
            {
                if (pageFilename.Equals(currentPageFileListOnPMG[i].filename))
                    return currentPageFileListOnPMG[i];
            }

            return null;
        }

        //
        // This function will check whether the page exists, if yes,
        // it will compare the hash value reported by PMG with the same
        // page filename. If it matches, the function will return true.
        // This is to avoid redundant page read if we have already have the
        // same page on PCA
        //
        public Boolean IsSamePageExistOnPCA(string pageFilename, ref Boolean pageExistOnPCA,
                                            ref Boolean pageExistOnPMG)
        {
            PageInfo pageInfoOnPMG = GetPageInfoOnPMG(pageFilename);
            PageTag currentPage = new PageTag(pageFilename);

            if (pageInfoOnPMG != null)
                pageExistOnPMG = true;
            else
                pageExistOnPMG = false;

            //
            // Get Page file from local pmg directory, if not found, 
            // we get it from installed page directory
            //
            PageFile pageFile = PageFile.CreatePageFile(currentPage);

            if (pageFile != null)
                pageExistOnPCA = true;
            else
                pageExistOnPCA = false;

            //
            // If page exist, we compare the hash with entry in the page list 
            // reported from PMG
            //
            if (pageFile != null && pageInfoOnPMG != null)
            {
                UInt16 hashValue = pageFile.getHashValue();

                if (pageInfoOnPMG.isSameHash(hashValue))
                    return true;
            }

            return false;
        }

        public Boolean IsSamePageExistOnPMG(PageFile pageFile)
        {
            PageInfo pageInfoOnPMG = GetPageInfoOnPMG(pageFile.getFilename());

            //
            // If page exist, we compare the hash with entry in the page list 
            // reported from PMG
            //     
            if (pageInfoOnPMG != null)
            {
                UInt16 hashValue = pageFile.getHashValue();

                if (pageInfoOnPMG.isSameHash(hashValue))
                    return true;
            }

            return false;
        }

        public Boolean IsSamePageExistOnPMG(ScheduledOperation scheduleOperation)
        {
            PageInfo pageInfoOnPMG = GetPageInfoOnPMG(scheduleOperation.getFilename());

            //
            // If page exist, we compare the hash with entry in the page list 
            // reported from PMG
            //     
            if (pageInfoOnPMG != null)
            {
                UInt16 hashValue = scheduleOperation.getHashValue();

                if (pageInfoOnPMG.isSameHash(hashValue))
                    return true;
            }

            return false;
        }
        public Boolean UpdatePageInfoOnPMG(string pageFilename, UInt16 newHashValue,
                                          Boolean removeEntry = false)
        {

            for (int i = 0; i < currentPageFileListOnPMG.Count; i++)
            {
                if (pageFilename.Equals(currentPageFileListOnPMG[i].filename))
                {
                    if (removeEntry)
                    {
                        currentPageFileListOnPMG.RemoveAt(i);
                        return true;
                    }
                    else
                    {
                        currentPageFileListOnPMG[i].hashLSB = (byte)(newHashValue & 0xFF);
                        currentPageFileListOnPMG[i].hashMSB = (byte)((newHashValue >> 8) & 0xFF);
                        return true;
                    }
                }
            }

            // To this point, the entry to be deleted is
            // not in the list
            if (removeEntry)
                return false;

            //
            // To this point, it is a new page
            //
            PageInfo newEntry = new PageInfo();

            newEntry.filename = pageFilename;
            newEntry.hashLSB = (byte)(newHashValue & 0xFF);
            newEntry.hashMSB = (byte)((newHashValue >> 8) & 0xFF);

            currentPageFileListOnPMG.Add(newEntry);
            return true;
        }


        // Send message to PMG
        public Boolean SendMessage(PMGCommandID commandId, byte[] packetData,
                                   ACFormatPacketProtocol.ReadWriteType type = ACFormatPacketProtocol.ReadWriteType.Write,
                                   byte targetSubsystemID = (byte)SubSystem_ID_t.SubSys_Controller,
                                   byte sourceSubsystemID = (byte)SubSystem_ID_t.SubSys_Application,
                                   byte seqNo = 0, byte packetNum = 1, byte totalPacketNumber = 1)
        {
            byte[] sendData = ACFormatPacketProtocol.FormatMessage(commandId, packetData, type,
                                                    targetSubsystemID, sourceSubsystemID, seqNo,
                                                    packetNum, totalPacketNumber);

            seqNo = sendData[5];

            if (!IsConnectedToPMG())
                return false;

            try
            {
                //
                // Before we send message to PMG,
                // we clear the received message queue
                //
                ResetPMGMessageQueue();

                SendData(sendData);      

            }
            catch (Exception)
            {
                //return false;
            }

            return true;
        }

        // Send message to PMG
        public Boolean SendData(byte[] data)
        {
            if (!IsConnectedToPMG())
                return false;

            try
            {
                ResetPMGMessageQueue();

                Boolean status = false;

                //if (currentConnectionType == ConnectionType.SerialPort)
                //    status = serialPortConnectionToPMG.SendData(data);
                //else if (currentConnectionType == ConnectionType.Bluetooth)
                //    status = bleConnectionToPMG.SendData(data);

                //// PENDING911
                //else if (currentConnectionType == ConnectionType.Cellular)
                //    status = serverConnection.SendDirectToPMG(data);

                return status;
            }
            catch (Exception)
            {
                //return false;
            }

            return false;
        }

        public Boolean SendReadPage(string filename)
        {
            List<byte> byteList = new List<byte>();

            if (filename.Length == 0)
                return false;

            byteList.Add(1); // Number of files
            byteList.Add((byte)(filename.Length + 1)); // file length

            byte[] data = Encoding.ASCII.GetBytes(filename);
            Util.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            //
            // Notes:
            // ReadWriteTextPage (Read) is obsoleted. However, to support the
            // the backward compability, we still use this message, instead
            // of the new ReadPageRequest
            //

            Boolean status;

            status = SendMessage(PMGCommandID.ReadPageRequest, byteList.ToArray(),
                                 ACFormatPacketProtocol.ReadWriteType.Read);

           

            return status;
        }

        public Boolean SendReadPages(string[] filenames)
        {
            List<byte> byteList = new List<byte>();
            byte[] data;

            if (filenames.Length == 0)
                return false;

            byteList.Add((byte)filenames.Length); // Number of files

            for (int i = 0; i < filenames.Length; i++)
            {
                byteList.Add((byte)(filenames[i].Length + 1)); // file length + 1 byte terminator

                data = Encoding.ASCII.GetBytes(filenames[i]);
                Util.AddArrayToList(ref byteList, data);
                byteList.Add(0);
            }

            Boolean status = SendMessage(PMGCommandID.ReadPageRequest, byteList.ToArray(),
                                         ACFormatPacketProtocol.ReadWriteType.Read);

            return status;
        }

        public Boolean SendDeletePage(string filename)
        {
            List<byte> byteList = new List<byte>();

            if (filename.Length == 0)
                return false;

            byteList.Add(1); // Number of files
            byteList.Add((byte)(filename.Length + 1)); // file length

            byte[] data = Encoding.ASCII.GetBytes(filename);
            Util.AddArrayToList(ref byteList, data);
            byteList.Add(0);

            Boolean status =
               SendMessage(PMGCommandID.DeletePages, byteList.ToArray(),
                                                 ACFormatPacketProtocol.ReadWriteType.Write);

            return status;
        }

        public Boolean SendRequestReset()
        {
            Boolean status =
               SendMessage(PMGCommandID.RequestReset, null,
                           ACFormatPacketProtocol.ReadWriteType.Write);

            return status;
        }

        public Boolean SendStatsCommand(StatsCommandType cmdType)
        {
            byte[] payloadData = new byte[5];

            payloadData[0] = (byte)cmdType;
            payloadData[1] = 0;
            payloadData[2] = 0;
            payloadData[3] = 0;
            payloadData[4] = 0;

            Boolean status =
               SendMessage(PMGCommandID.StatsRecordCommand, payloadData,
                           ACFormatPacketProtocol.ReadWriteType.Write);

            return status;
        }

        public Boolean SendStatsReceivedCommand(byte seqNo)
        {
            byte[] payloadData = new byte[5];

            payloadData[0] = (byte)StatsCommandType.StatsReceived;
            payloadData[1] = seqNo;
            payloadData[2] = 0;
            payloadData[3] = 0;
            payloadData[4] = 0;

            Boolean status =
               SendMessage(PMGCommandID.StatsRecordCommand, payloadData,
                           ACFormatPacketProtocol.ReadWriteType.Write);

            return status;
        }

        public Boolean SendGenerateTestStatsCommand(UInt16 numRecords)
        {
            byte[] payloadData = new byte[5];

            payloadData[0] = (byte)StatsCommandType.GenereateTestStats;
            payloadData[1] = 0x00; // Reserved_1
            payloadData[2] = (byte)(numRecords & 0xff);
            payloadData[3] = (byte)((numRecords >> 8) & 0xff); ;
            payloadData[4] = 0x00; // Reserved_2;

            Boolean status =
               SendMessage(PMGCommandID.StatsRecordCommand, payloadData,
                           ACFormatPacketProtocol.ReadWriteType.Write);

            return status;
        }

        public Boolean SendAckMessage(byte seqNo, PMGCommandID commandToAck, byte statusCode,
                                byte cmdSpecificStatus, byte packetNumBeingAcked = 0xFF)
        {
            byte[] payloadData;

            payloadData = new byte[6];

            payloadData[0] = (byte)commandToAck;
            payloadData[1] = seqNo;
            payloadData[2] = statusCode;
            payloadData[3] = cmdSpecificStatus;
            payloadData[4] = packetNumBeingAcked;
            payloadData[5] = 0x00;

            Boolean status = SendMessage(PMGCommandID.Ack, payloadData,
                                         ACFormatPacketProtocol.ReadWriteType.Write);

            return status;
        }

        public Boolean RequestToDeleteExistingScheduledOperationsOnPMG()
        {
            for (int i = 0; i < currentScheduledOperationFilesOnPMG.Count; i++)
            {
                SendDeletePage(currentScheduledOperationFilesOnPMG[i].getFilename());
                Thread.Sleep(250);
            }

            return true;
        }

        public Boolean RemovePMDNewConfiguration()
        {
            if (NewConfiguration != null)
            {
                Array.Clear(NewConfiguration, 0,
                            NewConfiguration.Length);

                NewConfigurationTime = Utils.GetCurrentDateTime();

                //ACIMntServer.Server.DBUpdatePMGNewConfiguration(this);

                return true;
            }

            return false;
        }


        // This is for debugging purpose
        public byte[] GetEncodedSendMessageData(PMGCommandID commandId, byte[] packetData,
                                   ACFormatPacketProtocol.ReadWriteType type = ACFormatPacketProtocol.ReadWriteType.Write,
                                   byte targetSubsystemID = (byte)SubSystem_ID_t.SubSys_Controller,
                                   byte sourceSubsystemID = (byte)SubSystem_ID_t.SubSys_Application,
                                   byte seqNo = 0, byte packetNum = 1, byte totalPacketNumber = 1)
        {
            byte[] sendData = ACFormatPacketProtocol.FormatMessage(commandId, packetData, type,
                                                    targetSubsystemID, sourceSubsystemID, seqNo,
                                                    packetNum, totalPacketNumber);

            seqNo = sendData[5];

            try
            {
               

            }
            catch (Exception)
            {
                return null;
            }

            return sendData;
        }

   

        private void ResetPMGMessageQueue()
        {
            lock (msgLock)
            {
                msgPMGQueue.Clear();
            }
        }

        public PMGMessage[] GetResponseMessageFromPMG(int waitTime, int numberOfTrys = 1,
                                                      PMGCommandID msgWaitingFor = PMGCommandID.Unknown,
                                                      PMGCommandID cmdToAck = PMGCommandID.Unknown)
        {
            int tryTimes;
            PMGMessage[] msgArray;
            int msgCount, i;

            if (enableDebugMessageInfiniteWait && msgWaitingFor != PMGCommandID.Unknown)
            {
               
            }

            for (tryTimes = 0; tryTimes < numberOfTrys; tryTimes++)
            {
                Thread.Sleep(waitTime);

              
                if (enableDebugMessageInfiniteWait)
                    tryTimes = 0;

                lock (msgLock)
                {
                    //
                    // If we are not waitting for specific message, we will return
                    // if there is any messages we receive
                    //
                    if (msgWaitingFor == PMGCommandID.Unknown)
                    {
                        if (msgPMGQueue.Count > 0)
                        {
                            msgCount = msgPMGQueue.Count;
                            msgArray = new PMGMessage[msgCount];

                            for (i = 0; i < msgCount; i++)
                                msgArray[i] = (PMGMessage)msgPMGQueue.Dequeue();

                            return msgArray;
                        }
                    }
                    else
                    {
                        if (msgPMGQueue.Count > 0)
                        {
                            msgCount = msgPMGQueue.Count;
                            msgArray = new PMGMessage[msgCount];

                            for (i = 0; i < msgCount; i++)
                                msgArray[i] = (PMGMessage)msgPMGQueue.Dequeue();

                            for (i = 0; i < msgCount; i++)
                            {
                                //
                                // If we find the response message we wait for,
                                // we return. Otherwise, we keep waiting until timeout
                                //
                                if (cmdToAck == PMGCommandID.Unknown)
                                {
                                    if (msgArray[i].GetCommandID() == msgWaitingFor)
                                    {
                                        PMGMessage[] msgArray2 = new PMGMessage[1];
                                        msgArray2[0] = msgArray[i];

                                        return msgArray2;
                                    }
                                }
                                else
                                {
                                    if (msgArray[i].GetCommandID() == msgWaitingFor &&
                                        msgArray[i].GetWhichCommandToAck() == cmdToAck)
                                    {
                                        PMGMessage[] msgArray2 = new PMGMessage[1];
                                        msgArray2[0] = msgArray[i];

                                        return msgArray2;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        public PMGMessage[] GetGroupResponseMessageFromPMG(int waitTime, int numberOfTrys = 1,
                                                         PMGCommandID msgWaitingFor = PMGCommandID.Unknown)
        {
            int tryTimes;

            List<PMGMessage> msgList = new List<PMGMessage>();

            if (enableDebugMessageInfiniteWait && msgWaitingFor != PMGCommandID.Unknown)
            {
              
            }

            for (tryTimes = 0; tryTimes < numberOfTrys; tryTimes++)
            {
                Thread.Sleep(waitTime);
               

                if (enableDebugMessageInfiniteWait)
                    tryTimes = 0;

                lock (msgLock)
                {
                    if (msgPMGQueue.Count > 0)
                    {
                        int msgCount = msgPMGQueue.Count;

                        for (int i = 0; i < msgCount; i++)
                        {
                            PMGMessage msg = (PMGMessage)msgPMGQueue.Dequeue();

                            if (msgWaitingFor == PMGCommandID.Unknown ||
                                msgWaitingFor == msg.GetCommandID())
                                msgList.Add(msg);
                            else
                                continue;

                            // Check if it is the last group message
                            if (msg.GetPacketNumber() == msg.GetTotalPacketNumber())
                            {
                                if (msgList.Count == msg.GetTotalPacketNumber())
                                    return msgList.ToArray();
                                else
                                {
                                    // We receive last packet, however, we lost at least
                                    // one group message packet
                                    return null;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        // Combine payload for each individual messages
        public PMGMessage ProcessGroupMessages(PMGMessage[] rspMessages)
        {
            List<byte> combinedPayload = new List<byte>();

            if (rspMessages == null || rspMessages.Length == 0)
                return null;

            //
            // If it is single AC message, no need to combine
            //
            if (rspMessages.Length == 1)
                return rspMessages[0];

            //
            // Add Header Portion of first message
            //
            Util.AddArrayToList(ref combinedPayload, rspMessages[0].data, 10);

            //
            // Add payload of each individual group message
            //
            for (int i = 0; i < rspMessages.Length; i++)
            {
                byte[] payload = rspMessages[i].GetPayloadDataArray();
                Util.AddArrayToList(ref combinedPayload, payload);
            }

            int payloadDataLen = combinedPayload.Count - 10;

            combinedPayload[8] = (byte)payloadDataLen;        // Payload Length LSB
            combinedPayload[9] = (byte)(payloadDataLen >> 8); // Payload Length MSB

            byte[] msgData = combinedPayload.ToArray();

            PMGMessage msg = new PMGMessage(msgData);

            return msg;
        }

        public PMGMessage ProcessGroupCompositeMessages(PMGMessage[] rspMessages)
        {
            List<byte> combinedPayload = new List<byte>();
            byte[] payload;

            if (rspMessages == null || rspMessages.Length == 0)
                return null;

            //
            // Add Header Portion of first message
            //
            Util.AddArrayToList(ref combinedPayload, rspMessages[0].data, 10);

            //
            // We convert multi packets new Write Composite Sequence messages into single
            // Composite Sequence message (with no reserved bytes on so on)
            //
            for (int i = 0; i < rspMessages.Length; i++)
            {
                if (rspMessages[i] == null)
                    break;

                if (rspMessages[i].GetPacketNumber() == 1)
                {
                    //
                    // Compared with 0x35, we have extra Total Segments Size (2 bytes) + 3 Reseved + 1 (Num Segs in Packet)
                    // which is total 6 bytes, which we need to remove
                    //
                    payload = rspMessages[i].GetPayloadDataArray();

                    // 
                    // Front part which we need:
                    // Hash (2) + Filename Length (1) + Filename + Total Num Segments (1) + Num Cycles (1)
                    //
                    int len = 2 + 1 + payload[2] + 1 + 1;

                    Util.AddArrayToList(ref combinedPayload, payload, 0, len);

                    // We need to skip extra 6 bytes introduced here
                    Util.AddArrayToList(ref combinedPayload, payload, len + 6, payload.Length - len - 6);
                }
                else
                {
                    // We just need to skip first 4 bytes (Num Segments and 3 Reserved byte)
                    payload = rspMessages[i].GetPayloadDataArray();

                    Util.AddArrayToList(ref combinedPayload, payload, 4, payload.Length - 4);
                }
            }

            combinedPayload[2] = (byte)PMGCommandID.ReadWriteCompositePage;

            int payloadDataLen = combinedPayload.Count - 10;

            combinedPayload[8] = (byte)payloadDataLen;        // Payload Length LSB
            combinedPayload[9] = (byte)(payloadDataLen >> 8); // Payload Length MSB

            byte[] msgData = combinedPayload.ToArray();

            PMGMessage msg = new PMGMessage(msgData);

            return msg;
        }

        public PMGMessage[] GetMTTPMessageFromPMG()
        {

            lock (msgLock)
            {
                if (mpptMsgQueue.Count > 0)
                {
                    int msgCount = mpptMsgQueue.Count;

                    PMGMessage[] msgArray = new PMGMessage[msgCount];

                    for (int i = 0; i < msgCount; i++)
                    {
                        msgArray[i] = (PMGMessage)mpptMsgQueue.Dequeue();
                    }

                    return msgArray;
                }
            }

            return null;
        }

     

   
    }


}
