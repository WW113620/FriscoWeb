using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static PMDCellularInterface.PMDConfiguration;

using uint8_t = System.Byte;
using uint32_t = System.UInt32;
using int16_t = System.Int16;
using uint16_t = System.UInt16;
using int8_t = System.Byte;

namespace PMDInterface
{
    public class ServerConnection
    {
        public static string serverIP = "138.91.73.155";
        public static UdpClient udpClient = null;

        public static bool SendDataToServer(TableID tableId, NotificationType type, long transactionID, string imsi)
        {
            if (udpClient == null)
            {
                udpClient = new UdpClient(12000);
                udpClient.Connect(serverIP, 37002);
            }
            try
            {
                byte[] key = Encoding.ASCII.GetBytes(imsi);
                byte[] payload = FormatMessage(tableId, type, transactionID, key);

                int i = udpClient.Send(payload, payload.Length);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        //
        // Formating Web Notification message sent to Server
        // key is the 20 bytes IMSI number for all PMG related operation
        // key is username for account related operation

        public static byte[] FormatMessage(TableID tableId, NotificationType type,
                                           long transactionID, byte[] key)
        {
            // First, decide the message length and whether we should pad 0
            //     Header Part (10 bytes) + TableId (1 byte) + Type (1 byte) + 
            //     TransactionId (8 bytes) + Key + CS (2 bytes)
            // 
            int msgLen;
            int payloadDataLen;

            msgLen = 10 + 1 + 1 + 8 + key.Length + 2;
            payloadDataLen = 10 + key.Length;

            // Pad 0 if needed
            if (msgLen % 2 != 0)
            {
                payloadDataLen++;
                msgLen += 1;
            }

            byte[] msgData = new byte[msgLen];

            //
            // Encode header portion (10 bytes)
            //
            msgData[0] = 0x41;
            msgData[1] = 0x43;
            msgData[2] = (byte)PMGCommandID.Notification;
            msgData[3] = 0;
            msgData[4] = 0;
            msgData[5] = 0;
            msgData[6] = 1;     // Packet Number
            msgData[7] = 1;     // Total Number Of Packets

            msgData[8] = (byte)payloadDataLen;        // Payload Length LSB
            msgData[9] = (byte)(payloadDataLen >> 8); // Payload Length MSB

            msgData[10] = (byte)tableId;
            msgData[11] = (byte)type;

            // 8 Bytes transaction ID
            byte[] tranIdData = BitConverter.GetBytes(transactionID);
            Buffer.BlockCopy(tranIdData, 0, msgData, 12, tranIdData.Length);

            // Key 
            Buffer.BlockCopy(key, 0, msgData, 20, key.Length);

            // Calculate Checksum
            uint16_t cs = U16ComputeCRC(msgData, 0, msgLen - 2);

            msgData[msgLen - 1] = (byte)(cs & 0xFF);
            msgData[msgLen - 2] = (byte)((cs >> 8) & 0xFF);

            return msgData;
        }

        static public uint16_t U16ComputeCRC(byte[] data, int startIdx, int len)
        {
            int u8Bit, i;
            uint16_t u16CRC = 0xFFFF;
            uint16_t u16Odd;

            for (i = startIdx; i < startIdx + len; i++)
            {
                u16CRC ^= ((uint16_t)(data[i] << 8));

                for (u8Bit = 0; u8Bit < 8; u8Bit++)
                {
                    u16Odd = (uint16_t)(u16CRC & 0x8000);

                    u16CRC <<= 1;

                    if (u16Odd == 0x8000)
                    {
                        u16CRC ^= 0x1021;  //C13 + C6 + C1
                    }
                }
            }

            return (u16CRC);
        }

        public enum PMGCommandID
        {
            Unknown = 0x00,

            ConnectAgent = 0x0F,

            Registration = 0x10,

            DeviceSystemInfo_Deprecated = 0x11,

            DeviceSystemInfo = 0x13,

            SerialNumber = 0x14,

            ReadWritePageIDList = 0x1F,

            WriteParameter = 0x12,
            KeepAlive = 0x16,
            Status = 0x18,

            WriteDateTime = 0x1C,

            ReadPageRequest = 0x26,

            WriteFeatureEnableKey = 0x17,

            EthernetStatus = 0x1A,

            uSDFileUpdate = 0x1D,
            WiFiStatus = 0x1E,

            // New Commands
            ReadWriteCalendar = 0x30,

            ReadWriteTextPage = 0x32,
            ReadWriteGrahicPage = 0x33,
            ReadWriteAnimationPage = 0x34,

            ReadWriteCompositePage = 0x37,

            ReadWriteScheduledOperationDeprecated = 0x29,

            ReadWriteScheduledOperation = 0x2A,  // New command with Calendar

            USBLoggingEnable = 0x55,
            StatsRecordCommand = 0xBC,
            Ack = 0x98,

            Deprecated_ReadWriteTextPage = 0x22,
            Deprecated_ReadWriteGrahicPage = 0x23,
            Deprecated_ReadWriteAnimationPage = 0x24,
            Deprecated_ReadWriteSchledOperation = 0x28,

            DeletePages = 0x25,

            RequestReset = 0x92,
            GeneralAck = 0x99,  // Deprecated

            AddRemote = 0xA4,
            PurgeRemote = 0xA5,

            PowerDataRequest = 0xCC,
            PowerMonitor = 0xCD,
            PowerSpecsDeprecated = 0xCE,
            PowerSpecs = 0xCF,

            FirmwareUpdateUpload = 0xBE,
            FirmwareUpdateInstall = 0xBF,
            FirmwareUpdateResult = 0xC0,

            FactoryReset = 0xFF,

            PMGStartUpNoification = 0xDD,

            PMGSetUSBLogging = 0x55,


            // Debug portion
            RegisterServerLogTrace = 0x81,
            UnRegisterServerLogTrace = 0x82,
            ServerLogTrace = 0x83,
            ProcessHeartBeat = 0x84,
            ProcessHeartBeatAck = 0x85,
            Notification = 0x86
        };


    }
}
