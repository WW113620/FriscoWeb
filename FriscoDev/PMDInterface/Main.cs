using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

/*** NOTE:
  
  In PMD table, Column CurrentConfiguration store the existing configuration for the PMD.

  To display the existing configuration for the selected PMD, create a new object 
  PMDConfiguration(byte [] data) (data is the 'CurrentConfiguration' column, 278 byte binary data). Then
  The member data 'pmdSetting' and 'pmdCalendar' of  PMDConfiguration has all information needed
  for GUI display

  To configure PMD, user can change the setting using GUI, the web should change the member data pmdSetting
  and pmdCalendar using the api provided in PMDSetting() and CalendarData() class. When user clicks button to
  configure the PMD, Web interface should call function ToData() of PMDConfiguration to pack the 
  modified 'pmdSetting' and 'pmdCalendar' to binary data (should also be 278 bytes). The output binary data
  should be stored in column NewConfiguration.

  When server detects the new configuration for the PMD, it would send it to PMD when the PMD is online and
  copy the NewConfiguration to CurrentConfiguration and set the NewConfiguration column to empty.
  
  Testcase 1:
  ==========

  (1) Create a new PMD
  (2) Create a PMDConfiguration() with default empty data
  (3) Display the PMDConfiguration()
  (4) Use GUI to modify setting
  (5) Call ToData() of PMDConfiguration and store the data to CurrentConfiguration column
  (6) Read the CurrentConfiguration from database and and display the content to see if
      it matches the previous modification

 */

namespace PMDCellularInterface
{
    public enum TimerMode
    {
        Off = 0,
        Period,
        Daily,
        SelectedDays,
        AlwaysOn,
    };

    public enum TimerFunctionType
    {
        SpeedDisplay = 0,
        SpeedLimit,
        TrafficStatistics,
        Message1,
        Message2,
        Message3
    };

    public enum TimerCalendarControl
    {
        Off = 0,
        On = 1,
    };

    public class CalendarData
    {
        public const int CALDATA_SIZE = 46;

        public DateTime startDate = DateTime.Now;

        //
        // Calendar data will keep event day info for 365 days
        // beginning from the start date. Each day is represented
        // by 1 bit. 46 bytes can represent up to 365 * 8 = 368
        //
        //
        public byte[] value = new byte[CALDATA_SIZE];

        public DateTime daylightSavingStartDate = DateTime.Now;
        public DateTime daylightSavingEndDate = DateTime.Now;
        public Boolean enableDaylightSavingTime = false;

        public Boolean calendarEnabled = true;

        public CalendarData()
        {
        }

        public CalendarData(byte[] data)
        {
            if (data.Length != 59)
            {
                Console.WriteLine("*** Error, Calendar Data length = " + data.Length);
                return;
            }

            if (data[0] == 0x00)
            {
                startDate = DateTime.Now;
                for (int i = 0; i < CALDATA_SIZE; i++)
                    value[i] = 0;
            }
            else
            {
                startDate = Utility.CreateDateTime(data[3], data[4], data[5]);

                for (int i = 0; i < CALDATA_SIZE; i++)
                    value[i] = data[i + 6];
            }

            // Get daylightving date
            int idx = 3 + 3 + CALDATA_SIZE;

            daylightSavingStartDate = Utility.CreateDateTime(data[idx], data[idx + 1], data[idx + 2]);
            daylightSavingEndDate = Utility.CreateDateTime(data[idx + 3], data[idx + 4], data[idx + 5]);

            if (data[idx + 6] == 0x01)
                enableDaylightSavingTime = true;
            else
                enableDaylightSavingTime = false;
        }

        Boolean HasEventDays()
        {
            for (int i = 0; i < CALDATA_SIZE; i++)
            {
                if (value[i] != 0x00)
                    return true;
            }

            return false;
        }

        // Get all event days from Calendar data
        public List<DateTime> GetAllEventDays()
        {
            int i, j;

            byte mask;
            DateTime eventDate;
            int numExtraDays = 0;

            List<DateTime> eventDays = new List<DateTime>();

            for (i = 0; i < value.Length; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    mask = (byte)(1 << (7 - j));

                    if ((value[i] & mask) != 0x00)
                    {
                        numExtraDays = (8 * i) + j;

                        eventDate = startDate.AddDays(numExtraDays);

                        eventDays.Add(eventDate);
                    }
                }
            }

            return eventDays;
        }

        // Set all event days to Calendar data
        public void SetAllEventDays(List<DateTime> eventDays)
        {
            int i;
            int byteIndex, bitIndex;
            int numDays = 0;

            // Clear all existing data
            for (i = 0; i < value.Length; i++)
                value[i] = 0x00;

            DateTime endDay = startDate.AddDays(365);

            for (i = 0; i < eventDays.Count; i++)
            {
                if (eventDays[i].Date < startDate || eventDays[i].Date > endDay)
                    continue;

                numDays = (int)((eventDays[i].Date - startDate).Days);

                byteIndex = numDays / 8;
                bitIndex = (numDays % 8);

                value[byteIndex] |= (byte)(1 << (7 - bitIndex));
            }
        }

        public byte[] ToData()
        {

            byte[] data = new byte[3 + 3 + CALDATA_SIZE + 3 + 3 + 1]; // Total 59 bytes
            int i, idx = 0;

            if (calendarEnabled)
                data[idx++] = 0x01;
            else
                data[idx++] = 0x00;

            data[idx++] = 0x00;     // calONOFF
            data[idx++] = 0x00;     // calReset

            data[idx++] = (byte)(startDate.Month);
            data[idx++] = (byte)(startDate.Day);
            data[idx++] = (byte)(startDate.Year % 1000);

            for (i = 0; i < CALDATA_SIZE; i++)
                data[idx++] = value[i];

            data[idx++] = (byte)(daylightSavingStartDate.Month);
            data[idx++] = (byte)(daylightSavingStartDate.Day);
            data[idx++] = (byte)(daylightSavingStartDate.Year % 1000);

            data[idx++] = (byte)(daylightSavingEndDate.Month);
            data[idx++] = (byte)(daylightSavingEndDate.Day);
            data[idx++] = (byte)(daylightSavingEndDate.Year % 1000);

            if (enableDaylightSavingTime)
                data[idx++] = 0x01;
            else
                data[idx++] = 0x00;

            return data;
        }

    }

    public class TimerSetting
    {
        public TimerFunctionType functionType;
        public TimerMode modeType;
        public DateTime startDate;
        public DateTime startTime;
        public DateTime stopDate;
        public DateTime stopTime;
        public byte days;
        public int speedLimit;
        public TimerCalendarControl linkToCalendar;
        public string daysName;

        public TimerSetting()
        {
            modeType = TimerMode.Off;
            functionType = TimerFunctionType.SpeedDisplay;
            days = 0x00;
            speedLimit = 90;
            linkToCalendar = TimerCalendarControl.Off;

            startDate = DateTime.Now;
            startTime = DateTime.Now;
            stopDate = DateTime.Now;
            stopTime = DateTime.Now;

            startTime = startTime.AddSeconds(-startTime.Second);
            stopTime = stopTime.AddSeconds(-stopTime.Second);
        }

        //
        // Array of size 7
        //
        // Monday is the 1st item
        // Sunday is the last item
        //
        // False -- Not selected
        // True -- Selected
        //
        public Boolean SetDays(Boolean[] daySettings)
        {
            if (daySettings.Length != 7)
                return false;

            days = 0x00;

            for (int i = 0; i < daySettings.Length; i++)
            {
                if (!daySettings[i])
                    continue;

                days |= (byte)(1 << (7 - i));
            }

            return true;
        }

        public Boolean[] GetDays()
        {
            Boolean[] daySettings = new Boolean[7];
            byte mask;

            for (int i = 0; i < daySettings.Length; i++)
            {
                mask = (byte)(1 << (7 - i));

                if ((days & mask) == 0x00)
                    daySettings[i] = false;
                else
                    daySettings[i] = true;
            }

            return daySettings;
        }


        static public TimerFunctionType GetTimerFunctionType(byte data)
        {
            if (data >= 0 && data <= 5)
                return (TimerFunctionType)data;
            else
                return 0;
        }

        static public TimerCalendarControl GetCalendarControl(byte data)
        {
            if (data >= 0 && data <= 5)
                return (TimerCalendarControl)data;
            else
                return TimerCalendarControl.Off;
        }

        static public TimerMode GetTimerModeType(byte data)
        {
            if (data >= 0 && data <= 4)
                return (TimerMode)data;
            else
                return TimerMode.Off;
        }
    };

    public class Utility
    {
        public static string GetStringValue(byte[] value, int maxChars, Boolean removeTrailingSpace)
        {
            if (value == null)
                return string.Empty;

            int len = value.Length;

            if (maxChars != 0)
            {
                if (value.Length > maxChars)
                {
                    len = maxChars;
                }
            }

            //
            // Replace non-printable ascii as space
            //
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] < 0x20 || value[i] > 127)
                    value[i] = 0x20;
            }

            string s = Encoding.UTF8.GetString(value, 0, len);

            if (removeTrailingSpace)
                s = s.TrimEnd();

            return s;
        }

        public static DateTime CreateDateTime(byte month, byte day, byte year)
        {
            int year2 = 2000 + year;

            if (month > 12 || month <= 0 || day <= 0 || day > 31)
                return new DateTime(1900, 1, 1);

            return new DateTime(year2, (int)month, (int)day);
        }

        public static int GetLast7Digits(string id)
        {
            int len = id.Length;

            if (len > 7)
                id = id.Remove(0, len - 7);

            int value = Convert.ToInt32(id);

            return value;
        }
    }

    public class PMDSetting
    {
        public TimerSetting[] timers = new TimerSetting[5];

        public int minSpeed = 15;
        public int maxSpeed = 90;
        public int alertFlashType;
        public int alertMessageType;
        public int dataFormat;

        public string[] topMsgs = new string[3];
        public string[] bottomMsgs = new string[3];

        public Boolean useDaylightSavingTime = false;
        public DateTime daylightSavingTimeStart;
        public DateTime daylightSavingTimeEnd;

        public PMDSetting()
        {
            int i;

            for (i = 0; i < 5; i++)
                timers[i] = new TimerSetting();


            for (i = 0; i < 3; i++)
            {
                topMsgs[i] = string.Empty;
                bottomMsgs[i] = string.Empty;
            }
        }

        public int GetTimerSpeedLimit(byte data)
        {
            if (data >= 1 && data <= 99)
                return (int)data;
            else
                return 65;
        }

        public int GetAlertFlashType(byte data)
        {
            if (data >= 0 && data <= 3)
                return (int)data;
            else
                return 0;
        }

        public int GetDataFormat(byte data)
        {
            if (data >= 0 && data <= 2)
                return (int)data;
            else
                return 0;
        }

        public int GetAlertMessage(byte data)
        {
            if (data >= 0 && data <= 3)
                return (int)data;
            else
                return 3;
        }
    };

    public class PMD
    {
        static public SqlConnection dbConn = null;

        // Member data
        private string PMDName { get; set; }
        private string IMSI { get; set; }
        private string UserName { get; set; }
        private string Location { get; set; }
        private string Address { get; set; }
        private Boolean StatsCollection { get; set; }
        private Boolean Connection { get; set; }

        public string dump()
        {
            return PMDName + "," + IMSI + "," + UserName + "," + Location + "," +
                   Address + "," + StatsCollection + "," + Connection;
        }
        public PMD()
        {
        }

        //
        // NOTE2: Retrieve All PMDs belong to the user (Get Device List for the user)
        //        If no PMDs belong to the user, the returned value will be null
        // 
        public static List<PMD> GetPMDList(string username)
        {
            if (dbConn == null)
            {
                dbConn = GetDefaultSqlConnection();
                if (dbConn == null)
                    return null;
            }

            SqlCommand cmd = new SqlCommand("GetPMDForUser", dbConn);
            SqlDataReader rdr = null;

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", username);

            List<PMD> pmdList = new List<PMD>();

            // execute the command
            try
            {
                rdr = cmd.ExecuteReader();
                if (rdr == null)
                    return null;

                while (rdr.Read())
                {
                    PMD pmd = new PMD();

                    if (!rdr.IsDBNull(0))
                        pmd.PMDName = rdr.GetString(0);

                    pmd.IMSI = rdr.GetString(1);

                    if (!rdr.IsDBNull(2))
                        pmd.Address = rdr.GetString(2);

                    pmd.UserName = username;

                    if (!rdr.IsDBNull(3))
                        pmd.Location = rdr.GetString(3);

                    pmd.Connection = rdr.GetBoolean(4);
                    pmd.StatsCollection = rdr.GetBoolean(5);

                    pmdList.Add(pmd);
                }
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();
            }

            return pmdList;
        }

        //
        // NOTE2: Add a new PMD
        // 
        // (1) If there is no address, address parameter is set to null
        // (2) If there is no location, location parameter is set to null
        //     In the PMD table, column 'Location' will store coordinate in X,Y format.   
        //     Location Format: CoordiateX,Coordinate Y (For example, 33.013444,-93.444444)
        //
        static public Boolean AddPMD(string pmdName, string imsi, string username,
                                     Boolean statsCollection, string location, string address)
        {
            if (dbConn == null)
            {
                dbConn = GetDefaultSqlConnection();
                if (dbConn == null)
                    return false;
            }

            SqlCommand cmd = new SqlCommand("PMDAdd", dbConn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@pmdName", pmdName);
            cmd.Parameters.AddWithValue("@IMSI", imsi);
            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@StatsCollection", statsCollection);
            cmd.Parameters.AddWithValue("@pmdId", Utility.GetLast7Digits(imsi));

            if (location != null || location != string.Empty)
                cmd.Parameters.AddWithValue("@Location", location);

            if (address != null || address != string.Empty)
                cmd.Parameters.AddWithValue("@Address", address);

            if (dbConn.State == ConnectionState.Closed)
                dbConn.Open();

            Boolean status = DBExecuteNonQueryCommand(cmd);

            // If we sucessfully add the PMD, we send notification to cloud server
            if (status)
            {
                // Notify the change of configuration
                PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMG,
                                                 PMDConfiguration.DatabaseOperationType.Insert, imsi);
            }

            return status;
        }


        //
        // NOTE2: Remove a PMD
        //
        static public Boolean RemovePMD(string imsi)
        {
            if (dbConn == null)
            {
                dbConn = GetDefaultSqlConnection();
                if (dbConn == null)
                    return false;
            }

            SqlCommand cmd = new SqlCommand("PMDRemove", dbConn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pmdId", Utility.GetLast7Digits(imsi));

            if (dbConn.State == ConnectionState.Closed)
                dbConn.Open();

            Boolean status = DBExecuteNonQueryCommand(cmd);

            // If we sucessfully add the PMD, we send notification to cloud server
            if (status)
            {
                // Notify the change of configuration
                PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMG,
                                                 PMDConfiguration.DatabaseOperationType.Delete, imsi);
            }

            return status;
        }


        //
        // NOTE2: Modify PMD
        // 
        // (1) If there is no address, address parameter is set to null
        // (2) If there is no location, location parameter is set to null
        //     In the PMD table, column 'Location' will store coordinate in X,Y format.   
        //     Location Format: CoordiateX,Coordinate Y (For example, 33.013444,-93.444444)
        //
        static public Boolean UpdatePMD(string pmdName, string imsi, string username,
                                        Boolean statsCollection, string location, string address)
        {
            if (dbConn == null)
            {
                dbConn = GetDefaultSqlConnection();
                if (dbConn == null)
                    return false;
            }

            SqlCommand cmd = new SqlCommand("PMDUpdate", dbConn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@pmdName", pmdName);
            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@StatsCollection", statsCollection);

            cmd.Parameters.AddWithValue("@pmdId", Utility.GetLast7Digits(imsi));

            if (location != null || location != string.Empty)
                cmd.Parameters.AddWithValue("@Location", location);

            if (address != null || address != string.Empty)
                cmd.Parameters.AddWithValue("@Address", address);

            if (dbConn.State == ConnectionState.Closed)
                dbConn.Open();

            Boolean status = DBExecuteNonQueryCommand(cmd);

            // If we sucessfully add the PMD, we send notification to cloud server
            if (status)
            {
                // Notify the change of configuration
                PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMG,
                                                 PMDConfiguration.DatabaseOperationType.Update, imsi);
            }

            return status;
        }

        static private SqlConnection GetDefaultSqlConnection()
        {
            string connetionString =
               "Data Source = pxvajz11qz.database.windows.net; Initial Catalog = Abilene; Integrated Security = False; Persist Security Info = True; User ID = React; Password = aci2609@; Connect Timeout = 15; Encrypt = True; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";

            SqlConnection conn = new SqlConnection(connetionString);

            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDefaultSqlConnection Error: " + ex);
                return null;
            }

            return conn;
        }

        private static Boolean DBExecuteNonQueryCommand(SqlCommand cmd)
        {

            for (int i = 0; i < 2; i++)
            {
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    // SQL connection timeout, we need to re-connect
                    if (ex.Message.Contains("A transport-level error"))
                    {
                        if (cmd.Connection.State == ConnectionState.Closed)
                            cmd.Connection.Open();

                        if (i == 0)
                            continue;
                        else
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }

            }

            return false;
        }
    };

    public class PMDConfiguration
    {
        public PMDSetting pmdSetting = new PMDSetting();
        public CalendarData pmdCalenar = new CalendarData();

        public Boolean isValid { get; set; }

        public static UdpClient udpClient = null;
        public const string cloudServerIP = "138.91.73.155";

        //public const string cloudServerIP = "47.187.169.87";
        public const int webServerId = 80000;
        public const int clockUpdateId = 100;

        public enum DatabaseOperationType
        {
            Insert = 0,
            Update,
            Delete,
            ClockUpdate,
            ConfigurationUpdate
        };

        public enum NotificationType
        {
            Insert = 0,
            Update,
            Delete,
            ResetPMG,
            SetClock,
            FactoryResetPMG
        };


        public enum TableID
        {
            PMG = 0,
            Account,
            Connection,
            Message
        };


        public PMDConfiguration()
        {
        }

        public PMDConfiguration(byte[] data)
        {
            int idx = 0;
            int i;

            if (data.Length != 278)
            {
                Console.WriteLine("setConfigurationFromFlashFormatData Decode Error");
                return;
            }

            // ======= Confiugure Portion Length: 7=======
            pmdSetting.maxSpeed = data[idx++];
            pmdSetting.minSpeed = data[idx++];
            pmdSetting.alertFlashType = pmdSetting.GetAlertFlashType(data[idx++]);
            pmdSetting.alertMessageType = pmdSetting.GetAlertMessage(data[idx++]);
            pmdSetting.dataFormat = pmdSetting.GetDataFormat(data[idx++]);
            idx += 2;

            // ======= Message String Portion Length:72 =======

            byte[] strData = new byte[9];

            // memory layout: top msg 1, bottom msg1, top msg 2, bottom msg2 ...
            for (i = 0; i < 3; i++)
            {
                // Set top msg         
                Buffer.BlockCopy(data, idx, strData, 0, 9);
                pmdSetting.topMsgs[i] = Utility.GetStringValue(strData, 6, true);
                idx += 9;

                // Set bottom msg
                Buffer.BlockCopy(data, idx, strData, 0, 9);
                pmdSetting.bottomMsgs[i] = Utility.GetStringValue(strData, 6, true);
                idx += 9;
            }
            idx += 18; // There are 4 rows of msg data on embeded side instead of 3


            // ======= Timer Value Portion Length: 140 (Each Timer is 28 bytes) =======

            for (i = 0; i < 5; i++)
            {
                pmdSetting.timers[i].functionType =
                            (TimerFunctionType)TimerSetting.GetTimerFunctionType(data[idx]);
                idx += 4;

                pmdSetting.timers[i].modeType = TimerSetting.GetTimerModeType(data[idx]);
                idx += 4;

                pmdSetting.timers[i].linkToCalendar = TimerSetting.GetCalendarControl(data[idx]);
                idx += 4;

                // Speed Limit
                pmdSetting.timers[i].speedLimit = pmdSetting.GetTimerSpeedLimit(data[idx++]);

                // Start Date
                pmdSetting.timers[i].startDate =
                            Utility.CreateDateTime(data[idx + 1], data[idx], data[idx + 2]); // month, day, year
                idx += 4; // Year is 2 bytes on embeded side

                // Stop Date
                pmdSetting.timers[i].stopDate =
                            Utility.CreateDateTime(data[idx + 1], data[idx], data[idx + 2]); // month, day, year
                idx += 4; // Year is 2 bytes on embeded side

                // Starting Time
                if (data[idx] <= 23 && data[idx + 1] <= 59 && data[idx + 2] <= 59)
                    pmdSetting.timers[i].startTime =
                            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                         data[idx], data[idx + 1], data[idx + 2]);
                else
                    pmdSetting.timers[i].startTime =
                           new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                idx += 3;

                // Selected Days
                pmdSetting.timers[i].days = data[idx++];

                // Stop Time
                if (data[idx] <= 23 && data[idx + 1] <= 59 && data[idx + 2] <= 59)
                    pmdSetting.timers[i].stopTime =
                         new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                      data[idx], data[idx + 1], data[idx + 2]);
                else
                    pmdSetting.timers[i].stopTime =
                     new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 0, 0);


                idx += 3;
            }

            // ========= Calendar Portion Length: 59 =======
            /*
             *      public struct CalendarType
                    {
                        uint8	calEnable;
                        uint8	calONOFF;
                        uint8   calReset; 
                        uint8   StartDate[3] ;
                        uint8	calData[CALDATA_SIZE];
                        uint8   DaylightStart[3]; 
                        uint8   DaylightStop[3];  
                        uint8   DaylightEnable;
                    };
          
                len = sizeof(CalendarType);
                buf[idx++] = len;
                memcpy(&buf[idx], (uint8*)&(cal.calEnable), len);
                idx += len;
            */

            int calDataLeft = data.Length - idx;

            if (calDataLeft != 59)
                return;

            byte[] calData = new byte[59];

            Buffer.BlockCopy(data, idx, calData, 0, 59);

            pmdCalenar = new CalendarData(calData);

            isValid = true;
        }

        public byte[] ToData()
        {
            byte[] data = new byte[278];
            int idx = 0;
            int i, j;

            // ======= Confiugure Portion =======
            /*
                public struct CfgValueType
                {
	                public uint8	MaxSpeed;	    // Max Speed
	                public uint8	MinSpeed;	    // Min Speed
	                public uint8	AlertFlash;		// Alert Type - Flash
	                public uint8	AlertMessage;	// Alert Type - Message
	                public uint8	DateType;	    // Date Type
	                public uint8	SpeedSource;    // Speed source
	                public uint8	BTMode;         // Bluetooth Mode
                };

                len = sizeof(CfgValueType);
                buf[idx++] = len;
                memcpy(&buf[idx], (uint8*)&(cfg.MaxSpeed), len);
                idx += len;
            */
            data[idx++] = (byte)pmdSetting.maxSpeed;
            data[idx++] = (byte)pmdSetting.minSpeed;
            data[idx++] = (byte)pmdSetting.alertFlashType;
            data[idx++] = (byte)pmdSetting.alertMessageType;
            data[idx++] = (byte)pmdSetting.dataFormat;
            data[idx++] = 0x00;     // SpeedSource from sensor
            data[idx++] = 0x02;     // PC Application Mode

            // ======= Message String Portion =======
            /*
                const uint8 MESSAGES_TOT = 4;
                const uint8 MESSAGES_LINES = 2;
             
                uint8  ConfigMessages[MESSAGES_TOT][MESSAGES_LINES][9];
            
                len = (MESSAGES_TOT * MESSAGES_LINES) * 9;
                buf[idx++] = len;
                memcpy(&buf[idx], &(ConfigMessages[0][0][0]), len);
                idx += len;
             
                public string[] topMsgs = new string[3];
                public string[] bottomMsgs = new string[3];      
            */

            char[] strData;

            // memory layout: top msg 1, bottom msg1, top msg 2, bottom msg2 ...
            for (i = 0; i < 3; i++)
            {
                // Add top msg
                strData = pmdSetting.topMsgs[i].ToCharArray();
                for (j = 0; j < strData.Length; j++)
                    data[idx + j] = (byte)strData[j];
                idx += 9;

                // Add bottom msg
                strData = pmdSetting.bottomMsgs[i].ToCharArray();
                for (j = 0; j < strData.Length; j++)
                    data[idx + j] = (byte)strData[j];
                idx += 9;
            }
            idx += 18; // There are 4 rows of msg data on embeded side instead of 3

            // ======= Timer Value Portion =======
            /*
                idx++;
                memcpy((uint8*)&(tmr[0].tmrFunction), &buf[idx], len3);
                idx += len3;
             
                public struct TimerType
                {
	                TimerFunctionValue	tmrFunction;
	                TimerModeValue		tmrMode;
             
	                uint8	tmrSetLimit;
             
	                uint8	tmrStartDay;
	                uint8	tmrStartMon;
	                uint16  tmrStartYear;
             
	                uint8	tmrStopDay;
	                uint8	tmrStopMon;
	                uint16  tmrStopYear;
             
	                uint8	tmrStartHour;
	                uint8	tmrStartMinute;
	                uint8   tmrStartSecond;
             
	                uint8   tmrSetDay;
             
	                uint8	tmrStopHour;
	                uint8	tmrStopMinute;
	                uint8   tmrStopSecond;
                } ;
             
                public TimerMode modeType;
                public int      functionType;
                public DateTime startDate;
                public DateTime startTime;
                public DateTime stopDate;
                public DateTime stopTime;
                public byte     days;
                public int      speedLimit;
                public Boolean  linkToCalendar;
             
                Total 24 bytes for each timer, 5 timers
             
                Note: LinkToCalendar is not in yet. It should be the extra byte for modeType
                      However, the embeded side code looks like doesn't handle this
            
            */

            for (i = 0; i < 5; i++)
            {
                data[idx] = (byte)pmdSetting.timers[i].functionType;
                idx += 4;

                data[idx] = (byte)pmdSetting.timers[i].modeType;
                idx += 4;

                data[idx] = (byte)pmdSetting.timers[i].linkToCalendar;
                idx += 4;

                // Speed Limit
                data[idx++] = (byte)pmdSetting.timers[i].speedLimit;

                // Start Date
                data[idx++] = (byte)pmdSetting.timers[i].startDate.Day;
                data[idx++] = (byte)pmdSetting.timers[i].startDate.Month;
                data[idx++] = (byte)(pmdSetting.timers[i].startDate.Year % 1000);
                idx++; // Year is 2 bytes on embeded side

                // Stop Date
                data[idx++] = (byte)pmdSetting.timers[i].stopDate.Day;
                data[idx++] = (byte)pmdSetting.timers[i].stopDate.Month;
                data[idx++] = (byte)(pmdSetting.timers[i].stopDate.Year % 1000);
                idx++; // Year is 2 bytes on embeded side

                // Start Time
                data[idx++] = (byte)pmdSetting.timers[i].startTime.Hour;
                data[idx++] = (byte)pmdSetting.timers[i].startTime.Minute;
                data[idx++] = 0x00;

                // Selected Days
                data[idx++] = (byte)pmdSetting.timers[i].days;

                // Stop Time
                data[idx++] = (byte)pmdSetting.timers[i].stopTime.Hour;
                data[idx++] = (byte)pmdSetting.timers[i].stopTime.Minute;
                data[idx++] = 0x00;
            }

            // ========= Calendar Portion =======
            /*
             *      public struct CalendarType
                    {
                        uint8	calEnable;
                        uint8	calONOFF;
                        uint8   calReset; 
                        uint8   StartDate[3] ;
                        uint8	calData[CALDATA_SIZE];
                        uint8   DaylightStart[3]; 
                        uint8   DaylightStop[3];  
                        uint8   DaylightEnable;
                    };
          
                len = sizeof(CalendarType);
                buf[idx++] = len;
                memcpy(&buf[idx], (uint8*)&(cal.calEnable), len);
                idx += len;
            */
            byte[] calData = pmdCalenar.ToData();

            for (i = 0; i < calData.Length; i++)
            {
                data[idx++] = calData[i];
            }

            // HERE idx should be equal 278
            if (idx != 278)
            {
                Console.WriteLine("ToData Encoded Error");
                return null;
            }

            return data;
        }

        static public Boolean UpdatePMDClock(string imsi, DateTime clockTime)
        {
            long ticks = clockTime.Ticks;

            byte[] clockData = BitConverter.GetBytes(ticks);

            if (udpClient == null)
            {
                udpClient = new UdpClient(11000);
                udpClient.Connect(cloudServerIP, 37000);
            }

            int len = 4 + 2 + imsi.Length + clockData.Length;

            byte[] data = new byte[5 + len];

            data[0] = 0xB2;

            data[1] = (byte)(len & 0xff);
            data[2] = (byte)((len >> 8) & 0xff);
            data[3] = 0x00;
            data[4] = 0x00;

            // Payload
            data[5] = (byte)(webServerId & 0xff);
            data[6] = (byte)((webServerId >> 8) & 0xff);
            data[7] = (byte)((webServerId >> 16) & 0xff);
            data[8] = (byte)((webServerId >> 24) & 0xff);

            data[9] = (byte)TableID.PMG;
            data[10] = (byte)DatabaseOperationType.ClockUpdate;

            byte[] keyArray = Encoding.ASCII.GetBytes(imsi);

            Buffer.BlockCopy(keyArray, 0, data, 11, keyArray.Length);
            Buffer.BlockCopy(clockData, 0, data, 11 + keyArray.Length, clockData.Length);

            int byteSent = udpClient.Send(data, data.Length);

            if (byteSent == data.Length)
                return true;
            else
                return false;

        }

        static public Boolean SendNotificationToCloudServer(TableID tableId, DatabaseOperationType type,
                                                            string imsi)
        {
            if (udpClient == null)
            {
                udpClient = new UdpClient(12000);
                udpClient.Connect(cloudServerIP, 37002);
            }

            //if (udpClient == null)
            //{
            //    udpClient = new UdpClient(11000);
            //    udpClient.Connect(cloudServerIP, 37000);
            //}


            int len = 4 + 2 + imsi.Length; // 4 is PMD ID

            byte[] data = new byte[5 + len];

            data[0] = 0xB2;

            data[1] = (byte)(len & 0xff);
            data[2] = (byte)((len >> 8) & 0xff);
            data[3] = 0x00;
            data[4] = 0x00;

            data[5] = (byte)(webServerId & 0xff);
            data[6] = (byte)((webServerId >> 8) & 0xff);
            data[7] = (byte)((webServerId >> 16) & 0xff);
            data[8] = (byte)((webServerId >> 24) & 0xff);

            data[9] = (byte)tableId;
            data[10] = (byte)type;

            byte[] keyArray = Encoding.ASCII.GetBytes(imsi);

            Buffer.BlockCopy(keyArray, 0, data, 11, keyArray.Length);

            int byteSent = udpClient.Send(data, data.Length);

            if (byteSent == data.Length)
                return true;
            else
                return false;
        }
    }

}
