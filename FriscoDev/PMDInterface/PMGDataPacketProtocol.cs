

//using System;

//using System.Collections.Generic;

//using System.Linq;

//using System.Text;

//using System.Net;

//using System.Text.RegularExpressions;

//using System.IO;



//using uint8_t = System.Byte;

//using uint32_t = System.UInt32;

//using int16_t = System.Int16;

//using uint16_t = System.UInt16;

//using int8_t = System.Byte;

//using FriscoTab;





//namespace PMGDataPacketProtocol

//{

//    public class GlobalConstantDefintion

//    {

//        public const uint32_t PARAM_MAP_SIGNATURE = 0x4A8C1B29;



//        static public DateTime NullTimeStamp = new DateTime(2000, 1, 1);



//        //public const uint32_t PARAM_MAP_VERSION_MAJOR = 0xFF00000F;

//        //public const uint8_t  PARAM_MAP_VERSION_INCR = 0X00000007;



//        //

//        // Major Firmware Version which has effect on PCA side

//        //

//        public const uint32_t FIRMWARE_SUPPORT_NEW_POWER_SPECS = 0x01050304;    // v1.5.3.4

//        public const uint32_t FIRMWARE_SUPPORT_NO_RESET = 0x01040401;           // v1.4.4.1 

//        public const uint32_t FIRMWARE_SUPPORT_USB_FILEDOWNLOAD = 0x01030320;   // v1.3.3.32

//        public const uint32_t FIRMWARE_SUPPORT_COMPOSITE_PAGE = 0x01050600;     // V1.5.6.0



//        public const uint32_t MAX_BATTERY_CAPACITY = 5000000; // 5 millian mAH



//        //

//        // This version of PCA will only support 0xFF000011 and above

//        // due to parameter size change from 300 bytes to 1404

//        //

//        public const uint32_t PARAM_MAP_VERSION_MAJOR_GPIO = 0xFF000013;

//        public const uint32_t PARAM_MAP_VERSION_MAJOR_BETA_4 = 0xFF00000F;



//        public const int PARAM_MAP_SIZE_LARGE = 1404;

//        public const int PARAM_MAP_SIZE_BETA_4 = 300;



//        public const long UNDEFINED_CLOCK_TICK = -10000;



//        public const byte PMG3GSubsystem = 0x03;

//        public const byte PMGServerSubsystem = 0x66;



//        public const string DEFAULT_PMG_PARAM_MAP =

//          "29 1B 8C 4A 13 00 00 FF 07 00 00 00 00 00 01 00 00 00 FF FF FF FF FF FF 02 00 5A 00 0A 14 " +

//            "01 00 0B 00 CE FF 00 00 00 00 00 00 00 00 01 00 00 00 C0 0F 00 00 00 00 00 00 1E 28 00 00 " +

//            "01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 01 01 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 01 01 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 41 28 41 28 D0 07 00 00 3C 00 00 00 " +

//            "01 01 02 00 63 46 00 32 96 00 00 00 D0 07 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 40 42 0F 00 41 01 4E 02 F6 28 DC 3E 10 27 00 00 00 23 04 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 D0 07 00 00 32 00 2C 01 02 01 DC 05 D0 07 " +

//            "00 01 0A 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 04 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 05 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 02 00 00 00 00 00 06 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 02 00 00 00 02 00 00 00 02 00 00 00 02 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 02 73 74 61 6C 6B 65 72 5F 50 4D " +

//            "47 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 " +

//            "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 7D 71 ";

//    }

//    public enum PMGErrorCode

//    {

//        Success = 0,

//        Wrong_CRC = 1,

//        Wrong_Message_Format = 2,



//        File_No_Found = 0x11,

//        Failed_To_SendMessage = 0x13,

//        Animation_Read_Not_Yet_Supported = 0x15,

//        Animation_Exceeds_Max_Num_Cells = 0x18,

//        Invalid_Payload_Size = 0x19,

//        Animation_Num_Cells_NE_Num_Packets = 0x1A,

//        Packet_Already_Written = 0x1B,

//        Signature_Mismatch = 0x23,

//        Major_Version_Mismatch = 0x34,

//        Incremental_Version_Mismatch = 0x35,

//        CRC_Mismatch = 0x55,

//        Screen_Size_Mismatch = 0x89,

//        Type_Mismatch = 0x94,

//        Invalid_Filename = 0x95,

//        Invalid_Filename_Length = 0x96,

//        Filename_Size_Mismatch = 0x97,

//        Invalid_Filename_Extension = 0x98,

//        Page_Already_Exists = 0x99,

//        Out_Of_Page_Storage_Space = 0x9D,

//        Page_Not_Open = 0xA5,

//        Internal_Invalid_Page_Address = 0xF0,

//        Internal_Failed_SPI_Mutex = 0xF1,

//        Internal_Unknown_Failure = 0xF2

//    };



//    public enum ModuleTypeCode

//    {

//        RadarProcessor = 9999,

//        TwelveInchDisplay = 5499,

//        MTTP = 5501,

//        Cellular = 5503,

//        Bluetooth = 5504,

//        Wifi = 5505,

//        Wireless433 = 5509,

//        StormWarning = 5510,

//        EmergencyAlert = 5511,

//        GPS = 5512,

//        Ethernet = 5513,

//        MeshRadio = 5515,

//        GPIO = 5516,

//        SmallLionBattery = 5531,

//        FiberOptic = 5532,

//        BatteryBackup = 5534,

//        SimulatedFlash = 5539,

//        FifteenInchDisplay = 5540,

//        EighteenInchDisplay = 5541,

//        LargeLionBattery = 5572,  // So called Internal Battery Charger by Mike

//        SlotControlPCB = 5571,

//        TrafficCamera = 5666,

//        Controller12_Inch = 5500,

//        Controller15_Inch = 5542,

//        Controller18_Inch = 5543,

//        InternalCharger = 5621,

//        LTE4G = 5624,

//        PAN_Radio = 9998,

//        TextAndGraphic = 126

//    };



//    public enum StatsCommandType

//    {

//        SendStats = 0x01,

//        StopStats = 0x02,

//        StatsReceived = 0x03,

//        StatsStatus = 0x04,

//        DeleteStats = 0x05,

//        GenereateTestStats = 0x99

//    };



//    public enum StatsRecordType

//    {

//        HeaderRecord = 0x00,

//        NotesRecord = 0x01,

//        StatsRecord = 0x02,

//        Unknown = 0x03

//    };



//    public enum MPPTDataType

//    {

//        InstantaneousData = 0x00,

//        TwentyFourHourMax = 0x01,

//    }



//    public enum MPPTSubCommandId

//    {

//        RepeatedSend = 0x00,

//        SendOnce = 0x01,

//        StopSending = 0x02

//    }



//    public enum BatterLocation

//    {

//        ExternalBattery = 0,

//        InternalBattery = 1

//    }

//    public enum FirmwareUpdateStatusCode

//    {

//        Success = 0x01,

//        UploadTransferFailed = 0x23,

//        MD5Mismatch = 0x34,

//        ModuleNotPresent = 0x41,

//        HeaderMismatch = 0x59,

//        InvalidModule = 0x61,

//        SequenceError = 0x73,

//        UnknownFailure = 0x99

//    }



//    public enum uSDFileUpdateCommandType

//    {

//        UploadNewFile = 0x00,

//        WriteDataToFile,

//        CompleteFileTransfer,

//        AbortFileTransfer

//    }

//    public enum PMGCommandID

//    {

//        Unknown = 0x00,



//        ConnectAgent = 0x0F,



//        Registration = 0x10,



//        DeviceSystemInfo_Deprecated = 0x11,



//        DeviceSystemInfo = 0x13,



//        SerialNumber = 0x14,



//        ReadWritePageIDList = 0x1F,



//        WriteParameter = 0x12,

//        KeepAlive = 0x16,

//        Status = 0x18,



//        WriteDateTime = 0x1C,



//        ReadPageRequest = 0x26,



//        WriteFeatureEnableKey = 0x17,



//        EthernetStatus = 0x1A,



//        uSDFileUpdate = 0x1D,

//        WiFiStatus = 0x1E,



//        // New Commands

//        ReadWriteCalendar = 0x30,



//        ReadWriteTextPage = 0x32,

//        ReadWriteGrahicPage = 0x33,

//        ReadWriteAnimationPage = 0x34,



//        ReadWriteCompositePage = 0x37,



//        ReadWriteScheduledOperationDeprecated = 0x29,



//        ReadWriteScheduledOperation = 0x2A,  // New command with Calendar



//        USBLoggingEnable = 0x55,

//        StatsRecordCommand = 0xBC,

//        Ack = 0x98,



//        Deprecated_ReadWriteTextPage = 0x22,

//        Deprecated_ReadWriteGrahicPage = 0x23,

//        Deprecated_ReadWriteAnimationPage = 0x24,

//        Deprecated_ReadWriteSchledOperation = 0x28,



//        DeletePages = 0x25,



//        RequestReset = 0x92,

//        GeneralAck = 0x99,  // Deprecated



//        AddRemote = 0xA4,

//        PurgeRemote = 0xA5,



//        PowerDataRequest = 0xCC,

//        PowerMonitor = 0xCD,

//        PowerSpecsDeprecated = 0xCE,

//        PowerSpecs = 0xCF,



//        FirmwareUpdateUpload = 0xBE,

//        FirmwareUpdateInstall = 0xBF,

//        FirmwareUpdateResult = 0xC0,



//        FactoryReset = 0xFF,



//        PMGStartUpNoification = 0xDD,



//        PMGSetUSBLogging = 0x55,





//        // Debug portion

//        RegisterServerLogTrace = 0x81,

//        UnRegisterServerLogTrace = 0x82,

//        ServerLogTrace = 0x83,

//        ProcessHeartBeat = 0x84,

//        ProcessHeartBeatAck = 0x85,

//        Notification = 0x86

//    };



//    public enum SubSystem_ID_t

//    {

//        SubSys_Controller = 0,

//        SubSys_3G_Module = 3,

//        SubSys_Bluetooth_Module = 4,

//        SubSys_WIFI_Module = 5,

//        SubSys_433_Module = 9,

//        SubSys_Storm_Module = 10,

//        SubSys_EAS_Module = 11,

//        SubSys_GPS_Module = 12,

//        SubSys_Ethernet_Module = 13,

//        SubSys_Mesh_Radio_Module = 15,

//        SubSys_GPIO_Module = 16,

//        SubSys_Battery_Backup_Module = 35,

//        SubSys_Left_LED_Panel = 99,

//        SubSys_Right_LED_Panel = 100,

//        SubSys_Frisco_COMM_Bridge = 101,



//        SubSys_Application = 102,  // Application side



//        SubSys_Broadcast = 127

//    };



//    public enum StatsOffloadOption

//    {

//        OnDemand = 0,

//        Streaming = 1

//    };



//    public enum StatsOnDemandPort

//    {

//        AC_Commands = 0,

//        USB_MSC = 1,

//    };



//    public enum WifiMode

//    {

//        Off = 0,

//        Station,

//        AccessPoint

//    };



//    public enum WifiSecurityType

//    {

//        Open = 0,

//        WPA,

//        WPA2

//    };



//    public enum Action_Type_Enum_t

//    {

//        None = 0,

//        Speed,

//        Text,

//        Graphic,

//        Animation,

//        Time,

//        Temp,

//        Composite

//    };



//    public enum Alert_Type_Enum_t

//    {

//        None = 0,

//        Blink_Display,

//        Strobes,

//        Blink_and_Strobes,

//        Camera_Trigger,

//        GPIO_Out_1,

//        GPIO_Out_2,

//        GPIO_Out_3,

//        GPIO_Out_4

//    };



//    public enum GPIOs_Present_Bits_t

//    {

//        gpio_present_In_1 = 0x01,

//        gpio_present_In_2 = 0x02,

//        gpio_present_In_3 = 0x04,

//        gpio_present_In_4 = 0x08,

//        gpio_present_Out_Pos = 0x10,

//        gpio_present_Out_1 = 0x10,

//        gpio_present_Out_2 = 0x20,

//        gpio_present_Out_3 = 0x40,

//        gpio_present_Out_4 = 0x80,

//        gpio_present_In_Mask = 0x0F,

//        gpio_present_Out_Mask = 0xF0

//    };



//    public enum GPIO_FLAG

//    {

//        Enabled = 1,

//        Disabled = 0,

//        ActiveStateClosed = 1,

//        ActiveStateOpened = 0,

//        InputFollow = 1,

//        InputPulse = 0

//    };



//    public class WiFiStatus

//    {

//        public byte[] macAddress = new byte[6];

//        public uint32_t ipAddress;

//        public uint32_t netMask;

//        public uint32_t gateway;

//        public byte numClient = 0;



//        public WiFiStatus() { }

//    };



//    public class EthernetStatus

//    {

//        public uint32_t ipAddress;

//        public uint32_t netMask;

//        public uint32_t gateway;

//        public byte connectionStatus = 0;

//        public byte linkStatus = 0;

//        public EthernetStatus() { }

//    };





//    /* **********************************************

//     * GPIO Flag Definitions:

//     * 		Bit 0:	Enable 			1-Enable, 0-Disable

//     * 		Bit 1:	Active State	1-Closed, 0-Open

//     * 		Bit 2:	Follow Input	1-Follow, 0-Pulse	(Not used--set to 0)

//     */

//    public class GPIO_Input_t

//    {

//        // Type			// Name				    // Offset		Size	// Description

//        //------------------------------------------------------------------------------------------------------------------------------------------------

//        //public uint8_t Flags;                   // 0x00000000	1		See Above

//        //public uint8_t Display;                 // 0x00000001	1		Display type (cast to Display_Type_Enum_t)

//        //public uint8_t Action;                  // 0x00000002	1		Alert type (cast to Alert_Type_Enum_t)

//        //public uint8_t Reserved;                // 0x00000003	1

//        //public byte[]  Filename = new byte[32]; // 0x00000004  32

//        //                                        //  		   36



//        // Type		// Name				            // Offset		Size	// Description

//        //------------------------------------------------------------------------------------------------------------------------------------------------

//        public action_t Action = new action_t();    // 0x00000000	36

//        public uint8_t Flags;                       // 0x00000024	1		See Above

//        public uint8_t Reserved;	                // 0x00000025	1

//        public uint16_t Duration = 1;               // 0x00000026	2       (1 second up to 15 minutes)

//                                                    //              40

//        public GPIO_Input_t()

//        {

//        }

//        public bool decode(byte[] bytes, ref int byteIdx)

//        {

//            Action.decode(bytes, ref byteIdx);

//            Flags = Util.GetUint8(bytes, ref byteIdx);

//            Reserved = Util.GetUint8(bytes, ref byteIdx);

//            Duration = Util.GetUint16(bytes, ref byteIdx);



//            // Valid range is 1 ~ 900 (1 seconds to 15 minutes)  

//            //if (Duration == 0)

//            //    Duration = 1;



//            // Valid range is 0 ~ 900 (1 seconds to 15 minutes)  

//            if (Duration > 900)

//                Duration = 1;



//            return false;

//        }



//        public void encode(ref byte[] recvData, ref int byteIdx)

//        {

//            Action.encode(ref recvData, ref byteIdx);

//            Util.SetUint8(Flags, ref recvData, ref byteIdx);

//            Util.SetUint8(Reserved, ref recvData, ref byteIdx);

//            Util.SetUint16(Duration, ref recvData, ref byteIdx);

//        }



//        public void reset()

//        {

//            Flags = 0x00;

//            Duration = 1;

//            Action.reset();

//        }



//        public string toString()

//        {

//            string s = string.Empty;



//            s += ("[Flags:" + Utils.ToBitsString(Flags) + "] ");

//            s += ("[Duration:" + Duration + "] ");

//            s += ("Action " + Action.toString());



//            return s;

//        }

//    }



//    public class GPIO_Output_t

//    {



//        // Type			// Name				    // Offset		Size	// Description

//        //------------------------------------------------------------------------------------------------------------------------------------------------

//        public uint8_t Flags;                  // 0x00000000	 1		See above flag definition

//        public uint8_t Reserved;               // 0x00000001    1

//        public uint16_t Duration = 1;           // 0x00000002    2		Unit: second (1 second up to 15 minutes)  

//                                                //               4

//        public GPIO_Output_t()

//        {

//        }



//        public bool decode(byte[] bytes, ref int byteIdx)

//        {

//            Flags = Util.GetUint8(bytes, ref byteIdx);

//            Reserved = Util.GetUint8(bytes, ref byteIdx);

//            Duration = Util.GetUint16(bytes, ref byteIdx);



//            // Valid range is 1 ~ 900 (1 seconds to 15 minutes)

//            //if (Duration == 0)

//            //    Duration = 1;



//            // Valid range is 0 ~ 900 (1 seconds to 15 minutes)  

//            if (Duration > 900)

//                Duration = 1;



//            return false;

//        }



//        public void encode(ref byte[] recvData, ref int byteIdx)

//        {

//            Util.SetUint8(Flags, ref recvData, ref byteIdx);

//            Util.SetUint8(Reserved, ref recvData, ref byteIdx);

//            Util.SetUint16(Duration, ref recvData, ref byteIdx);

//        }



//        public void reset()

//        {

//            Flags = 0x00;

//            Duration = 1;

//        }



//        public string toString()

//        {

//            string s = string.Empty;



//            s += ("[Flags:" + Utils.ToBitsString(Flags) + "] ");

//            s += ("[Duration:" + Duration + "] ");



//            return s;

//        }



//    }



//    public enum Speed_Unit_Enum_t

//    {

//        MPH = 0,

//        KPH,

//        knots

//    };

//    public enum Temp_Unit_Enum_t

//    {

//        Tempunit_Celsius = 0,

//        Tempunit_Fahrenheit,

//        Tempunit_Kelvin,

//        Tempunit_Rankine

//    };

//    public enum Language_Enum_t

//    {

//        Lan_English,

//        Lan_Spanish,

//        Lan_German,

//        Lan_French

//    };

//    public enum Power_Mode_Enum_t

//    {

//        Normal,

//        LowPower

//    };



//    public enum IPType

//    {

//        DHCP = 0,

//        Static = 1

//    };



//    public enum RadarEnable

//    {

//        Disabled = 0,

//        Internal = 1,

//        PANExternal = 2,

//        RS232External = 3

//    };



//    public enum OperationDirection

//    {

//        Both = 0,

//        Closing = 1,

//        Away = 2,

//    };



//    public enum SpeedUnit

//    {

//        MPH = 0,

//        kmh = 1,

//        knots = 2,

//        metersPerSec = 3,

//        feetPerSec = 4

//    };



//    public enum SpeedResolution

//    {

//        Ones = 0,

//        Tenths = 1

//    };



//    public enum ExternalRadarSpeed

//    {

//        LastAndLive = 0,

//        Peak = 1,

//        HitAndSpin = 2,

//    };



//    public enum NotificationType

//    {

//        Insert = 0,

//        Update,

//        Delete,

//        ResetPMG

//    };



//    public enum TableID

//    {

//        PMG = 0,

//        Account,

//        Connection,

//        Message

//    };



//    public enum ParamaterId

//    {

//        Unknown = 0,



//        IdleDisplay = 1,

//        IdleDisplayPage,



//        SpeedLimit,

//        SpeedLimitDisplay,

//        SpeedLimitDisplayPage,

//        SpeedLimitAlertAction,



//        AlertLimit,

//        AlertLimitDisplay,

//        AlertLimitDisplayPage,

//        AlertLimitAlertAction,



//        MinLimit,

//        MaxLimit,

//        SpeedUnit,

//        TemperatureUnit,

//        Brightness,

//        EnableMUTCDCompliance,



//        Radar,

//        RadarHoldoverTime,

//        RadarCosine,

//        RadarUnitResolution,

//        RadarSensitivity,

//        RadarTargetStrength,

//        RadarTargetAcceptance,

//        RadarTargetHoldOn,

//        RadarOperationDirection,

//        RadarExternalRadarSpeed,

//        RadarExternalEchoPanRadarData,



//        TrafficEnableRecording,

//        TrafficTargetStrength,

//        TrafficMinimumTrackingDistance,

//        TrafficMinimumFollowingTime,

//        TrafficDataOnDemand,



//        WirelessPIN,

//        EthernetIPSetting,

//        EthernetIPAddress,

//        EthernetSubnetMask,

//        EthernetDefaultGateway,



//        WifiMode,

//        WifiAccessPointSecurity,

//        WifiAccessPointPassword,

//        WifiStationSecurity,

//        WifiStationPassword,

//        WifiStationSSID,

//        WifiStationIPType,

//        WifiStationIPAddress,

//        WifiStationSubnetMask,

//        WifiStationDefaultGateway

//    };



//    public class action_t

//    {

//        // Type			// Name			             // Offset		Size	// Description

//        //------------------------------------------------------------------------------------------------------------------------------------------------

//        public uint8_t primary;                      // 0x00000000	1		Action type (cast to Action_Type_Enum_t)

//        public uint8_t alert;                        // 0x00000001	1		Alert type (cast to Alert_Type_Enum_t)

//        public uint8_t cache_slot;                   // 0x00000002	1		EEPROM value ignored--Shadow copy will be set at runtime

//        public uint8_t reserved;				     // 0x00000003	1		

//        public byte[] Filename = new byte[32];      // 0x00000004  32

//                                                    // 		    36

//        public bool decode(byte[] bytes, ref int byteIdx)

//        {

//            int i, j;



//            primary = Util.GetUint8(bytes, ref byteIdx); // --> Display

//            alert = Util.GetUint8(bytes, ref byteIdx);   // --> Action (Alert Type) in ParameterMap.h

//            cache_slot = Util.GetUint8(bytes, ref byteIdx);

//            reserved = Util.GetUint8(bytes, ref byteIdx);



//            Filename = Util.GetByteArray(bytes, ref byteIdx, 32);



//            //

//            // Now, only keep 3 characters appendix after "."

//            // For unknown reason, map data received from PMG

//            // sometime will have garbage data after appendix 

//            //

//            for (i = 0; i < 32; i++)

//            {

//                if (Filename[i] == '.')

//                {

//                    for (j = i + 4; j < 32; j++)

//                    {

//                        Filename[j] = 0;

//                    }

//                    break;

//                }

//            }





//            // Somehow, the PMG will send out garbage page filename

//            // even the display type is not text, graphic or animation. So,

//            // here we reset to 0 if necessary

//            Action_Type_Enum_t actionType = (Action_Type_Enum_t)primary;



//            if (!PageTag.isPageFileRequired(actionType))

//                setFilename(null);



//            return true;

//        }



//        public void encode(ref byte[] recvData, ref int byteIdx)

//        {

//            Util.SetUint8(primary, ref recvData, ref byteIdx);

//            Util.SetUint8(alert, ref recvData, ref byteIdx);

//            Util.SetUint8(cache_slot, ref recvData, ref byteIdx);

//            Util.SetUint8(reserved, ref recvData, ref byteIdx);

//            Util.SetByteArray(Filename, ref recvData, ref byteIdx);

//        }



//        public void setFilename(string filename)

//        {

//            for (int i = 0; i < Filename.Length; i++)

//                Filename[i] = 0x00;



//            if (filename != null && filename.Length > 0)

//            {

//                byte[] bytes = Encoding.ASCII.GetBytes(filename);



//                for (int i = 0; i < bytes.Length; i++)

//                {

//                    Filename[i] = (byte)bytes[i];

//                }

//            }

//        }



//        public string getFilename()

//        {

//            string s = System.Text.Encoding.Default.GetString(Filename);



//            s = s.Replace("\0", string.Empty);

//            s = s.Trim();



//            if (s.IndexOfAny(Path.GetInvalidFileNameChars()) < 0)

//                return s;

//            else

//                return string.Empty;

//        }



//        public void reset()

//        {

//            primary = 0;

//            alert = 0;

//            setFilename(null);

//        }



//        public string toString()

//        {

//            string s = string.Empty;



//            string name = System.Text.Encoding.Default.GetString(Filename);



//            name = name.Replace("\0", string.Empty);

//            name = name.Trim();



//            s += ("[action:" + ((Action_Type_Enum_t)primary).ToString() + "] ");

//            s += ("[alert:" + ((Alert_Type_Enum_t)alert).ToString() + "] ");



//            if (name.Length > 0)

//                s += ("[page:" + name + "]");

//            else

//                s += ("[page: Empty]");



//            return s;

//        }

//    };



//    public class Action_Overlay_t

//    {

//        public uint8_t Speed_Limit = 65;               // 0x00000000  1		// The posted speed limit.

//        public uint8_t Alert_Speed = 90;               // 0x00000001  1		// Excessive speed above the speed limit.

//        public uint8_t[] reserved = new uint8_t[2];    // 0x00000002  2

//        public action_t Idle_Action = new action_t();  // 0x00000004  3  	// Action to take when speed limit is exceeded

//        public action_t Limit_Action = new action_t(); // 0x00000028  36	// Action to take when Alert speed is exceeded

//        public action_t Alert_Action = new action_t(); // 0x0000004C  36	// Action to take when idle (e.g. when detecting speeds from 0 (e.g. no target) up to and including the speed limit.

//                                                       // 			  112

//        public bool decode(byte[] bytes, ref int byteIdx)

//        {

//            Speed_Limit = Util.GetUint8(bytes, ref byteIdx);

//            Alert_Speed = Util.GetUint8(bytes, ref byteIdx);



//            reserved = Util.GetByteArray(bytes, ref byteIdx, 2);



//            Idle_Action.decode(bytes, ref byteIdx);

//            Limit_Action.decode(bytes, ref byteIdx);

//            Alert_Action.decode(bytes, ref byteIdx);



//            return true;

//        }



//        public void encode(ref byte[] recvData, ref int byteIdx)

//        {

//            Util.SetUint8(Speed_Limit, ref recvData, ref byteIdx);

//            Util.SetUint8(Alert_Speed, ref recvData, ref byteIdx);



//            Util.SetByteArray(reserved, ref recvData, ref byteIdx);



//            Idle_Action.alert = 0;

//            Idle_Action.cache_slot = 0;

//            Idle_Action.reserved = 0;



//            Idle_Action.encode(ref recvData, ref byteIdx);

//            Limit_Action.encode(ref recvData, ref byteIdx);

//            Alert_Action.encode(ref recvData, ref byteIdx);

//        }

//    }



//    #region Minor Version 1 (EEPROM_Param_Data_t)

//    //    typedef struct

//    //{

//    // // Type						// Name							// Offset		Size	// Description

//    // //------------------------------------------------------------------------------------------------------------------------------------------------

//    //    uint32_t sig;                           // 0x00000000	4		// Magic number to ensure valid data is loaded

//    //    uint32_t version_major;                 // 0x00000004	4		// Incompatible with earlier versions

//    //    uint32_t version_incremental;           // 0x00000008	4		// Incremental additions to older versions

//    //    uint8_t Stats_Offload_Option;           // 0x0000000C	1		// Selection for data on demand or streaming. 0 - on demand, 1 - streaming

//    //    uint8_t Stats_On_Demand_Port;           // 0x0000000D	1		// Selection of the on demand port. 0 - AC commands, 1 - USB MSC

//    //    uint8_t Temp_Unit;                      // 0x0000000E	1		// Unit for measuring temperature (tempunit_Fahrenheit, tempunit_Celsius, tempunit_Kelvin, tempunit_Rankine) (Temp_Unit_Enum_t)

//    //    uint8_t Speed_Resolution;               // 0x0000000F	1		// Resolution of speed units.  0 - ones, 1 - tenths

//    //    uint8_t Speed_Measurement_Unit;         // 0x00000010	1		// Unit for measuring speed (spdunit_MPH, spdunit_KPH or spdunit_knots, spdunit_MPS, spdunit_FPS) (Speed_Unit_Enum_t)

//    //    uint8_t Resources_First_Write;          // 0x00000011	1		// Resources need initialization

//    //    int16_t Speed_Limit_Hysteresis;         // 0x00000012	2		// Speed to drop before Speed Limit threshold action can be reactivated.  Set to 0xFFFF to disable hysteresis.

//    //    int16_t Max_Speed_Hysteresis;           // 0x00000014	2		// Speed to drop before Max Speed threshold action can be reactivated.  Set to 0xFFFF to disable hysteresis.

//    //    int16_t Min_Speed_Hysteresis;           // 0x00000016	2		// Speed to rise before Min Speed threshold action can be reactivated.  Set to 0xFFFF to disable hysteresis.

//    //    int16_t Alert_Speed_Hysteresis;         // 0x00000018	2		// Speed to drop before Alert speed threshold action can be reactivated.  Set to 0xFFFF to disable hysteresis.

//    //    uint16_t Max_Speed_Threshold;           // 0x0000001A	2		// Max speed displayed.

//    //    uint8_t Min_Speed_Threshold;            // 0x0000001C	1		// Min speed displayed.

//    //    uint8_t Speed_Alert_Duration;           // 0x0000001D	1		// Number of 50 ms frames during which to continue displaying the speed alert (red/blue flashers).

//    //    uint8_t Stats_Enable;                   // 0x0000001E	1		// Enable/Disable stats collection

//    //    uint8_t Enable_USB_Logging;             // 0x0000001F	1		// Enable Log output to USB Serial I/O port

//    //    uint8_t Radar_Sensitivity;              // 0x00000020	1		// Radar Sensitivity 0-16, with 16 = most sensitive.

//    //    uint8_t reserved_1[19];                 // 0x00000021  19		//

//    //    uint32_t Stats_Offload_Time;            // 0x00000034	4		// Time stamp of last stats offload(EPOCH)

//    //    Action_Overlay_t Std_Ops;               // 0x00000038 112

//    //    uint32_t Alert_Sequence_Order;          // 0x000000A8	4		// Each nibble represents one frame step in the sequence starting from LSB and Top Left (see Display_Update_Flashers() for more)

//    //    uint32_t Idle_Timeout;                  // 0x000000AC	4		// Time below min speed before moving to idle state if not displaying anything.

//    //    uint32_t Menu_Timeout_s;                    // 0x000000B0	4		// Automatically exit the menu if this amount of time elapses between key-presses while in menu (seconds).

//    //    uint8_t Radar_Enable;                   // 0x000000B4	1		// 0 - Disable, 1 - Enable Internal Radar, 2 - Enable External Radar

//    //    uint8_t Radar_Operating_Direction;      // 0x000000B5	1		// 0 - Both, 1 - Closing, 2 - Away

//    //    uint8_t Radar_Holdover_Time;            // 0x000000B6	1		// Time target is held after signal is lost.  0 - 11 seconds

//    //    uint8_t Radar_Cosine;                   // 0x000000B7	1		// Angle between radar antenna normal and centroid of detection region.  0-70 degrees

//    //    uint8_t Radar_Target_Strength;          // 0x000000B8	1		// AKA Visual Target Strength Sensitivity (1-99, 99 - most sensitive).  Discriminates targets based on absolute signal strength independent of noise floor.

//    //    uint8_t Radar_Target_Acceptance;        // 0x000000B9	1		// AKA Target Acquisition Density (0-100).  Percentage of time target is seen over the course of the Target Acquisition Span time period.

//    //    uint8_t Radar_Target_Hold_On;           // 0x000000BA	1		// AKA Target Loss Density (0-100).  Percentage of time target is seen below which, target is will be considered lost.

//    //    uint8_t Stats_Target_Strength;          // 0x000000BB	1		// AKA Stats Target Strength Sensitivity (1-99, 99 - most sensitive).  Discriminates targets based on absolute signal strength independent of noise floor.

//    //    uint16_t Stats_Min_Tracking_Distance;   // 0x000000BC	2		// Minimum distance between targets to be counted in stats (same units as speed measurement units)

//    //    uint8_t Stats_Min_Following_Time;       // 0x000000BE	1		// Minimum following time between targets to be counted in stats in seconds (0-10)

//    //    uint8_t reserved_2[21];                 // 0x000000BF  21 		//

//    //    uint32_t High_clux;                     // 0x000000D4	4		// Highest possible _measured_ brightness in hundredths of lux

//    //    uint8_t High_Brightness;                // 0x000000D8	1		// Best brightness in conditions in which High_clux was measured (0-100 percent brightness)

//    //    uint8_t Min_Brightness;                 // 0x000000D9	1		// Desired brightness at Min_clux (0-100 percent brightness)

//    //    uint8_t Max_Brightness;                 // 0x000000DA	1		// Maximum brightness output--brightness rails out here regardless of higher clux measurements. (0-100 percent brightness)

//    //    uint8_t Lux_Low_Pass_Divider;           // 0x000000DB	1		// Divide error value by this to integrate delta over time

//    //    float Gamma;                            // 0x000000DC	4		// Brightness gamma exponent (0.0 < Gamma < 3.0)

//    //    uint32_t Lux_Measurement_Period_ms;     // 0x000000E0	4		// Ambient light sensor sampling period (milliseconds)

//    //    uint8_t Manual_Brightness_Level;        // 0x000000E4	1		// Override automatic brightness with this level.  Set to 0 for automatic brightness.

//    //    uint8_t Demo_Brightness_Day;            // 0x000000E5	1		// Demo daytime brightness level

//    //    uint8_t Demo_Brightness_Night;          // 0x000000E6	1		// Demo nighttime brightness level

//    //    uint8_t Language;                       // 0x000000E7	1		// Language (Cast to Language_Enum_t)

//    //    uint8_t Power_Mode;                     // 0x000000E8	1		// Power Mode (Cast to Power_Mode_Enum_t)

//    //    uint8_t Time_Format_24_Hour;            // 0x000000E9	1		// Display time in 24-hour format (if non-zero).

//    //    uint8_t reserved_3[20];                 // 0x000000EA  20		//

//    //    uint32_t FFlash_Dimmer_Threshold_clux;  // 0x00000100	4		// Faux flash dimmer threshold in hundredths of lux (clux)

//    //    uint16_t FFlash_On_Time_ms;             // 0x00000104	2		// Faux flash on time (milliseconds)

//    //    uint16_t FFlash_Seq_Dwell_ms;           // 0x00000106	2		// Faux flash sequence dwell time between flashes (milliseconds)

//    //    uint8_t FFlash_Num_Per_Seq;             // 0x00000108	1		// Faux flash number of flashes per sequence

//    //    uint8_t FFlash_Num_Seq;                 // 0x00000109	1		// Faux flash number of repetitions of sequence

//    //    uint16_t FFlash_Inter_Seq_Dwell_ms;     // 0x0000010A	2		// Faux flash dwell time between sequences (milliseconds)

//    //    uint16_t FFlash_Initial_Delay_ms;       // 0x0000010C	2		// Faux flash initial delay to first flash sequence (milliseconds)

//    //    uint8_t Auto_Start_Demo_Mode;           // 0x0000010E	1		// Automatically start demo mode at startup

//    //    uint8_t Capture_Log_Data;               // 0x0000010F	1		// Enable log capture to EEPROM

//    //    uint32_t Data_Logger_Period_min;            // 0x00000110	4		// How frequently data should be logged (minutes)

//    //    uint8_t reserved_4[22];                 // 0x00000114  22		//

//    //    uint16_t Crc;							// 0x0000012A	2		// CRC16-CCITT

//    //}

//    //EEPROM_Param_Data_t;											// 			  300



//    #endregion



//    #region Latest Minor Version 7 (EEPROM_Param_Data_t)

//    //  typedef struct

//    //{

//    // // Type	   // Name					  	  // Offset		Size	// Description

//    // //------------------------------------------------------------------------------------------------------------------------------------------------

//    //    uint32_t sig;                           // 0x00000000	4		// Magic number to ensure valid data is loaded

//    //    uint32_t version_major;                 // 0x00000004	4		// Incompatible with earlier versions

//    //    uint32_t version_incremental;           // 0x00000008	4		// Incremental additions to older versions

//    //    uint8_t Stats_Offload_Option;           // 0x0000000C	1		// Selection for data on demand or streaming. 0 - on demand, 1 - streaming

//    //    uint8_t reserved_0;                     // 0x0000000D	1		//

//    //    uint8_t Temp_Unit;                      // 0x0000000E	1		// Unit for measuring temperature (tempunit_Fahrenheit, tempunit_Celsius, tempunit_Kelvin, tempunit_Rankine) (Temp_Unit_Enum_t)

//    //    uint8_t Speed_Resolution;               // 0x0000000F	1		// Resolution of speed units.  0 - ones, 1 - tenths

//    //    uint8_t Speed_Measurement_Unit;         // 0x00000010	1		// Unit for measuring speed (spdunit_MPH, spdunit_KPH or spdunit_knots, spdunit_MPS, spdunit_FPS) (Speed_Unit_Enum_t)

//    //    uint8_t Resources_First_Write;          // 0x00000011	1		// Resources need initialization

//    //    int16_t Speed_Limit_Hysteresis;         // 0x00000012	2		// Speed to drop before Speed Limit threshold action can be reactivated.  Set to 0xFFFF to disable hysteresis.

//    //    int16_t Max_Speed_Hysteresis;           // 0x00000014	2		// Speed to drop before Max Speed threshold action can be reactivated.  Set to 0xFFFF to disable hysteresis.

//    //    int16_t Min_Speed_Hysteresis;           // 0x00000016	2		// Speed to rise before Min Speed threshold action can be reactivated.  Set to 0xFFFF to disable hysteresis.

//    //    int16_t Alert_Speed_Hysteresis;         // 0x00000018	2		// Speed to drop before Alert speed threshold action can be reactivated.  Set to 0xFFFF to disable hysteresis.

//    //    uint16_t Max_Speed_Threshold;           // 0x0000001A	2		// Max speed displayed.

//    //    uint8_t Min_Speed_Threshold;            // 0x0000001C	1		// Min speed displayed.

//    //    uint8_t Speed_Alert_Duration;           // 0x0000001D	1		// Number of 50 ms frames during which to continue displaying the speed alert (red/blue flashers).

//    //    uint8_t Stats_Enable;                   // 0x0000001E	1		// Enable/Disable stats collection

//    //    uint8_t Enable_USB_Logging;             // 0x0000001F	1		// Enable Log output to USB Serial I/O port

//    //    uint8_t Radar_Sensitivity;              // 0x00000020	1		// Radar Sensitivity 0-16, with 16 = most sensitive.

//    //    uint8_t reserved_1[15];                 // 0x00000025  15		//

//    //    uint32_t Bluetooth_PIN;                 // 0x00000021	4		// PIN to pair with Bluetooth module. Acceptable values 0 - 999,999

//    //    uint32_t Stats_Offload_Time;            // 0x00000034	4		// Time stamp of last stats offload(EPOCH)

//    //    Action_Overlay_t Std_Ops;               // 0x00000038 112		//

//    //    uint32_t Alert_Sequence_Order;          // 0x000000A8	4		// Each nibble represents one frame step in the sequence starting from LSB and Top Left (see Display_Update_Flashers() for more)

//    //    uint32_t Idle_Timeout;                  // 0x000000AC	4		// Time below min speed before moving to idle state if not displaying anything.

//    //    uint32_t Menu_Timeout_s;                    // 0x000000B0	4		// Automatically exit the menu if this amount of time elapses between key-presses while in menu (seconds).

//    //    uint8_t Radar_Enable;                   // 0x000000B4	1		// 0 - Disable, 1 - Enable Internal Radar, 2 - Enable External Radar

//    //    uint8_t Radar_Operating_Direction;      // 0x000000B5	1		// 0 - Both, 1 - Closing, 2 - Away

//    //    uint8_t Radar_Holdover_Time;            // 0x000000B6	1		// Time target is held after signal is lost.  0 - 11 seconds

//    //    uint8_t Radar_Cosine;                   // 0x000000B7	1		// Angle between radar antenna normal and centroid of detection region.  0-70 degrees

//    //    uint8_t Radar_Target_Strength;          // 0x000000B8	1		// AKA Visual Target Strength Sensitivity (1-99, 99 - most sensitive).  Discriminates targets based on absolute signal strength independent of noise floor.

//    //    uint8_t Radar_Target_Acceptance;        // 0x000000B9	1		// AKA Target Acquisition Density (0-100).  Percentage of time target is seen over the course of the Target Acquisition Span time period.

//    //    uint8_t Radar_Target_Hold_On;           // 0x000000BA	1		// AKA Target Loss Density (0-100).  Percentage of time target is seen below which, target is will be considered lost.

//    //    uint8_t Stats_Target_Strength;          // 0x000000BB	1		// AKA Stats Target Strength Sensitivity (1-99, 99 - most sensitive).  Discriminates targets based on absolute signal strength independent of noise floor.

//    //    uint16_t Stats_Min_Tracking_Distance;   // 0x000000BC	2		// Minimum distance between targets to be counted in stats (same units as speed measurement units)

//    //    uint8_t Stats_Min_Following_Time;       // 0x000000BE	1		// Minimum following time between targets to be counted in stats in seconds (0-10)

//    //    uint8_t Speed_Proc_Type;                // 0x000000BF	1		// Method for processing radar speeds accrued during Speed_Display_Min_Hold_Time_cs (Speed_Proc_Type_t)

//    //    uint16_t Speed_Display_Min_Hold_Time_ms;    // 0x000000C0	2		// Speeds displayed must be held for this period of time at a minimum before changing, 0 to bypass, 3000 max.  (milliseconds)

//    //    uint8_t MUTCD_Compliance_Enable;        // 0x000000C3	1		// Enable MUTCD Compliance Mode.

//    //    uint8_t reserved_2[17];                 // 0x000000C2  17 		//

//    //    uint32_t High_clux;                     // 0x000000D4	4		// Highest possible _measured_ brightness in hundredths of lux

//    //    uint8_t High_Brightness;                // 0x000000D8	1		// Best brightness in conditions in which High_clux was measured (0-100 percent brightness)

//    //    uint8_t Min_Brightness;                 // 0x000000D9	1		// Desired brightness at Min_clux (0-100 percent brightness)

//    //    uint8_t Max_Brightness;                 // 0x000000DA	1		// Maximum brightness output--brightness rails out here regardless of higher clux measurements. (0-100 percent brightness)

//    //    uint8_t Lux_Low_Pass_Divider;           // 0x000000DB	1		// Divide error value by this to integrate delta over time

//    //    float Gamma;                            // 0x000000DC	4		// Brightness gamma exponent (0.0 < Gamma < 3.0)

//    //    uint32_t Lux_Measurement_Period_ms;     // 0x000000E0	4		// Ambient light sensor sampling period (milliseconds)

//    //    uint8_t Manual_Brightness_Level;        // 0x000000E4	1		// Override automatic brightness with this level.  Set to 0 for automatic brightness.

//    //    uint8_t Demo_Brightness_Day;            // 0x000000E5	1		// Demo daytime brightness level

//    //    uint8_t Demo_Brightness_Night;          // 0x000000E6	1		// Demo nighttime brightness level

//    //    uint8_t Language;                       // 0x000000E7	1		// Language (Cast to Language_Enum_t)

//    //    uint8_t Power_Mode;                     // 0x000000E8	1		// Power Mode (Cast to Power_Mode_Enum_t)

//    //    uint8_t Time_Format_24_Hour;            // 0x000000E9	1		// Display time in 24-hour format (if non-zero).

//    //    uint8_t reserved_3[20];                 // 0x000000EA  20		//

//    //    uint32_t FFlash_Dimmer_Threshold_clux;  // 0x00000100	4		// Faux flash dimmer threshold in hundredths of lux (clux)

//    //    uint16_t FFlash_On_Time_ms;             // 0x00000104	2		// Faux flash on time (milliseconds)

//    //    uint16_t FFlash_Seq_Dwell_ms;           // 0x00000106	2		// Faux flash sequence dwell time between flashes (milliseconds)

//    //    uint8_t FFlash_Num_Per_Seq;             // 0x00000108	1		// Faux flash number of flashes per sequence

//    //    uint8_t FFlash_Num_Seq;                 // 0x00000109	1		// Faux flash number of repetitions of sequence

//    //    uint16_t FFlash_Inter_Seq_Dwell_ms;     // 0x0000010A	2		// Faux flash dwell time between sequences (milliseconds)

//    //    uint16_t FFlash_Initial_Delay_ms;       // 0x0000010C	2		// Faux flash initial delay to first flash sequence (milliseconds)

//    //    uint8_t Auto_Start_Demo_Mode;           // 0x0000010E	1		// Automatically start demo mode at startup

//    //    uint8_t Capture_Log_Data;               // 0x0000010F	1		// Enable log capture to EEPROM

//    //    uint32_t Data_Logger_Period_min;            // 0x00000110	4		// How frequently data should be logged (minutes)

//    //    uint8_t reserved_4[22];                 // 0x00000114  22		//

//    //    uint16_t Crc;							// 0x0000012A	2		// CRC16-CCITT

//    //}

//    //EEPROM_Param_Data_t;	

//    #endregion





//    public class EEPROM_Param_Data_t

//    {

//        public string errorMsg = string.Empty;



//        private byte[] data = new byte[GlobalConstantDefintion.PARAM_MAP_SIZE_LARGE];



//        // Type		   // Name					            // Offset		Size	// Description

//        //------------------------------------------------------------------------------------------------------------------------------------------------

//        public uint8_t Speed_Resolution;				    // 0x0000000F	1		// Resolution of speed units.  0 - ones, 1 - tenths

//        public uint8_t Speed_Measurement_Unit;              // 0x00000010	1       // Unit for measuring speed (spdunit_MPH, spdunit_KPH or spdunit_knots) (Speed_Unit_Enum_t)



//        public uint8_t Temp_Unit;                           // 0x0000000E	1	



//        public uint16_t Max_Speed_Threshold = 90;           // 0x0000001A	2		// Max speed displayed.

//        public uint8_t Min_Speed_Threshold = 15;           // 0x0000001C	1		// Min speed displayed.

//        public uint8_t Manual_Brightness_Level;             // 0x000000E4	1		// Override automatic brightness with this level.  Set to 0 for automatic brightness.

//        public uint8_t Power_Mode;                          // 0x000000E8	1	    // Power Mode (Cast to Power_Mode_Enum_t)

//        public uint32_t Version_Major = 0xFF000013;         // 0x00000004	4		// Incompatible with earlier versions

//        public uint32_t Version_Incremental = 0;            // 0x00000008	4		// Incremental additions to older versions



//        public uint32_t Feature_Set;	                    // 0x0000002C	4	

//        public uint32_t Bluetooth_PIN;                      // 0x00000030	4		// PIN to pair with Bluetooth module. Acceptable values 0 - 999,999



//        public uint8_t Radar_Sensitivity;				    // 0x00000020	1		// Radar Sensitivity 0-16, with 16 = most sensitive.

//        public uint8_t Radar_Enable;                        // 0x000000B4	1		// 0 - Disable, 1 - Enable Internal Radar, 2 - Bluetooth External Radar, 3 -- RS232 External Radar

//        public uint8_t Radar_Operating_Direction;           // 0x000000B5	1		// 0 - Both, 1 - Closing, 2 - Away

//        public uint8_t Radar_Holdover_Time;                 // 0x000000B6	1		// Time target is held after signal is lost.  0 - 11 seconds

//        public uint8_t Radar_Cosine;                        // 0x000000B7	1		// Angle between radar antenna normal and centroid of detection region.  0-70 degrees

//        public uint8_t Radar_Target_Strength;               // 0x000000B8	1		// AKA Visual Target Strength Sensitivity (1-99, 99 - most sensitive).  Discriminates targets based on absolute signal strength independent of noise floor.

//        public uint8_t Radar_Target_Acceptance;             // 0x000000B9	1		// AKA Target Acquisition Density (0-100).  Percentage of time target is seen over the course of the Target Acquisition Span time period.

//        public uint8_t Radar_Target_Hold_On;                // 0x000000BA	1		// AKA Target Loss Density (0-100).  Percentage of time target is seen below which, target is will be considered lost.



//        // External Radar Parameter Setting

//        public uint8_t Echo_PAN_Radar_Data;                 // 0x00000024	1		// Enable echoing all radar data received from the PAN radio to the RS232 port.

//        public uint8_t External_Radar_Speed;                // 0x00000025	1		// Speed to show when external radar is using bE format.

//                                                            //                      0 - Last/Live, 1 - Peak, 2 - Hit/Spin



//        public uint8_t Stats_Offload_Option;			    // 0x0000000C	1       // Selection for data on demand or streaming. 0 - on demand, 1 - streaming

//        public uint8_t Stats_Enable;					    // 0x0000001E	1		// Enable/Disable stats collection

//        public uint8_t Stats_Target_Strength;              // 0x000000BB	1		// AKA Stats Target Strength Sensitivity (1-99, 99 - most sensitive).  Discriminates targets based on absolute signal strength independent of noise floor.

//        public uint16_t Stats_Min_Tracking_Distance;        // 0x000000BC	2		// Minimum distance between targets to be counted in stats (same units as speed measurement units)

//        public uint8_t Stats_Min_Following_Time;           // 0x000000BE	1		// Minimum following time between targets to be counted in stats in seconds (0-10)



//        public uint32_t Stats_Offload_Time;                 // 0x00000034	4	    // Time stamp of last stats offload(EPOCH)    



//        public uint8_t MUTCD_Compliance_Enable;            // 0x000000C2	1		// Enable MUTCD Compliance Mode.



//        public Action_Overlay_t Std_Ops = new Action_Overlay_t();



//        // ===== GPIO Data ===================

//        public GPIO_Input_t[] GPIO_In = new GPIO_Input_t[4];        // 0x00000114 160		// GPIO Inputs

//        public GPIO_Output_t[] GPIO_Out = new GPIO_Output_t[4];    // 0x000001A4  16		// GPIO Outputs

//        public uint8_t GPIOs_Present = 0;			                // 0x000001B4	1	





//        // Ethernet Data

//        public uint8_t Ethernet_Connection_Type;           // 0x000001C7	1		// 0=DHCP, 1=Static

//        public uint32_t Ethernet_Static_IP;                 // 0x000001C8	4		// IP to use when in Static mode

//        public uint32_t Ethernet_Static_Subnet_Mask;        // 0x000001CC	4		// Net Mask to use when in static mode

//        public uint32_t Ethernet_Static_Default_Gateway;    // 0x000001D0	4		// Gateway to use when in static mode



//        // WIFI Data

//        public uint8_t WIFI_Mode;                                   // 0x000001D4	1		// Modes: 0=Off,1=Station,2=Access Point

//        public uint8_t WIFI_AP_Security;                            // 0x000001D5	1		// AP Security: 0=Open,1=WPA,2=WPA2

//        public uint8_t[] WIFI_AP_Password = new uint8_t[24];       // 0x000001D6	24		// AP Password: 8-23 characters with terminating '\0'

//        public uint8_t[] WIFI_Station_SSID = new uint8_t[24];      // 0x000001EC	24		// SSID to scan for in Station mode with terminating '/0'

//        public uint8_t[] WIFI_Station_Password = new uint8_t[24];  // 0x00000204	24		// Password for SSID with terminating '\0'

//        public uint8_t WIFI_Station_Security;                       // 0x0000021C	1		// SSID Security: 0=Open,1=WPA,2=WPA2

//        public uint8_t WIFI_Station_IP_Type;                        // 0x0000021D	1		// 0=DHCP, 1=Static

//        public uint32_t WIFI_Station_Static_IP;                     // 0x0000021E	4		// IP to use when in Station mode and IP Type is Static

//        public uint32_t WIFI_Station_Static_Mask;                   // 0x00000222	4		// Net Mask to use in Station mode and IP Type is Static

//        public uint32_t WIFI_Station_Static_Gateway;                // 0x00000226	4		// Gateway to use in Station mode and IP Type is Static



//        public const int Bluetooth_PIN_Offset = 0x00000030;



//        public const int Speed_Measurement_Unit_Offset = 0x00000010; // RADAR

//        public const int Speed_Resolution_Offset = 0x0000000F;       // RADAR

//        public const int Max_Speed_Threshold_Offset = 0x0000001A;

//        public const int Min_Speed_Threshold_Offset = 0x0000001C;

//        public const int Manual_Brightness_Level_Offset = 0x000000E4;

//        public const int Power_Mode_Offset = 0x000000E8;

//        public const int Version_Major_Offset = 0x00000004;

//        public const int Version_Incremental_Offset = 0x00000008;

//        public const int Overlay_Action_Offset = 0x00000038;

//        public const int Temp_Unit_Offset = 0x0000000E;

//        public const int MUTCD_Compliance_Enable_Offset = 0x000000C2;



//        // RADAR Parameters

//        public const int Radar_Sensitivity_Offset = 0x00000020;

//        public const int Radar_Enable_Offset = 0x000000B4;

//        public const int Radar_Operating_Direction_Offset = 0x000000B5;

//        public const int Radar_Holdover_Time_Offset = 0x000000B6;

//        public const int Radar_Cosine_Offset = 0x000000B7;

//        public const int Radar_Target_Strength_Offset = 0x000000B8;

//        public const int Radar_Target_Acceptance_Offset = 0x000000B9;

//        public const int Radar_Target_Hold_On_Offset = 0x000000BA;



//        public const int Echo_PAN_Radar_Data_Offset = 0x00000024;

//        public const int External_Radar_Speed_Offset = 0x00000025;



//        public const int Stats_Target_Strength_Offset = 0x000000BB;

//        public const int Stats_Min_Tracking_Distance_Offset = 0x000000BC;

//        public const int Stats_Min_Following_Time_Offset = 0x000000BE;

//        public const int Stats_Enable_Offset = 0x0000001E;

//        public const int Stats_Offload_Option_Offset = 0x0000000C;

//        public const int Stats_Offload_Time_Offset = 0x00000034;



//        public const int GPIO_Input_Offset = 0x00000114;

//        public const int GPIO_Output_Offset = 0x000001B4;

//        public const int GPIOs_Present_Offset = 0x000001C4;



//        public const int GPIO_Input_Item_Size = 40;

//        public const int GPIO_Output_Item_Size = 4;



//        // Ethernet Index

//        public const int Ethernet_Connection_Typ_Offset = 0x000001C7;

//        public const int Ethernet_Static_IP_Offset = 0x000001C8;

//        public const int Ethernet_Static_Subnet_Mask_Offset = 0x000001CC;

//        public const int Ethernet_Static_Default_Gateway_Offset = 0x000001D0;



//        // WIFI Index

//        public const int WIFI_Mode_Offset = 0x000001D4;

//        public const int WIFI_AP_Security_Offset = 0x000001D5;

//        public const int WIFI_AP_Password_Offset = 0x000001D6;



//        //

//        // These field offset are off by 2 bytes. We correct it here

//        //

//        public const int WIFI_Station_SSID_Offset = 0x000001EE;

//        public const int WIFI_Station_Password_Offset = 0x00000206;

//        public const int WIFI_Station_Security_Offset = 0x0000021E;

//        public const int WIFI_Station_IP_Type_Offset = 0x0000021F;

//        public const int WIFI_Station_Static_IP_Offset = 0x00000220;

//        public const int WIFI_Station_Static_Mask_Offset = 0x00000224;

//        public const int WIFI_Station_Static_Gateway_Offset = 0x00000228;



//        public const int Feature_Set_Offset = 0x0000002C;



//        public EEPROM_Param_Data_t()

//        {

//            for (int i = 0; i < 4; i++)

//            {

//                GPIO_In[i] = new GPIO_Input_t();

//                GPIO_Out[i] = new GPIO_Output_t();

//            }



//            string s = GlobalConstantDefintion.DEFAULT_PMG_PARAM_MAP;



//            s = Regex.Replace(s, @"\s+", "");

//            s = Regex.Replace(s, "[^A-Za-z0-9 _]", "");



//            data = Utils.StringToByteArrayFastest(s);

//            decode();

//        }



//        public bool setData(byte[] dataIn)

//        {

//            //

//            // Beta 4 and before: 300 bytes (PARAM_MAP_SIZE_BETA_4)

//            // GPIO Version and After: 1404 (PARAM_MAP_SIZE)

//            //

//            if (dataIn != null && (dataIn.Length == GlobalConstantDefintion.PARAM_MAP_SIZE_LARGE ||

//                dataIn.Length == GlobalConstantDefintion.PARAM_MAP_SIZE_BETA_4))

//            {

//                data = dataIn;

//                return decode();

//            }

//            else

//            {

//                return false;

//            }

//        }



//        public bool decode()

//        {

//            int byteIdx, i;



//            errorMsg = string.Empty;



//            try

//            {

//                Speed_Measurement_Unit = Util.GetUint8(data, Speed_Measurement_Unit_Offset);

//                Max_Speed_Threshold = Util.GetUint16(data, Max_Speed_Threshold_Offset);



//                Min_Speed_Threshold = Util.GetUint8(data, Min_Speed_Threshold_Offset);

//                Manual_Brightness_Level = Util.GetUint8(data, Manual_Brightness_Level_Offset);

//                Power_Mode = Util.GetUint8(data, Power_Mode_Offset);

//                Version_Major = Util.GetUint32(data, Version_Major_Offset);

//                Version_Incremental = Util.GetUint32(data, Version_Incremental_Offset);



//                byteIdx = Overlay_Action_Offset;

//                Std_Ops.decode(data, ref byteIdx);



//                Temp_Unit = Util.GetUint8(data, Temp_Unit_Offset);



//                Bluetooth_PIN = Util.GetUint32(data, Bluetooth_PIN_Offset);



//                Speed_Resolution = Util.GetUint8(data, Speed_Resolution_Offset);

//                Radar_Sensitivity = Util.GetUint8(data, Radar_Sensitivity_Offset);

//                Radar_Enable = Util.GetUint8(data, Radar_Enable_Offset);

//                Radar_Operating_Direction = Util.GetUint8(data, Radar_Operating_Direction_Offset);

//                Radar_Holdover_Time = Util.GetUint8(data, Radar_Holdover_Time_Offset);

//                Radar_Cosine = Util.GetUint8(data, Radar_Cosine_Offset);

//                Radar_Target_Strength = Util.GetUint8(data, Radar_Target_Strength_Offset);

//                Radar_Target_Acceptance = Util.GetUint8(data, Radar_Target_Acceptance_Offset);

//                Radar_Target_Hold_On = Util.GetUint8(data, Radar_Target_Hold_On_Offset);



//                Echo_PAN_Radar_Data = Util.GetUint8(data, Echo_PAN_Radar_Data_Offset);

//                External_Radar_Speed = Util.GetUint8(data, External_Radar_Speed_Offset);



//                Stats_Enable = Util.GetUint8(data, Stats_Enable_Offset);

//                Stats_Target_Strength = Util.GetUint8(data, Stats_Target_Strength_Offset);

//                Stats_Min_Tracking_Distance = Util.GetUint16(data, Stats_Min_Tracking_Distance_Offset);

//                Stats_Min_Following_Time = Util.GetUint8(data, Stats_Min_Following_Time_Offset);



//                Stats_Offload_Option = Util.GetUint8(data, Stats_Offload_Option_Offset);



//                Stats_Offload_Time = Util.GetUint32(data, Stats_Offload_Time_Offset);



//                MUTCD_Compliance_Enable = Util.GetUint8(data, MUTCD_Compliance_Enable_Offset);



//                Ethernet_Connection_Type = Util.GetUint8(data, Ethernet_Connection_Typ_Offset);

//                Ethernet_Static_IP = Util.GetUint32(data, Ethernet_Static_IP_Offset);

//                Ethernet_Static_Subnet_Mask = Util.GetUint32(data, Ethernet_Static_Subnet_Mask_Offset);

//                Ethernet_Static_Default_Gateway = Util.GetUint32(data, Ethernet_Static_Default_Gateway_Offset);



//                // WIFI

//                WIFI_Mode = Util.GetUint8(data, WIFI_Mode_Offset);



//                WIFI_AP_Security = Util.GetUint8(data, WIFI_AP_Security_Offset);

//                if (WIFI_AP_Security > (byte)WifiSecurityType.WPA2)

//                    WIFI_AP_Security = 0;



//                WIFI_AP_Password = Util.GetByteArray(data, WIFI_AP_Password_Offset, 24);

//                WIFI_Station_SSID = Util.GetByteArray(data, WIFI_Station_SSID_Offset, 24);

//                WIFI_Station_Password = Util.GetByteArray(data, WIFI_Station_Password_Offset, 24);



//                WIFI_Station_Security = Util.GetUint8(data, WIFI_Station_Security_Offset);

//                if (WIFI_Station_Security > (byte)WifiSecurityType.WPA2)

//                    WIFI_Station_Security = 0;



//                WIFI_Station_IP_Type = Util.GetUint8(data, WIFI_Station_IP_Type_Offset);



//                WIFI_Station_Static_IP = Util.GetUint32(data, WIFI_Station_Static_IP_Offset);

//                WIFI_Station_Static_Mask = Util.GetUint32(data, WIFI_Station_Static_Mask_Offset);

//                WIFI_Station_Static_Gateway = Util.GetUint32(data, WIFI_Station_Static_Gateway_Offset);



//                Feature_Set = Util.GetUint32(data, Feature_Set_Offset);



//                //

//                // GPIO is added for large parameter map

//                //

//                if (data.Length == GlobalConstantDefintion.PARAM_MAP_SIZE_LARGE)

//                {

//                    // GPIO

//                    GPIOs_Present = Util.GetUint8(data, GPIOs_Present_Offset);



//                    for (i = 0; i < 4; i++)

//                    {

//                        byteIdx = GPIO_Input_Offset + (i * GPIO_Input_Item_Size);

//                        GPIO_In[i].decode(data, ref byteIdx);

//                    }



//                    for (i = 0; i < 4; i++)

//                    {

//                        byteIdx = GPIO_Output_Offset + (i * GPIO_Output_Item_Size);

//                        GPIO_Out[i].decode(data, ref byteIdx);

//                    }

//                }

//            }

//            catch (Exception e)

//            {

//                errorMsg = "Exception = " + e.Message;



//                //LogForm.LogMessage("Exception: \n" + e.ToString(), System.Drawing.Color.Red,

//                //                   true, LogForm.Direction.Internal, 0);



//                return false;

//            }



//            return true;

//        }

//        public byte[] encode()

//        {

//            int byteIdx, i;



//            Util.SetUint8(Speed_Measurement_Unit, ref data, Speed_Measurement_Unit_Offset);

//            Util.SetUint16(Max_Speed_Threshold, ref data, Max_Speed_Threshold_Offset);



//            Util.SetUint8(Min_Speed_Threshold, ref data, Min_Speed_Threshold_Offset);

//            Util.SetUint8(Manual_Brightness_Level, ref data, Manual_Brightness_Level_Offset);

//            Util.SetUint8(Power_Mode, ref data, Power_Mode_Offset);



//            //

//            // Version will come from PMG when parameter map message is received and decoded.

//            // We don't change the version. However, we do need to handle following requirement

//            // from Mike.

//            //



//            /*** From Mike ************************************************************************

//            Russell has requested that we ensure the PC, IOS &Android tools all maintain

//            backward compatibility with both the Beta 4 Parameter Map (Major Version 0xFF00000F)

//            and the latest GPIO build(Major Version 0xFF000013).Note that no Major Versions

//            between 0xFF00000F and 0xFF000013 will be released.

//            The following information should simplify the necessary implementation:

 

//            1) Both versions are identical up to, but not including reserved_4 located at offset 

//               0x00000114.This offset is the location of GPIO_In[4] in version 0xFF000013.



//            2) Crc is the only field common to the two maps that has moved, the common portion of 

//               the map can be accessed with a common structure, so most of your existing code will work fine.



//            Thus, when you receive the parameter map on initial connection, you need to do the following:

//            1) Check the Major Version and determine which of these two major versions is present.



//            2) Calculate the CRC over:

//		        a.The first 298 bytes for version 0xFF00000F and compare with the Crc located at offset 298



//                b.The first 1402 bytes and compare with the Crc located at offset 1402



//            3) If neither version is present, state that the sign firmware is incompatible with 

//               the tool and instruct the user to check for an update for the tool, or, if no update 

//               is present, to contact ACI support.

//            4) Cast the parameter map to the correct structure.



//            5) Enable the GPIO feature only if the new parameter map is present.



//            6) And, finally use the same version when sending a parameter map back to the sign.



//            ****************************************************************************************/



//            Util.SetUint32(Version_Major, ref data, Version_Major_Offset);



//            //

//            // From Devin's input, parameter map version increment 

//            // needs to be bumped to version 4 to have ethernet setting enabled

//            //

//            if (Version_Incremental == 3)

//                Version_Incremental = 4;



//            Util.SetUint32(Version_Incremental, ref data, Version_Incremental_Offset);



//            byteIdx = Overlay_Action_Offset;

//            Std_Ops.encode(ref data, ref byteIdx);



//            Util.SetUint32(Bluetooth_PIN, ref data, Bluetooth_PIN_Offset);



//            Util.SetUint8(Temp_Unit, ref data, Temp_Unit_Offset);

//            Util.SetUint8(Speed_Resolution, ref data, Speed_Resolution_Offset);

//            Util.SetUint8(Radar_Sensitivity, ref data, Radar_Sensitivity_Offset);

//            Util.SetUint8(Radar_Enable, ref data, Radar_Enable_Offset);

//            Util.SetUint8(Radar_Operating_Direction, ref data, Radar_Operating_Direction_Offset);

//            Util.SetUint8(Radar_Holdover_Time, ref data, Radar_Holdover_Time_Offset);

//            Util.SetUint8(Radar_Cosine, ref data, Radar_Cosine_Offset);

//            Util.SetUint8(Radar_Target_Strength, ref data, Radar_Target_Strength_Offset);

//            Util.SetUint8(Radar_Target_Acceptance, ref data, Radar_Target_Acceptance_Offset);

//            Util.SetUint8(Radar_Target_Hold_On, ref data, Radar_Target_Hold_On_Offset);



//            Util.SetUint8(Echo_PAN_Radar_Data, ref data, Echo_PAN_Radar_Data_Offset);

//            Util.SetUint8(External_Radar_Speed, ref data, External_Radar_Speed_Offset);



//            Util.SetUint8(Stats_Enable, ref data, Stats_Enable_Offset);

//            Util.SetUint8(Stats_Target_Strength, ref data, Stats_Target_Strength_Offset);

//            Util.SetUint16(Stats_Min_Tracking_Distance, ref data, Stats_Min_Tracking_Distance_Offset);

//            Util.SetUint8(Stats_Min_Following_Time, ref data, Stats_Min_Following_Time_Offset);



//            Util.SetUint8(Stats_Offload_Option, ref data, Stats_Offload_Option_Offset);

//            Util.SetUint32(Stats_Offload_Time, ref data, Stats_Offload_Time_Offset);



//            Util.SetUint8(MUTCD_Compliance_Enable, ref data, MUTCD_Compliance_Enable_Offset);



//            Util.SetUint8(Ethernet_Connection_Type, ref data, Ethernet_Connection_Typ_Offset);



//            if (Ethernet_Connection_Type == 0) // DHCP

//            {

//                Util.SetUint32(0, ref data, Ethernet_Static_IP_Offset);

//                Util.SetUint32(0, ref data, Ethernet_Static_Subnet_Mask_Offset);

//                Util.SetUint32(0, ref data, Ethernet_Static_Default_Gateway_Offset);

//            }

//            else

//            {

//                Util.SetUint32(Ethernet_Static_IP, ref data, Ethernet_Static_IP_Offset);

//                Util.SetUint32(Ethernet_Static_Subnet_Mask, ref data, Ethernet_Static_Subnet_Mask_Offset);

//                Util.SetUint32(Ethernet_Static_Default_Gateway, ref data, Ethernet_Static_Default_Gateway_Offset);

//            }



//            // WIFI        

//            Util.SetUint8(WIFI_Mode, ref data, WIFI_Mode_Offset);

//            Util.SetUint8(WIFI_AP_Security, ref data, WIFI_AP_Security_Offset);



//            Util.SetByteArray(WIFI_AP_Password, ref data, WIFI_AP_Password_Offset);

//            Util.SetByteArray(WIFI_Station_SSID, ref data, WIFI_Station_SSID_Offset);

//            Util.SetByteArray(WIFI_Station_Password, ref data, WIFI_Station_Password_Offset);



//            Util.SetUint8(WIFI_Station_Security, ref data, WIFI_Station_Security_Offset);

//            Util.SetUint8(WIFI_Station_IP_Type, ref data, WIFI_Station_IP_Type_Offset);



//            Util.SetUint32(WIFI_Station_Static_IP, ref data, WIFI_Station_Static_IP_Offset);

//            Util.SetUint32(WIFI_Station_Static_Mask, ref data, WIFI_Station_Static_Mask_Offset);

//            Util.SetUint32(WIFI_Station_Static_Gateway, ref data, WIFI_Station_Static_Gateway_Offset);



//            //

//            // Large parameter map for GPIO and after version

//            //

//            if (Version_Major >= GlobalConstantDefintion.PARAM_MAP_VERSION_MAJOR_GPIO)

//            {

//                // GPIO

//                Util.SetUint8(GPIOs_Present, ref data, GPIOs_Present_Offset);



//                for (i = 0; i < 4; i++)

//                {

//                    byteIdx = GPIO_Input_Offset + (i * GPIO_Input_Item_Size);

//                    GPIO_In[i].encode(ref data, ref byteIdx);

//                }



//                for (i = 0; i < 4; i++)

//                {

//                    byteIdx = GPIO_Output_Offset + (i * GPIO_Output_Item_Size);

//                    GPIO_Out[i].encode(ref data, ref byteIdx);

//                }

//            }



//            // Update Map's CRC

//            int msgLen = data.Length;

//            uint16_t cs = Util.U16ComputeCRC(data, 0, msgLen - 2);



//            data[msgLen - 2] = (byte)(cs & 0xFF);

//            data[msgLen - 1] = (byte)((cs >> 8) & 0xFF);



//            return data;

//        }



//        public int getHashValue()

//        {

//            int mapHash = data[data.Length - 2] + (data[data.Length - 1] << 8);

//            return mapHash;

//        }

//        public List<string> getReferencedPageFilenameList()

//        {

//            List<string> filenameList = new List<string>();



//            string filename = Std_Ops.Alert_Action.getFilename();



//            if (filename != null && filename.Length > 0)

//                filenameList.Add(filename);



//            filename = Std_Ops.Idle_Action.getFilename();

//            if (filename != null && filename.Length > 0)

//                filenameList.Add(filename);



//            filename = Std_Ops.Limit_Action.getFilename();

//            if (filename != null && filename.Length > 0)

//                filenameList.Add(filename);



//            // Check GPIOs

//            //

//            //    gpio_present_In_1 = 0x01,

//            //    gpio_present_In_2 = 0x02,

//            //    gpio_present_In_3 = 0x04,

//            //    gpio_present_In_4 = 0x08,

//            //

//            for (int i = 0; i < 4; i++)

//            {

//                if ((GPIOs_Present & (byte)(0x01 << i)) != 0)

//                {

//                    filename = GPIO_In[i].Action.getFilename();

//                    if (filename != string.Empty)

//                        filenameList.Add(filename);

//                }

//            }



//            // Remove duplication

//            filenameList = filenameList.Distinct().ToList();



//            return filenameList;

//        }



//        public string toHexString()

//        {

//            byte[] data2 = encode();

//            string s = string.Empty;



//            for (int i = 0; i < data2.Length; i++)

//            {

//                if (i != 0 && (i % 30 == 0))

//                    s += Environment.NewLine;



//                s += (data2[i].ToString("X2") + " ");

//            }



//            return s;

//        }

//        public Boolean fromHexString(string s)

//        {

//            s = Regex.Replace(s, @"\s+", "");

//            s = Regex.Replace(s, "[^A-Za-z0-9 _]", "");



//            byte[] data2 = Utils.StringToByteArrayFastest(s);



//            if (data2 == null || data2.Length != GlobalConstantDefintion.PARAM_MAP_SIZE_LARGE)

//                return false;



//            EEPROM_Param_Data_t other = new EEPROM_Param_Data_t();



//            if (!other.setData(data2))

//                return false;



//            Speed_Resolution = other.Speed_Resolution;

//            Speed_Measurement_Unit = other.Speed_Measurement_Unit;

//            Temp_Unit = other.Temp_Unit;

//            Max_Speed_Threshold = other.Max_Speed_Threshold;

//            Min_Speed_Threshold = other.Min_Speed_Threshold;

//            Manual_Brightness_Level = other.Manual_Brightness_Level;

//            Power_Mode = other.Power_Mode;

//            Bluetooth_PIN = other.Bluetooth_PIN;

//            Radar_Sensitivity = other.Radar_Sensitivity;

//            Radar_Enable = other.Radar_Enable;

//            Radar_Operating_Direction = other.Radar_Operating_Direction;

//            Radar_Holdover_Time = other.Radar_Holdover_Time;

//            Radar_Cosine = other.Radar_Cosine;

//            Radar_Target_Strength = other.Radar_Target_Strength;

//            Radar_Target_Acceptance = other.Radar_Target_Acceptance;

//            Radar_Target_Hold_On = other.Radar_Target_Hold_On;

//            Echo_PAN_Radar_Data = other.Echo_PAN_Radar_Data;

//            External_Radar_Speed = other.External_Radar_Speed;

//            Stats_Offload_Option = other.Stats_Offload_Option;

//            Stats_Enable = other.Stats_Enable;

//            Stats_Target_Strength = other.Stats_Target_Strength;

//            Stats_Min_Tracking_Distance = other.Stats_Min_Tracking_Distance;

//            Stats_Min_Following_Time = other.Stats_Min_Following_Time;

//            Stats_Offload_Time = other.Stats_Offload_Time;

//            MUTCD_Compliance_Enable = other.MUTCD_Compliance_Enable;

//            Std_Ops = other.Std_Ops;

//            GPIO_In = other.GPIO_In;

//            GPIO_Out = other.GPIO_Out;

//            GPIOs_Present = other.GPIOs_Present;

//            Ethernet_Connection_Type = other.Ethernet_Connection_Type;

//            Ethernet_Static_IP = other.Ethernet_Static_IP;

//            Ethernet_Static_Subnet_Mask = other.Ethernet_Static_Subnet_Mask;

//            Ethernet_Static_Default_Gateway = other.Ethernet_Static_Default_Gateway;

//            WIFI_Mode = other.WIFI_Mode;

//            WIFI_AP_Security = other.WIFI_AP_Security;

//            WIFI_AP_Password = other.WIFI_AP_Password;

//            WIFI_Station_SSID = other.WIFI_Station_SSID;

//            WIFI_Station_Password = other.WIFI_Station_Password;

//            WIFI_Station_Security = other.WIFI_Station_Security;

//            WIFI_Station_IP_Type = other.WIFI_Station_IP_Type;

//            WIFI_Station_Static_IP = other.WIFI_Station_Static_IP;

//            WIFI_Station_Static_Mask = other.WIFI_Station_Static_Mask;

//            WIFI_Station_Static_Gateway = other.WIFI_Station_Static_Gateway;



//            return true;

//        }

//        public string toString()

//        {

//            int i;

//            string s = string.Empty;



//            s += ("Version_Major = " + Utils.ToHexString(Version_Major) + "\n");

//            s += ("Version_Incremental = " + Utils.ToHexString(Version_Incremental) + "\n");

//            s += ("Speed_Measurement_Unit = " + ((Speed_Unit_Enum_t)Speed_Measurement_Unit).ToString() + "\n");

//            s += ("Speed_Limit = " + Std_Ops.Speed_Limit.ToString() + "\n");

//            s += ("Min_Speed_Threshold = " + Min_Speed_Threshold.ToString() + "\n");

//            s += ("Max_Speed_Threshold = " + Max_Speed_Threshold.ToString() + "\n");

//            s += ("Alert_Speed_Threshold = " + Std_Ops.Alert_Speed.ToString() + "\n");

//            s += ("Over_Limit_Action = " + Std_Ops.Limit_Action.toString() + "\n");

//            s += ("Alert_Action = " + Std_Ops.Alert_Action.toString() + "\n");

//            s += ("Idle_Action = " + Std_Ops.Idle_Action.toString() + "\n");

//            s += ("Manual_Brightness_Level = " + Manual_Brightness_Level.ToString() + "\n");

//            s += ("Power_Mode = " + ((Power_Mode_Enum_t)Power_Mode).ToString() + "\n");

//            s += ("Speed_Resolution = " + Speed_Resolution + "\n");

//            s += ("Radar_Sensitivity = " + Radar_Sensitivity + "\n");

//            s += ("Radar_Enable = " + Radar_Enable + "\n");

//            s += ("Radar_Operating_Direction = " + Radar_Operating_Direction + "\n");

//            s += ("Radar_Holdover_Time = " + Radar_Holdover_Time + "\n");

//            s += ("Radar_Cosine = " + Radar_Cosine + "\n");

//            s += ("Radar_Target_Strength = " + Radar_Target_Strength + "\n");

//            s += ("Radar_Target_Acceptance = " + Radar_Target_Acceptance + "\n");

//            s += ("Radar_Target_Hold_On = " + Radar_Target_Hold_On + "\n");



//            s += ("Echo_PAN_Radar_Data = " + Echo_PAN_Radar_Data + "\n");

//            s += ("External_Radar_Speed = " + ((ExternalRadarSpeed)External_Radar_Speed).ToString() + "\n");



//            s += ("Stats_Enable = " + Stats_Enable + "\n");

//            s += ("Stats_Target_Strength = " + Stats_Target_Strength + "\n");

//            s += ("Stats_Min_Tracking_Distance = " + Stats_Min_Tracking_Distance + "\n");

//            s += ("Stats_Min_Following_Time = " + Stats_Min_Following_Time + "\n");

//            s += ("Temp_Unit = " + ((Temp_Unit_Enum_t)Temp_Unit).ToString() + "\n");



//            s += ("Stats_Offload_Option = " + ((StatsOffloadOption)Stats_Offload_Option).ToString() + "\n");

//            s += ("Bluetooth_PIN = " + Bluetooth_PIN + "\n");

//            s += ("MUTCD_Compliance_Enable = " + MUTCD_Compliance_Enable + "\n");



//            s += ("Ethernet_IP_Type = " +

//                        ((IPType)Ethernet_Connection_Type).ToString() + "\n");



//            s += ("Ethernet_Static_IP = " + Util.UintToIP(Ethernet_Static_IP) + "\n");

//            s += ("Ethernet_Static_Subnet_Mask = " + Util.UintToIP(Ethernet_Static_Subnet_Mask) + "\n");

//            s += ("Ethernet_Static_Default_Gateway = " + Util.UintToIP(Ethernet_Static_Default_Gateway) + "\n");



//            // WIFI

//            s += ("WIFI_Mode = " + ((WifiMode)WIFI_Mode).ToString() + "\n");

//            s += ("WIFI_AP_Security = " + ((WifiSecurityType)WIFI_AP_Security).ToString() + "\n");

//            s += ("WIFI_AP_Password = [" + Util.GetAsciiString(WIFI_AP_Password) + "]\n");

//            s += ("WIFI_Station_SSID = [" + Util.GetAsciiString(WIFI_Station_SSID) + "]\n");

//            s += ("WIFI_Station_Password = [" + Util.GetAsciiString(WIFI_Station_Password) + "]\n");

//            s += ("WIFI_Station_Security = " + ((WifiSecurityType)WIFI_Station_Security).ToString() + "\n");

//            s += ("WIFI_Station_IP_Type = " + ((IPType)WIFI_Station_IP_Type).ToString() + "\n");

//            s += ("WIFI_Station_Static_IP = " + Util.UintToIP(WIFI_Station_Static_IP) + "\n");

//            s += ("WIFI_Station_Static_Mask = " + Util.UintToIP(WIFI_Station_Static_Mask) + "\n");

//            s += ("WIFI_Station_Static_Gateway = " + Util.UintToIP(WIFI_Station_Static_Gateway) + "\n");



//            s += ("Feature Set = " + Utils.ToHexString(Feature_Set) + "\n");



//            if (Version_Major >= GlobalConstantDefintion.PARAM_MAP_VERSION_MAJOR_GPIO)

//            {

//                s += ("GPIOs_Present = " + Utils.ToBitsString(GPIOs_Present) + "\n");



//                for (i = 0; i < 4; i++)

//                    s += "[GPIO_In " + (i + 1) + "] = " + (GPIO_In[i].toString() + "\n");



//                for (i = 0; i < 4; i++)

//                    s += "[GPIO_Out " + (i + 1) + "] = " + (GPIO_Out[i].toString() + "\n");

//            }



//            return s;

//        }



//        public List <ParamConfigureEntry> getParamConfigureEntryList()

//        {

//            List<ParamConfigureEntry> paramList = new List<ParamConfigureEntry>();



//            // Idle

//            paramList.Add(new ParamConfigureEntry(ParamaterId.IdleDisplay, (uint)Std_Ops.Idle_Action.primary));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.IdleDisplayPage, Std_Ops.Idle_Action.getFilename()));



//            // Limit Display

//            paramList.Add(new ParamConfigureEntry(ParamaterId.SpeedLimit, (uint)Std_Ops.Speed_Limit));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.SpeedLimitDisplay, (uint)Std_Ops.Limit_Action.primary));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.SpeedLimitDisplayPage, Std_Ops.Limit_Action.getFilename()));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.SpeedLimitAlertAction, (uint)Std_Ops.Limit_Action.alert));



//            // Alert Display

//            paramList.Add(new ParamConfigureEntry(ParamaterId.AlertLimit, (uint)Std_Ops.Alert_Speed));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.AlertLimitDisplay, (uint)Std_Ops.Alert_Action.primary));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.AlertLimitDisplayPage, Std_Ops.Alert_Action.getFilename()));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.AlertLimitAlertAction, (uint)Std_Ops.Alert_Action.alert));



//            // Configuration Tab

//            paramList.Add(new ParamConfigureEntry(ParamaterId.MinLimit, (uint)Min_Speed_Threshold));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.MaxLimit, (uint)Max_Speed_Threshold));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.SpeedUnit, (uint)Speed_Measurement_Unit));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.TemperatureUnit, (uint)Temp_Unit));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.Brightness, (uint)Manual_Brightness_Level));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.EnableMUTCDCompliance, (uint)MUTCD_Compliance_Enable));



//            // Radar

//            paramList.Add(new ParamConfigureEntry(ParamaterId.Radar, (uint)Radar_Enable));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.RadarHoldoverTime, (uint)Radar_Holdover_Time));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.RadarCosine, (uint)Radar_Cosine));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.RadarUnitResolution, (uint)Speed_Resolution));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.RadarSensitivity, (uint)Radar_Sensitivity));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.RadarTargetStrength, (uint)Radar_Target_Strength));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.RadarTargetAcceptance, (uint)Radar_Target_Acceptance));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.RadarTargetHoldOn, (uint)Radar_Target_Hold_On));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.RadarOperationDirection, (uint)Radar_Operating_Direction));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.RadarExternalRadarSpeed, (uint)External_Radar_Speed));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.RadarExternalEchoPanRadarData, (uint)Echo_PAN_Radar_Data));



//            // Traffic Data

//            paramList.Add(new ParamConfigureEntry(ParamaterId.TrafficEnableRecording, (uint)Stats_Enable));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.TrafficTargetStrength, (uint)Stats_Target_Strength));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.TrafficMinimumTrackingDistance, (uint)Stats_Min_Tracking_Distance));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.TrafficMinimumFollowingTime, (uint)Stats_Min_Following_Time));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.TrafficDataOnDemand, (uint)Stats_Offload_Option));



//            // Wireless PIN

//            paramList.Add(new ParamConfigureEntry(ParamaterId.WirelessPIN, (uint)Bluetooth_PIN));



//            // Ethernet

//            paramList.Add(new ParamConfigureEntry(ParamaterId.EthernetIPSetting, (uint)Ethernet_Connection_Type));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.EthernetIPAddress, (uint)Ethernet_Static_IP));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.EthernetSubnetMask, (uint)Ethernet_Static_Subnet_Mask));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.EthernetDefaultGateway, (uint)Ethernet_Static_Default_Gateway));



//            // WiFI

//            paramList.Add(new ParamConfigureEntry(ParamaterId.WifiMode, (uint)WIFI_Mode));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.WifiAccessPointSecurity, (uint)WIFI_AP_Security));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.WifiAccessPointPassword, Util.GetAsciiString(WIFI_AP_Password)));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationSecurity, (uint)WIFI_Station_Security));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationPassword, Util.GetAsciiString(WIFI_Station_Password)));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationSSID, Util.GetAsciiString(WIFI_Station_SSID)));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationIPType, (uint)WIFI_Station_IP_Type));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationIPAddress, (uint)WIFI_Station_Static_IP));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationSubnetMask, (uint)WIFI_Station_Static_Mask));

//            paramList.Add(new ParamConfigureEntry(ParamaterId.WifiStationDefaultGateway, (uint)WIFI_Station_Static_Gateway));



//            return paramList;

//        }



//        public void setParamConfigureEntryList(List<ParamConfigureEntry> entryList)

//        {

//            int i;



//            for  (i = 0; i < entryList.Count; i++)

//            {

//                switch (entryList[i].paramId)

//                {

//                    // Idle

//                    case ParamaterId.IdleDisplay:

//                        Std_Ops.Idle_Action.primary = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.IdleDisplayPage:

//                        Std_Ops.Idle_Action.setFilename(entryList[i].value);

//                        break;



//                    // Limit Display

//                    case ParamaterId.SpeedLimit:

//                        Std_Ops.Speed_Limit = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.SpeedLimitDisplay:

//                        Std_Ops.Limit_Action.primary = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.SpeedLimitDisplayPage:

//                        Std_Ops.Limit_Action.setFilename(entryList[i].value);

//                        break;

//                    case ParamaterId.SpeedLimitAlertAction:

//                        Std_Ops.Limit_Action.alert = (byte)entryList[i].getUintValue();

//                        break;



//                    // Alert Display

//                    case ParamaterId.AlertLimit:

//                        Std_Ops.Alert_Speed = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.AlertLimitDisplay:

//                        Std_Ops.Alert_Action.primary = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.AlertLimitDisplayPage:

//                        Std_Ops.Alert_Action.setFilename(entryList[i].value);

//                        break;

//                    case ParamaterId.AlertLimitAlertAction:

//                        Std_Ops.Alert_Action.alert = (byte)entryList[i].getUintValue();

//                        break;



//                    // Configuration Tab

//                    case ParamaterId.MinLimit:

//                        Min_Speed_Threshold = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.MaxLimit:

//                        Max_Speed_Threshold = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.SpeedUnit:

//                        Speed_Measurement_Unit = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.TemperatureUnit:

//                        Temp_Unit = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.Brightness:

//                        Manual_Brightness_Level = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.EnableMUTCDCompliance:

//                        MUTCD_Compliance_Enable = (byte)entryList[i].getUintValue();

//                        break;



//                    // Radar

//                    case ParamaterId.Radar:

//                        Radar_Enable = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.RadarHoldoverTime:

//                        Radar_Holdover_Time = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.RadarCosine:

//                        Radar_Cosine = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.RadarUnitResolution:

//                        Speed_Resolution = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.RadarSensitivity:

//                        Radar_Sensitivity = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.RadarTargetStrength:

//                        Radar_Target_Strength = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.RadarTargetAcceptance:

//                        Radar_Target_Acceptance = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.RadarTargetHoldOn:

//                        Radar_Target_Hold_On = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.RadarOperationDirection:

//                        Radar_Operating_Direction = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.RadarExternalRadarSpeed:

//                        External_Radar_Speed = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.RadarExternalEchoPanRadarData:

//                        Echo_PAN_Radar_Data = (byte)entryList[i].getUintValue();

//                        break;



//                    // Traffic Data

//                    case ParamaterId.TrafficEnableRecording:

//                        Stats_Enable = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.TrafficTargetStrength:

//                        Stats_Target_Strength = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.TrafficMinimumTrackingDistance:

//                        Stats_Min_Tracking_Distance = (UInt16)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.TrafficMinimumFollowingTime:

//                        Stats_Min_Following_Time = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.TrafficDataOnDemand:

//                        Stats_Offload_Option = (byte)entryList[i].getUintValue();

//                        break;



//                    // Wireless PIN

//                    case ParamaterId.WirelessPIN:

//                        Bluetooth_PIN = entryList[i].getUintValue();

//                        break;



//                    // Ethernet

//                    case ParamaterId.EthernetIPSetting:

//                        Ethernet_Connection_Type = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.EthernetIPAddress:

//                        Ethernet_Static_IP = entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.EthernetSubnetMask:

//                        Ethernet_Static_Subnet_Mask = entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.EthernetDefaultGateway:

//                        Ethernet_Static_Default_Gateway = entryList[i].getUintValue();

//                        break;



//                    // WiFI

//                    case ParamaterId.WifiMode:

//                        WIFI_Mode = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.WifiAccessPointSecurity:

//                        WIFI_AP_Security = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.WifiAccessPointPassword:

//                        Util.StringToByteArray(entryList[i].value, ref WIFI_AP_Password);

//                        break;

//                    case ParamaterId.WifiStationSecurity:

//                        WIFI_Station_Security = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.WifiStationPassword:

//                        Util.StringToByteArray(entryList[i].value, ref WIFI_Station_Password);

//                        break;

//                    case ParamaterId.WifiStationSSID:

//                        Util.StringToByteArray(entryList[i].value, ref WIFI_Station_SSID);

//                        break;

//                    case ParamaterId.WifiStationIPType:

//                        WIFI_Station_IP_Type = (byte)entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.WifiStationIPAddress:

//                        WIFI_Station_Static_IP = entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.WifiStationSubnetMask:

//                        WIFI_Station_Static_Mask = entryList[i].getUintValue();

//                        break;

//                    case ParamaterId.WifiStationDefaultGateway:

//                        WIFI_Station_Static_Gateway = entryList[i].getUintValue();

//                        break;

//                }

//            }

//        }



//        public Boolean syntaxCheck(ref string errorMsg)

//        {

//            ActionType actionType;



//            actionType = (ActionType)Std_Ops.Idle_Action.primary;



//            if (actionType == ActionType.ShowText ||

//                actionType == ActionType.ShowGraphics ||

//                actionType == ActionType.ShowAnimation ||

//                actionType == ActionType.ShowComposite)

//            {

//                if (Std_Ops.Idle_Action.getFilename() == string.Empty)

//                {

//                    errorMsg = "Idle Display Page is not set !";

//                    return false;

//                }

//            }



//            actionType = (ActionType)Std_Ops.Limit_Action.primary;



//            if (actionType == ActionType.ShowText ||

//                actionType == ActionType.ShowGraphics ||

//                actionType == ActionType.ShowAnimation ||

//                actionType == ActionType.ShowComposite)

//            {

//                if (Std_Ops.Limit_Action.getFilename() == string.Empty)

//                {

//                    errorMsg = "Over Limit Dislay Page is not set !";

//                    return false;

//                }

//            }



//            actionType = (ActionType)Std_Ops.Alert_Action.primary;



//            if (actionType == ActionType.ShowText ||

//                actionType == ActionType.ShowGraphics ||

//                actionType == ActionType.ShowAnimation ||

//                actionType == ActionType.ShowComposite)

//            {

//                if (Std_Ops.Alert_Action.getFilename() == string.Empty)

//                {

//                    errorMsg = "Alert Display Page is not set !";

//                    return false;

//                }

//            }



//            if (Min_Speed_Threshold >= Max_Speed_Threshold)

//            {

//                errorMsg = "Max Limit should be set higher than Min Limit.";

//                return false;

//            }



//            if (Std_Ops.Alert_Speed <= Std_Ops.Speed_Limit)

//            {

//                errorMsg = "Alert Speed should be set higher than Speed Limit!";

//                return false;

//            }



//            if (Std_Ops.Speed_Limit < Min_Speed_Threshold ||

//                Std_Ops.Speed_Limit > Max_Speed_Threshold)

//            {

//                errorMsg = "Speed Limit should be set within range of " +

//                                Min_Speed_Threshold + " ~ " +

//                                Max_Speed_Threshold + ".";

//                return false;

//            }



//            if (Std_Ops.Alert_Speed < Min_Speed_Threshold ||

//                Std_Ops.Alert_Speed > Max_Speed_Threshold)

//            {

//                errorMsg = "Alert Limit should be set within range of " +

//                                Min_Speed_Threshold + " ~ " +

//                                Max_Speed_Threshold + ".";

//                return false;

//            }





//            // If GPIP Output port is used in action,

//            // we need to check if the port is present and enabled



//            // public enum Alert_Type_Enum_t

//            //{

//            //    None = 0,

//            //    Blink_Display,

//            //    Strobes,

//            //    Blink_and_Strobes,

//            //    Camera_Trigger,

//            //    GPIO_Out_1,

//            //    GPIO_Out_2,

//            //    GPIO_Out_3,

//            //    GPIO_Out_4

//            //};



//            int idx, i;

//            Alert_Type_Enum_t type = (Alert_Type_Enum_t)Std_Ops.Limit_Action.alert;



//            if (type >= Alert_Type_Enum_t.GPIO_Out_1)

//            {

//                idx = type - Alert_Type_Enum_t.GPIO_Out_1;



//                if ((GPIO_Out[idx].Flags & 0x01) == 0)

//                {

//                    errorMsg = "Limit Display Alert Action: GPIO Output Port " + (idx + 1) +

//                                " is not enabled. Use GPIO Tab to enable the port first!";

//                    return false;

//                }

//            }



//            type = (Alert_Type_Enum_t)Std_Ops.Alert_Action.alert;

//            if (type >= Alert_Type_Enum_t.GPIO_Out_1)

//            {

//                idx = type - Alert_Type_Enum_t.GPIO_Out_1;



//                if ((GPIO_Out[idx].Flags & 0x01) == 0)

//                {

//                    errorMsg = "Alert Display Alert Action: GPIO Output Port " + (idx + 1) +

//                                " is not enabled. Use GPIO Tab to enable the port first!";

//                    return false;

//                }

//            }



//            // Check GPIO Input entries

//            for (i = 0; i < 4; i++)

//            {

//                // We only check if the inpurt port is enabled

//                if ((GPIO_In[i].Flags & 0x01) == 0)

//                    continue;



//                type = (Alert_Type_Enum_t)GPIO_In[i].Action.alert;



//                if (type >= Alert_Type_Enum_t.GPIO_Out_1)

//                {

//                    idx = type - Alert_Type_Enum_t.GPIO_Out_1;



//                    if ((GPIO_Out[idx].Flags & 0x01) == 0)

//                    {

//                        errorMsg = "GPIO Port " + (i + 1) + " Action: " + "GPIO Port " + (idx + 1) +

//                                   " is not enabled.\nUse GPIO Tab to enable the port first!";

//                        return false;

//                    }

//                }

//            }



//            return true;



//        }



//        public Boolean isExternalRadarSupported()

//        {

//            // PENDING888: isExternalRadarSupported, REMOVED



//            if (Version_Major > 0xFF000013)

//                return true;



//            if (Version_Major == 0xFF000013 &&

//                Version_Incremental >= 6)

//                return true;



//            return false;

//        }



//        static public byte[] getDefaultData()

//        {

//            EEPROM_Param_Data_t map = new EEPROM_Param_Data_t();



//            return map.encode();

//        }

//    };



//    public class PageInfo

//    {

//        public byte hashLSB = 0;

//        public byte hashMSB = 0;

//        public string filename = string.Empty;

//        public byte[] content = null;



//        public byte userData = 0;



//        public Boolean isReferenced = false;



//        public PageInfo() { }

//        public PageInfo(string filenameIn, byte hashLSBIn, byte hashMSBIn)

//        {

//            filename = filenameIn;

//            hashLSB = hashLSBIn;

//            hashMSB = hashMSBIn;

//        }



//        public string toString()

//        {

//            return filename + ", Hash=" + hashMSB.ToString("X2") + hashLSB.ToString("X2") + "," +

//                             GetHashValue();

//        }



//        public Boolean isSameHash(UInt16 hashValue)

//        {

//            byte hashLSBIn = (byte)(hashValue & 0xFF);

//            byte hashMSBIn = (byte)((hashValue >> 8) & 0xFF);



//            //

//            // PMG side will flip these two bytes. We try both cases

//            //

//            if (((hashLSBIn == hashLSB) && (hashMSBIn == hashMSB)) |

//                ((hashLSBIn == hashMSB) && (hashMSBIn == hashLSB)))

//                return true;

//            else

//                return false;

//        }



//        public UInt16 GetHashValue()

//        {

//            return (UInt16)((hashMSB << 8) + (byte)(hashLSB & 0xFF));

//        }



//        public void SetHashValue(UInt16 value)

//        {

//            hashLSB = (byte)(value & 0xFF);

//            hashMSB = (byte)((value >> 8) & 0xFF);

//        }



//        //public Boolean IsScheduledOperation()

//        //{

//        //    if (filename.Contains(".S"))

//        //        return true;

//        //    else

//        //        return false;



//        //}



//        public PageType GetPageType()

//        {

//            if (filename.Contains(".S"))

//                return PageType.Sequence;

//            else if (filename.Contains(".T"))

//                return PageType.Text;

//            else if (filename.Contains(".A"))

//                return PageType.Animation;

//            else if (filename.Contains(".G"))

//                return PageType.Graphic;

//            else if (filename.Contains(".J"))

//                return PageType.Calendar;

//            else

//                return PageType.Unknown;

//        }



//    };



//    public class ACFormatPacketProtocol

//    {

//        public static byte SeqNo = 1;

//        public enum ReadWriteType

//        {

//            Write = 0,

//            Read = 1

//        };



//        /***** Message Format Definition

//            Start LSB                   (0x41)

//            Start MSB                   (0x43)

//            Command

//            Target Subsystem & R/W bit

//            Source Subsystem

//            Sequence Number

//            Packet Number

//            Total Number of Packets

//            Packet Length  



//            First data byte  -----

//            …                Actual Payload 

//            Data byte n      -----

//            Padding



//            Checksum LSB

//            Checksum MSB



//            Note 1: 9 Bytes before actual payload data

//            Note 2: Last 2 bytes for checksum

//            Note 3: So, we have fixed 11 bytes, so, the payload data need to be odd length.

//                    Otherwise, we will pad one zero (add one byte to packet length) 

//                    before checksum is calculated

                    

//        ***/



//        public static byte[] FormatMessage(PMGCommandID commandID, byte[] packetData,

//                                           ReadWriteType type = ReadWriteType.Write,

//                                           byte targetSubsystemID = (byte)SubSystem_ID_t.SubSys_Controller,

//                                           byte sourceSubsystemID = (byte)SubSystem_ID_t.SubSys_Application,

//                                           byte seqNo = 0, byte packetNum = 1, byte totalPacketNumber = 1,

//                                           Boolean isPMG = false)



//        {

//            //

//            // First, decide the message length and whether we should pad 0

//            // Header Part (10 bytes) + Payload Data + CS (2 bytes)

//            // 

//            int msgLen;

//            int payloadDataLen;



//            if (seqNo == 0)

//            {

//                seqNo = SeqNo++;

//            }



//            if (packetData != null)

//            {

//                msgLen = 10 + packetData.Length + 2;

//                payloadDataLen = packetData.Length;

//            }

//            else

//            {

//                msgLen = 10 + 0 + 2;

//                payloadDataLen = 0;

//            }



//            // Pad 0 if needed

//            if (msgLen % 2 != 0)

//            {

//                payloadDataLen++;

//                msgLen += 1;

//            }



//            byte[] msgData = new byte[msgLen];



//            //

//            // Encode header portion (9 bytes)

//            //

//            msgData[0] = 0x41;

//            msgData[1] = 0x43;

//            msgData[2] = (byte)commandID;



//            msgData[3] = targetSubsystemID;

//            if (type == ReadWriteType.Read)

//                msgData[3] += 0x80;



//            msgData[4] = sourceSubsystemID; // Source subsystem (Decimal 102)



//            msgData[5] = seqNo;             // Seq Number

//            msgData[6] = packetNum;         // Packet Number

//            msgData[7] = totalPacketNumber; // Total Number Of Packets



//            msgData[8] = (byte)payloadDataLen;        // Payload Length LSB

//            msgData[9] = (byte)(payloadDataLen >> 8); // Payload Length MSB



//            // Copy actual payload data (leave pad 0 unchanged)

//            if (payloadDataLen > 0)

//            {

//                Buffer.BlockCopy(packetData, 0, msgData, 10, packetData.Length);

//            }



//            // Calculate Checksum

//            uint16_t cs = Util.U16ComputeCRC(msgData, 0, msgLen - 2);



//            if (!isPMG)

//            {

//                msgData[msgLen - 2] = (byte)(cs & 0xFF);

//                msgData[msgLen - 1] = (byte)((cs >> 8) & 0xFF);

//            }

//            else

//            {

//                msgData[msgLen - 1] = (byte)(cs & 0xFF);

//                msgData[msgLen - 2] = (byte)((cs >> 8) & 0xFF);

//            }



//            return msgData;

//        }



//        //

//        // Formating Web Notification message sent to Server

//        // key is the 20 bytes IMSI number for all PMG related operation

//        // key is username for account related operation

//        //

//        public static byte[] FormatMessage(TableID tableId, NotificationType type,

//                                           byte[] key)

//        {

//            // First, decide the message length and whether we should pad 0

//            // Header Part (10 bytes) + TableId (1 byte) + Type (1 byte) + Key + CS (2 bytes)

//            // 

//            int msgLen;

//            int payloadDataLen;



//            msgLen = 10 + 2 + key.Length + 2;

//            payloadDataLen = 2 + key.Length;



//            // Pad 0 if needed

//            if (msgLen % 2 != 0)

//            {

//                payloadDataLen++;

//                msgLen += 1;

//            }



//            byte[] msgData = new byte[msgLen];



//            //

//            // Encode header portion (10 bytes)

//            //

//            msgData[0] = 0x41;

//            msgData[1] = 0x43;

//            msgData[2] = (byte)PMGCommandID.Notification;

//            msgData[3] = 0;

//            msgData[4] = 0;

//            msgData[5] = 0;

//            msgData[6] = 1;     // Packet Number

//            msgData[7] = 1;     // Total Number Of Packets



//            msgData[8] = (byte)payloadDataLen;        // Payload Length LSB

//            msgData[9] = (byte)(payloadDataLen >> 8); // Payload Length MSB



//            msgData[10] = (byte)tableId;

//            msgData[11] = (byte)type;



//            Buffer.BlockCopy(key, 0, msgData, 12, key.Length);



//            // Calculate Checksum

//            uint16_t cs = Util.U16ComputeCRC(msgData, 0, msgLen - 2);



//            msgData[msgLen - 1] = (byte)(cs & 0xFF);

//            msgData[msgLen - 2] = (byte)((cs >> 8) & 0xFF);



//            return msgData;

//        }

//    }



//    public class PayloadData

//    {

//        public byte[] data = null;

//        public PayloadData(byte[] dataIn)

//        {

//            data = dataIn;

//        }

//    }



//    public class ParamConfigureEntry

//    {

//        public int pmgId = 0;

//        public ParamaterId paramId = 0;

//        public string value = string.Empty;

//        public byte state = 0;



//        public ParamConfigureEntry() { }

//        public ParamConfigureEntry(ParamaterId paramIdIn, string valueIn, int pmgIdIn = 0, byte stateIn = 0)

//        {

//            paramId = paramIdIn;

//            value = valueIn;

//            pmgId = pmgIdIn;

//            state = stateIn;

//        }



//        public ParamConfigureEntry(ParamaterId paramIdIn, uint valueIn, int pmgIdIn = 0, byte stateIn = 0)

//        {

//            paramId = paramIdIn;

//            value = valueIn.ToString();

//            pmgId = pmgIdIn;

//            state = stateIn;

//        }



//        public uint getUintValue()

//        {

//            uint valueOut = 0;



//            uint.TryParse(value, out valueOut);

//            return valueOut;

//        }

//        public string toString()

//        {

//            return paramId.ToString() + "= [" + value + "], state=" + state + ", pmgid=" + pmgId;

//        }

//    }



//    public class PMGMessage

//    {

//        #region

//        public enum InternalMsgType

//        {

//            ConfigurePMG = 0x01, ConfigurePMGClock = 0x02

//        };

//        public Boolean isInternalMessage = false;

//        public InternalMsgType internalMsgType = InternalMsgType.ConfigurePMG;



//        public DateTime timestamp = DateTime.Now;

//        public IPEndPoint remoteEndPoint;



//        private static PMGMessage[] msgList = new PMGMessage[10];



//        public byte[] data;



//        public const int PAYLOAD_START_BYTE_IDX = 10;

//        public const int PAYLOAD_LENGH_IDX = 8;



//        // Used in Registration message

//        public const int PAYLOAD_PMG_TYPE_IDX = 22;



//        public int pmdId = 0;



//        public string errorMsg;



//        public PMGMessage(byte[] dataIn)

//        {

//            data = dataIn;

//        }



//        public PMGMessage(InternalMsgType msgType, int pmdIdIn)

//        {

//            isInternalMessage = true;

//            internalMsgType = msgType;

//            pmdId = pmdIdIn;

//        }



//        //

//        // We only check possible message types sent from

//        // PMG

//        //

//        public Boolean IsValidMessage(ref PMGErrorCode errorCode)

//        {

//            //

//            // First do CRC check

//            //

//            uint16_t cs = Util.U16ComputeCRC(data, 0, data.Length - 2);

//            byte cs1 = (byte)(cs & 0xFF);

//            byte cs2 = (byte)((cs >> 8) & 0xFF);



//            //

//            // Real PMG will swap these two bytes

//            //

//            if (!((data[data.Length - 2] == cs2) && (data[data.Length - 1] == cs1)) &&

//                !((data[data.Length - 2] == cs1) && (data[data.Length - 1] == cs2)))

//            {

//                errorMsg = "CRC Mismatched! Recv " +

//                                    Utils.ByteArrayToHexString(data, data.Length - 2, 2, false) +

//                                  ", Expecting: " + cs2.ToString("X2") + " " + cs1.ToString("X2");

//                errorCode = PMGErrorCode.CRC_Mismatch;

//                return false;

//            }



//            if (GetCommandID() == PMGCommandID.Registration)

//            {

//                if (GetPayloadLength() >= 21)

//                    return true;

//                else

//                {

//                    errorCode = PMGErrorCode.Wrong_Message_Format;

//                    return false;

//                }

//            }

//            else if (GetCommandID() == PMGCommandID.WriteParameter)

//            {

//                //

//                // Check message size

//                //



//                //int len = 12;



//                //if (!IsReadCommand())

//                //    len = 10 + GlobalConstantDefintion.PARAM_MAP_SIZE + 2;



//                //if (data.Length != len)

//                //{

//                //    errorCode = PMGErrorCode.Wrong_Message_Format;

//                //    return false;

//                //}

//            }



//            return true;

//        }



//        public Boolean IsReadCommand()

//        {

//            if ((data[3] & 0x80) != 0)

//                return true;

//            else

//                return false;

//        }



//        public ACFormatPacketProtocol.ReadWriteType GetReadWriteType()

//        {

//            if (IsReadCommand())

//                return ACFormatPacketProtocol.ReadWriteType.Read;

//            else

//                return ACFormatPacketProtocol.ReadWriteType.Write;

//        }





//        public PMGCommandID GetCommandID()

//        {

//            if (data != null)

//                return (PMGCommandID)data[2];

//            else

//                return PMGCommandID.Unknown;

//        }



//        public Boolean IsAckMessage(ref byte cmdIdBeingAcked, ref byte seqNoBeingAcked)

//        {

//            PMGCommandID commandId = GetCommandID();



//            if (commandId == PMGCommandID.Ack || commandId == PMGCommandID.GeneralAck)

//            {

//                cmdIdBeingAcked = data[10];

//                seqNoBeingAcked = data[11];



//                return true;

//            }

//            else

//                return false;

//        }



//        public Boolean IsAckMessage()

//        {

//            PMGCommandID commandId = GetCommandID();



//            if (commandId == PMGCommandID.Ack || commandId == PMGCommandID.GeneralAck)

//                return true;

//            else

//                return false;

//        }



//        public int GetAckStatusCode()

//        {

//            PMGCommandID commandId = GetCommandID();



//            if (commandId == PMGCommandID.Ack)

//            {

//                //

//                // This is to fix firmware 1.7.0.0 problem

//                // 0x01 is not a valid value

//                //

//                if (data[13] == 0x01)

//                    data[13] = 0;



//                return ((data[12] << 8) + data[13]);

//            }

//            else

//            {

//                return 0;

//            }

//        }



//        public string GetAckStatusCodeHexString()

//        {

//            PMGCommandID commandId = GetCommandID();



//            if (commandId == PMGCommandID.Ack)

//            {

//                if (data.Length >= 14)

//                    return data[12].ToString("X2") + data[13].ToString("X2");

//                else

//                    return string.Empty;

//            }

//            else

//            {

//                return string.Empty;

//            }

//        }



//        public PMGCommandID GetWhichCommandToAck()

//        {

//            PMGCommandID commandId = GetCommandID();



//            if (commandId == PMGCommandID.Ack || commandId == PMGCommandID.GeneralAck)

//            {

//                if (data.Length >= 13)

//                    return (PMGCommandID)data[10];

//                else

//                    return 0;

//            }

//            else

//            {

//                return 0;

//            }

//        }

//        public byte GetPacketNumber()

//        {

//            if (data != null)

//                return data[6];

//            else

//                return 0;

//        }



//        public byte GetTotalPacketNumber()

//        {

//            if (data != null)

//                return data[7];

//            else

//                return 0;

//        }



//        public string GetDescription()

//        {

//            PMGCommandID commandId = GetCommandID();

//            string statsCommandType = string.Empty;



//            if (commandId != PMGCommandID.Unknown)

//            {

//                string s;



//                if (commandId == PMGCommandID.StatsRecordCommand)

//                    statsCommandType = ", CmdType=" + ((StatsCommandType)data[10]).ToString();



//                if (IsReadCommand())

//                    s = commandId.ToString() + statsCommandType + ", Read, Len=" + data.Length + ", Payload=" + data[8] + ", Seq=" + data[5] +

//                            ", Pkt=" + data[6] + ",TotalPkt=" + data[7];

//                else

//                    s = commandId.ToString() + statsCommandType + ", Write, Len=" + data.Length + ", Payload=" + data[8] + ", Seq=" + data[5] +

//                            ", Pkt=" + data[6] + ",TotalPkt=" + data[7];



//                return s;

//            }

//            else

//            {

//                return "Unknown";

//            }

//        }



//        public string GetRemoteEndpointKey()

//        {

//            if (remoteEndPoint == null)

//                return string.Empty;

//            else

//                return remoteEndPoint.ToString();

//        }

//        public int GetPayloadLength()

//        {

//            int payloadLength = data[PAYLOAD_LENGH_IDX] + (data[PAYLOAD_LENGH_IDX + 1] << 8);

//            return payloadLength;

//        }



//        // Get payload data, index start with 0

//        public byte GetPayloadDataByte(int payloadByteIdx)

//        {

//            //

//            // First make sure the index is valid

//            //

//            if (payloadByteIdx >= GetPayloadLength())

//                return 0;



//            return data[PAYLOAD_START_BYTE_IDX + payloadByteIdx];

//        }



//        // Get payload data, index start with 0

//        public byte[] GetPayloadDataArray(int payloadByteIdx = 0, int len = 0)

//        {

//            if (len == 0)

//                len = GetPayloadLength();



//            //

//            // First make sure the index and len are valid

//            //

//            if (payloadByteIdx + len > GetPayloadLength())

//                return null;



//            byte[] payload = new byte[len];



//            Buffer.BlockCopy(data, PAYLOAD_START_BYTE_IDX + payloadByteIdx, payload, 0, len);



//            return payload;

//        }



//        public byte GetSeqNumber()

//        {

//            if (data.Length <= 6)

//                return 0;



//            return data[5];

//        }



//        //

//        // Processing incoming UDP data and return a list of PMGMessage (up to 10)

//        // Note: One UDP message received, there may be multiple AC packets.

//        // This is due to PMD modem will send data in batch at 50 ms interval.

//        // If bad CRC is detected, we will retrieve the sequence number from the message.

//        // This sequence number will be used to send back general ack.

//        //

//        static public PMGMessage[] GetMessageList(byte[] actualData, ref int count,

//                                                  ref byte badMessageSeqNum,

//                                                  ref byte badMessageCmd,

//                                                  ref PMGErrorCode errorCode)



//        {

//            int idx = 0;



//            /***** Message Format Definition

//              Start LSB                   (0x41)

//              Start MSB                   (0x43)

//              Command

//              Target Subsystem & R/W bit

//              Source Subsystem

//              Sequence Number

//              Packet Number

//              Total Number of Packets

//              Packet Length  



//              First data byte  -----

//              …                Actual Payload 

//              Data byte n      -----

//              Padding



//              Checksum LSB

//              Checksum MSB



//              Note 1: 9 Bytes before actual payload data

//              Note 2: Last 2 bytes for checksum

//              Note 3: So, we have fixed 11 bytes, the payload data need to be odd length.

//                      Otherwise, we will pad one zero (add one byte to packet length) 

//                      before checksum is calculated



//            ***/



//            count = 0;



//            if (actualData == null)

//                return null;



//            while (true)

//            {

//                //

//                // First we check if we have enough data left.

//                // Minminum we should have at least 11 bytes 

//                //    (9 bytes header and 2 bytes checksum)

//                //

//                if (idx + 11 >= actualData.Length)

//                {

//                    break;

//                }



//                //

//                // Now, we check first 9 bytes

//                //

//                if (actualData[idx] != 0x41 || actualData[idx + 1] != 0x43)

//                {

//                    break;

//                }



//                // Check packet payload length

//                int payloadDataLen = actualData[idx + 8] + (actualData[idx + 9] << 8);



//                //

//                // Given the packet length, we now know the start and end of

//                // the message

//                //

//                int padZeroByte = 0;



//                if (payloadDataLen % 2 != 0)

//                    padZeroByte = 1;



//                int msgLen = 10 + payloadDataLen + padZeroByte + 2;





//                // 

//                // Check if we have enough data

//                //

//                if (idx + msgLen > actualData.Length)

//                {

//                    errorCode = PMGErrorCode.Wrong_Message_Format;

//                    badMessageSeqNum = actualData[idx + 5];

//                    badMessageCmd = actualData[idx + 2];



//                    break;

//                }



//                //// 

//                //// Last 2 bytes are checksum, we do the checking first here

//                ////

//                //if (Util.IsU16CheckSumValid(actualData, idx, msgLen))

//                //{



//                byte[] packetData = new byte[msgLen];



//                Buffer.BlockCopy(actualData, idx, packetData, 0, msgLen);



//                PMGMessage msg = new PMGMessage(packetData);



//                if (msg.IsValidMessage(ref errorCode))

//                {

//                    msgList[count] = new PMGMessage(packetData);

//                    count++;

//                }

//                else

//                {

//                    errorCode = PMGErrorCode.CRC_Mismatch;

//                    badMessageSeqNum = actualData[idx + 5];

//                    badMessageCmd = actualData[idx + 2];

//                    break;

//                }



//                idx += msgLen;

//            }



//            return msgList;

//        }

//        #endregion

//    }



//    public class PMGSystemInfo

//    {

//        public class GPIOPortInfo

//        {

//            public enum PortType

//            {

//                Input = 0,

//                Output,

//                Power3Dot3V,

//                Power12V

//            };



//            public PortType portType;

//            public int portNumber;



//            public GPIOPortInfo(PortType portTypeIn, int portNumberIn)

//            {

//                portType = portTypeIn;

//                portNumber = portNumberIn;

//            }

//        };



//        public class OptData

//        {

//            public const int OptDataBlockSize = 3 + 3 + 8 + 4 + 4;



//            public uint16_t moduleType = 0;

//            public byte moduleSubType;

//            public string hardwareVersion = string.Empty;

//            public string serialNumber = string.Empty;

//            public string firmwareVersion = string.Empty;

//            public string moduleLibFirmwareVersion = string.Empty;

//            public OptData() { }



//            public string getModuleName()

//            {

//                if (moduleType == (int)ModuleTypeCode.RadarProcessor)

//                {

//                    return "Radar Processor";

//                }

//                else if (moduleType == (int)ModuleTypeCode.TwelveInchDisplay)

//                {

//                    if (moduleSubType == 0)

//                        return "12” Display W/ Flashers";

//                    else if (moduleSubType == 1)

//                        return "12” Display";

//                    else if (moduleSubType == 2)

//                        return "12” Display W/ Flashers";

//                }

//                else if (moduleType == (int)ModuleTypeCode.MTTP)

//                {

//                    if (moduleSubType == 0)

//                        return "MPPT Lead Acid Module";

//                    else if (moduleSubType == 1)

//                        return "MPPT Lithium Ion Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.Cellular)

//                {

//                    if (moduleSubType == 0)

//                        return "3G Wireless Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.Bluetooth)

//                {

//                    if (moduleSubType == 0)

//                        return "PAN Wireless Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.Wifi)

//                {

//                    if (moduleSubType == 0)

//                        return "WIFI Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.Wireless433)

//                {

//                    if (moduleSubType == 0)

//                        return "433 Wireless Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.StormWarning)

//                {

//                    if (moduleSubType == 0)

//                        return "Storm Warning Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.EmergencyAlert)

//                {

//                    if (moduleSubType == 0)

//                        return "EAS Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.GPS)

//                {

//                    if (moduleSubType == 0)

//                        return "GPS Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.Ethernet)

//                {

//                    if (moduleSubType == 0)

//                        return "Ethernet Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.MeshRadio)

//                {

//                    if (moduleSubType == 0)

//                        return "Mesh Radio Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.GPIO)

//                {

//                    if (moduleSubType == 0)

//                        return "GPIO Module 4I0O";

//                    else if (moduleSubType == 1)

//                        return "GPIO Module 3I1O";

//                    else if (moduleSubType == 2)

//                        return "GPIO Module 2I2O";

//                    else if (moduleSubType == 3)

//                        return "GPIO Module 1I3O";

//                    else if (moduleSubType == 4)

//                        return "GPIO Module 0I4O";

//                    else if (moduleSubType == 5)

//                        return "GPIO Module 3I0O, 3.3V";

//                    else if (moduleSubType == 6)

//                        return "GPIO Module 2I1O, 3.3V";

//                    else if (moduleSubType == 7)

//                        return "GPIO Module 1I2O, 3.3V";

//                    else if (moduleSubType == 8)

//                        return "GPIO Module 0I3O, 3.3V";

//                    else if (moduleSubType == 9)

//                        return "GPIO Module 3I0O, 12V";

//                    else if (moduleSubType == 10)

//                        return "GPIO Module 2I1O, 12V";

//                    else if (moduleSubType == 11)

//                        return "GPIO Module 1I2O, 12V";

//                    else if (moduleSubType == 12)

//                        return "GPIO Module 0I3O, 12V";

//                    else

//                        return "GPIO Module, Subtype " + moduleSubType;

//                }

//                else if (moduleType == (int)ModuleTypeCode.SmallLionBattery)

//                {

//                    if (moduleSubType == 0)

//                        return "Small Lion Charger";

//                    else if (moduleSubType == 1)

//                        return "Small Lion Interface";

//                }

//                else if (moduleType == (int)ModuleTypeCode.FiberOptic)

//                {

//                    if (moduleSubType == 0)

//                        return "Fiber Optic Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.BatteryBackup)

//                {

//                    if (moduleSubType == 0)

//                        return "Battery Backup Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.FifteenInchDisplay)

//                {

//                    if (moduleSubType == 0)

//                        return "15” Display W/ Flashers";

//                    else if (moduleSubType == 1)

//                        return "15” Display";

//                    else if (moduleSubType == 2)

//                        return "15” Display W/ Flashers";



//                }

//                else if (moduleType == (int)ModuleTypeCode.EighteenInchDisplay)

//                {

//                    if (moduleSubType == 0)

//                        return "18” Display W/ Flashers";

//                    else if (moduleSubType == 1)

//                        return "18” Display";

//                    else if (moduleSubType == 2)

//                        return "18” Display W/ Flashers";

//                }

//                else if (moduleType == (int)ModuleTypeCode.LargeLionBattery)

//                {

//                    if (moduleSubType == 0)

//                        return "Large Lion Battery Module";

//                    else if (moduleSubType == 1)

//                        return "Large NiMH 22 Battery Module";

//                    else if (moduleSubType == 2)

//                        return "Large NiMH 34 Battery Module";

//                    else if (moduleSubType == 3)

//                        return "Large NiMH 45 Battery Module";

//                    else if (moduleSubType == 4)

//                        return "Large NiMH 50 Battery Module";

//                }

//                else if (moduleType == (int)ModuleTypeCode.SlotControlPCB)

//                {

//                    if (moduleSubType == 0)

//                        return "SLOT 1~7 or IO Test of controller test PCB 5571-00";

//                    else if (moduleSubType == 0x20)

//                        return "SLOT of controller test PCB 5571-20";

//                    else if (moduleSubType == 0x40)

//                        return "SLOT of controller test PCB 5571-40";

//                }

//                else if (moduleType == (int)ModuleTypeCode.SimulatedFlash)

//                {

//                    return "Simulated Flash";

//                }

//                else if (moduleType == (int)ModuleTypeCode.TrafficCamera)

//                {

//                    return "Traffic Camera";

//                }

//                else if (moduleType == (int)ModuleTypeCode.PAN_Radio)

//                {

//                    return "PAN Radio";

//                }

//                else if (moduleType == (int)ModuleTypeCode.LTE4G)

//                {

//                    return "4G LTE";

//                }

//                else if (moduleType == (int)ModuleTypeCode.Controller12_Inch)

//                {

//                    return "12” Controller";

//                }

//                else if (moduleType == (int)ModuleTypeCode.Controller15_Inch)

//                {

//                    return "15” Controller";

//                }

//                else if (moduleType == (int)ModuleTypeCode.Controller18_Inch)

//                {

//                    return "18” Controller";

//                }

//                else if (moduleType == (int)ModuleTypeCode.TextAndGraphic)

//                {

//                    if (moduleSubType == 1)

//                        return "Text and Graphics";

//                }

//                else if (moduleType == (int)ModuleTypeCode.InternalCharger)

//                {

//                    return "Internal Charger";

//                }



//                return moduleType + "-" + moduleSubType;

//            }



//            public Boolean isMPPTModule()

//            {

//                if (moduleType == (int)ModuleTypeCode.MTTP)

//                    return true;

//                else

//                    return false;

//            }



//            public Boolean isGPIOModule()

//            {

//                if (moduleType == (int)ModuleTypeCode.GPIO)

//                    return true;

//                else

//                    return false;

//            }



//            public Boolean isEthernetModule()

//            {

//                if (moduleType == (int)ModuleTypeCode.Ethernet)

//                    return true;

//                else

//                    return false;

//            }



//            public Boolean isWifiModule()

//            {

//                if (moduleType == (int)ModuleTypeCode.Wifi)

//                    return true;

//                else

//                    return false;

//            }



//            public Boolean isLargeLionBatteryModule()

//            {

//                if (moduleType == (int)ModuleTypeCode.LargeLionBattery)

//                    return true;

//                else

//                    return false;

//            }



//            public Boolean isInternalChargerModule()

//            {

//                if (moduleType == (int)ModuleTypeCode.InternalCharger)

//                    return true;

//                else

//                    return false;

//            }



//            public Boolean isSmallLionBatteryModule()

//            {

//                if (moduleType == (int)ModuleTypeCode.SmallLionBattery)

//                    return true;

//                else

//                    return false;

//            }



//            public Boolean isTextGraphicModule()

//            {

//                if (moduleType == (int)ModuleTypeCode.TextAndGraphic &&

//                    moduleSubType == 1)

//                    return true;

//                else

//                    return false;

//            }



//            public string toString(string indent)

//            {

//                string s = string.Empty;



//                s += (indent + "ModuleType=" + moduleType + Environment.NewLine);

//                s += (indent + "ModuleSubType=" + moduleSubType + Environment.NewLine);

//                s += (indent + "HardwareVersion=" + hardwareVersion + Environment.NewLine);

//                s += (indent + "SerialNumber=" + serialNumber + Environment.NewLine);

//                s += (indent + "FirmwareVersin=" + firmwareVersion + Environment.NewLine);

//                s += (indent + "ModuleLibFirmwareVersion=" + moduleLibFirmwareVersion + Environment.NewLine);



//                return s;

//            }

//        };



//        public uint16_t modelNumber = 0;

//        public string hardwareVersion;

//        public string serialNumber;

//        public string firmwareVersin;



//        public List<OptData> optDataList = new List<OptData>();



//        public PMGSystemInfo() { }



//        public Boolean decode(byte[] data)

//        {

//            int idx = 0;



//            modelNumber = (uint16_t)(data[idx] + (data[idx + 1] << 8)); idx += 2;

//            hardwareVersion = Util.GetAsciiSubString(data, idx, 3); idx += 3;

//            serialNumber = Util.GetAsciiSubString(data, idx, 8); idx += 8;

//            firmwareVersin = data[idx] + "-" + data[idx + 1] + "-" +

//                             data[idx + 2] + "-" + data[idx + 3]; idx += 4;



//            int numOptData = data[idx]; idx += 1;

//            Boolean optDecodeErrorDetected = false;



//            for (int i = 0; i < numOptData; i++)

//            {

//                if (idx + OptData.OptDataBlockSize > data.Length)

//                {

//                    optDecodeErrorDetected = true;

//                    break;

//                }



//                OptData optData = new OptData();



//                optData.moduleType = (uint16_t)(data[idx] + (data[idx + 1] << 8)); idx += 2;

//                optData.moduleSubType = data[idx]; idx += 1;

//                optData.hardwareVersion = Util.GetAsciiSubString(data, idx, 3); idx += 3;

//                optData.serialNumber = Util.GetAsciiSubString(data, idx, 8); idx += 8;



//                if (optData.serialNumber.Equals("None", StringComparison.OrdinalIgnoreCase))

//                    optData.serialNumber = string.Empty;





//                //

//                // From Mike, if all fields are 0xFF, blank version field

//                //

//                if (data[idx] == 0xFF && data[idx + 1] == 0xFF &&

//                data[idx + 2] == 0xFF && data[idx + 3] == 0xFF)

//                    optData.firmwareVersion = string.Empty;

//                else

//                {

//                    //optData.firmwareVersin = data[idx].ToString().PadLeft(2, '0') + "-" + 

//                    //                        data[idx + 1].ToString().PadLeft(2, '0') + "-" +

//                    //                        data[idx + 2].ToString().PadLeft(2, '0') + "-" + 

//                    //                        data[idx + 3].ToString().PadLeft(2, '0');



//                    optData.firmwareVersion = data[idx].ToString() + "." +

//                                              data[idx + 1].ToString() + "." +

//                                              data[idx + 2].ToString() + "." +

//                                              data[idx + 3].ToString();

//                }



//                idx += 4;



//                optData.moduleLibFirmwareVersion = string.Empty;



//                for (int j = idx; j <= idx + 3; j++)

//                {

//                    if (data[j] != 0xFF)

//                    {

//                        if (optData.moduleLibFirmwareVersion != string.Empty)

//                            optData.moduleLibFirmwareVersion += ("." + data[j]);

//                        else

//                            optData.moduleLibFirmwareVersion = data[j].ToString();

//                    }

//                }



//                idx += 4;



//                optDataList.Add(optData);

//            }



//            //

//            // PENDING828: getModuleData. Add Test GPIO Module

//            //

//            //if (!hasGPIOModule())

//            //{

//            //    OptData grpoData = new OptData();



//            //    grpoData.moduleType = (int)ModuleTypeCode.GPIO;



//            //    grpoData.moduleSubType = 0x09;  // 3 Out



//            //    // grpoData.moduleSubType = 0x02; // 2 In, 2 Out



//            //    //grpoData.moduleSubType = 0x0A; // 1 In, 2 Out



//            //    grpoData.serialNumber = "TEST";

//            //    grpoData.hardwareVersion = "TEST";

//            //    grpoData.firmwareVersin = "TEST";

//            //    grpoData.moduleLibFirmwareVersion = "TEST";



//            //    optDataList.Add(grpoData);

//            //}



//            if (optDecodeErrorDetected)

//                return false;

//            else

//                return true;

//        }



//        public string getModelName()

//        {

//            //

//            // Per request from Devin, we change to following names

//            // 

//            if (modelNumber == 5543)

//                return "PMG-18";

//            else if (modelNumber == 5542)

//                return "PMG-15";

//            else

//                return "PMG-12";



//            //if (modelNumber == 5543)

//            //    return modelNumber + ", 18\" Controller";

//            //else if (modelNumber == 5542)

//            //    return modelNumber + ", 15\" Controller";

//            //else

//            //    return modelNumber + ", 12\" Controller";

//        }



//        public UInt32 getFirmwareVersion()

//        {

//            // PENDING888: getFirmwareVersion. Simulation, REMOVED

//            // return GlobalConstantDefintion.FIRMWARE_SUPPORT_COMPOSITE_PAGE;



//            for (int i = 0; i < optDataList.Count; i++)

//            {

//                if (optDataList[i].moduleType == (int)ModuleTypeCode.Controller12_Inch ||

//                    optDataList[i].moduleType == (int)ModuleTypeCode.Controller15_Inch ||

//                    optDataList[i].moduleType == (int)ModuleTypeCode.Controller18_Inch)

//                {

//                    if (optDataList[i].firmwareVersion == string.Empty)

//                        return 0;



//                    string[] versionBytes;

//                    uint num = 0;



//                    versionBytes = optDataList[i].firmwareVersion.Split('.');



//                    if (versionBytes.Length == 4)

//                    {

//                        for (int j = 0; j < 4; j++)

//                        {

//                            byte data = (byte)int.Parse(versionBytes[j]);



//                            num += (uint)(data << (24 - j * 8));

//                        }

//                    }



//                    return num;

//                }

//            }



//            return 0;

//        }



//        public string getFirmwareVersionString()

//        {



//            for (int i = 0; i < optDataList.Count; i++)

//            {

//                if (optDataList[i].moduleType == (int)ModuleTypeCode.Controller12_Inch ||

//                    optDataList[i].moduleType == (int)ModuleTypeCode.Controller15_Inch ||

//                    optDataList[i].moduleType == (int)ModuleTypeCode.Controller18_Inch)

//                {

//                    return optDataList[i].firmwareVersion;

//                }

//            }



//            return string.Empty;

//        }



//        public PMDDisplaySize getPanelSize()

//        {

//            //

//            // For Panel Size Simulation Purpose. This portion of code 

//            // will not be executed in normal case

//            //

//            //if (LogForm.IsLogWindowVisible())

//            //{

//            //    int panelIdx = LogForm.GetLogger().comboBoxPanelSize.SelectedIndex;



//            //    if (panelIdx == 1)

//            //        return PMDDisplaySize.TwelveInchPMD;

//            //    else if (panelIdx == 2)

//            //        return PMDDisplaySize.FifteenInchPMD;

//            //    else if (panelIdx == 3)

//            //        return PMDDisplaySize.EighteenInchPMD;

//            //}



//            if (modelNumber == 5543)

//                return PMDDisplaySize.EighteenInchPMD;

//            else if (modelNumber == 5542)

//                return PMDDisplaySize.FifteenInchPMD;

//            else

//                return PMDDisplaySize.TwelveInchPMD;

//        }



//        public OptData getModuleData(uint16_t moduleType)

//        {

//            for (int i = 0; i < optDataList.Count; i++)

//            {

//                if (optDataList[i].moduleType == moduleType)

//                    return optDataList[i];

//            }



//            return null;

//        }



//        public Boolean hasPowerModule()

//        {

//            // PENDING888: hasPowerModule, REMOVED

//            // return true;



//            for (int i = 0; i < optDataList.Count; i++)

//            {

//                if (optDataList[i].isMPPTModule() ||

//                    optDataList[i].isLargeLionBatteryModule() ||

//                    optDataList[i].isInternalChargerModule())

//                    return true;

//            }



//            return false;

//        }



//        public Boolean hasMTTPModule()

//        {

//            // PENDING888: hasMTTPModule REMOVED

//            // return true;



//            for (int i = 0; i < optDataList.Count; i++)

//            {

//                if (optDataList[i].isMPPTModule())

//                    return true;

//            }

//            return false;

//        }



//        public Boolean hasInternalBatteryCharger()

//        {

//            // PENDING888: hasInternalBatteryCharger, REMOVED

//            // return true;



//            for (int i = 0; i < optDataList.Count; i++)

//            {

//                if (optDataList[i].isLargeLionBatteryModule())

//                    return true;

//            }

//            return false;

//        }

//        public Boolean hasGPIOModule()

//        {

//            // PENDING888: hasGPIOModule, REMOVED

//            // return true;



//            for (int i = 0; i < optDataList.Count; i++)

//            {

//                if (optDataList[i].isGPIOModule())

//                    return true;

//            }



//            return false;

//        }



//        public Boolean hasEthernetModule()

//        {

//            // PENDING888 hasEthernetModule, REMOVED

//            // return true;



//            for (int i = 0; i < optDataList.Count; i++)

//            {

//                if (optDataList[i].isEthernetModule())

//                    return true;

//            }



//            return false;

//        }



//        public Boolean hasWifiModule()

//        {

//            // PENDING888 hasWifiModule, REMOVED

//            // return true;



//            for (int i = 0; i < optDataList.Count; i++)

//            {

//                if (optDataList[i].isWifiModule())

//                    return true;

//            }



//            return false;

//        }



//        public Boolean hasTextGraphicModule()

//        {

//            // PENDING888: hasTextGraphicModule, REMOVED

//            // return false;



//            for (int i = 0; i < optDataList.Count; i++)

//            {

//                if (optDataList[i].isTextGraphicModule())

//                    return true;

//            }



//            return false;

//        }



//        public GPIOPortInfo[] getGPIOPortInfoList()

//        {

//            List<GPIOPortInfo> portInfoList = new List<GPIOPortInfo>();



//            // PENDING888: Testing hasGPIOModule, getGPIOPortInfoList, REMOVED

//            //portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 1));

//            //portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 2));

//            //portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 3));

//            //portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 4));

//            //return portInfoList.ToArray();



//            OptData gpio = getModuleData((uint16_t)ModuleTypeCode.GPIO);



//            if (gpio == null)

//                return null;



//            //

//            // Old Version (Obsoleted)

//            //Type Subtype  Port 1  Port 2  Port 3  Port 4

//            //

//            //5516  00      Out     Out     Out     Out                     GPIO 0:IN 4:OUT

//            //5516  01      In      Out     Out     Out                     GPIO 1:IN 3:OUT

//            //5516  02      In      In      Out     Out                     GPIO 2:IN 2:OUT

//            //5516  03      In      In      In      Out                     GPIO 3:IN 1:OUT

//            //5516  04      In      In      In      In                      GPIO 4:IN 0:OUT

//            //5516  05      Out     Out     Out     3.3V Power 3.3V;        0:IN 3:OUT

//            //5516  06      In      Out     Out     3.3V Power 3.3V;        1:IN 2:OUT

//            //5516  07      In      In      Out     3.3V Power 3.3V;        2:IN 1:OUT

//            //5516  08      In      In      In      3.3V Power 3.3V;        3:IN 0:OUT

//            //5516  09      Out     Out     Out     12V Power 12V;          0:IN 3:OUT

//            //5516  0A      In      Out     Out     12V Power 12V;          1:IN 2:OUT

//            //5516  0B      In      In      Out     12V Power 12V;          2:IN 1:OUT

//            //5516  0C      In      In      In  12V Power 12V;              3:IN 0:OUT

//            //



//            //

//            // New Version From Mike (Jan 15, 2019) (In is switched to Out in firmware side)

//            //

//            //Type Subtype  Port 1  Port 2  Port 3  Port 4

//            //

//            //5516  00      In      In     In       In                    GPIO 4:IN 0:OUT

//            //5516  01      Out     In     In       In                    GPIO 3:IN 1:OUT

//            //5516  02      Out     Out    In       In                    GPIO 2:IN 2:OUT

//            //5516  03      Out     Out    Out      In                    GPIO 1:IN 3:OUT

//            //5516  04      Out     Out    Out      Out                   GPIO 0:IN 4:OUT

//            //5516  05      In      In     In       3.3V Power 3.3V;      GPIO 3:IN 0:OUT

//            //5516  06      Out     In     In       3.3V Power 3.3V;      GPIO 2:IN 1:OUT

//            //5516  07      Out     Out    In       3.3V Power 3.3V;      GPIO 1:IN 2:OUT

//            //5516  08      Out     Out    Out      3.3V Power 3.3V;      GPIO 0:IN 3:OUT

//            //5516  09      In      In     In       12V Power 12V;        GPIO 3:IN 0:OUT

//            //5516  0A      Out     In     In       12V Power 12V;        GPIO 2:IN 1:OUT

//            //5516  0B      Out     Out    In       12V Power 12V;        GPIO 1:IN 2:OUT

//            //5516  0C      Out     Out    Out      12V Power 12V;        GPIO 0:IN 3:OUT

//            //



//            if (gpio.moduleSubType == 0x00)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 4));

//            }

//            else if (gpio.moduleSubType == 0x01)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 4));



//            }

//            else if (gpio.moduleSubType == 0x02)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 4));



//            }

//            else if (gpio.moduleSubType == 0x03)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 4));

//            }

//            else if (gpio.moduleSubType == 0x04)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 4));

//            }

//            else if (gpio.moduleSubType == 0x05)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Power3Dot3V, 4));



//            }

//            else if (gpio.moduleSubType == 0x06)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Power3Dot3V, 4));

//            }

//            else if (gpio.moduleSubType == 0x07)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Power3Dot3V, 4));

//            }

//            else if (gpio.moduleSubType == 0x08)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Power3Dot3V, 4));

//            }

//            else if (gpio.moduleSubType == 0x09)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Power12V, 4));

//            }

//            else if (gpio.moduleSubType == 0x0A)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Power12V, 4));

//            }

//            else if (gpio.moduleSubType == 0x0B)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Input, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Power12V, 4));

//            }

//            else if (gpio.moduleSubType == 0x0C)

//            {

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 1));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 2));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Output, 3));

//                portInfoList.Add(new GPIOPortInfo(GPIOPortInfo.PortType.Power12V, 4));

//            }



//            if (portInfoList.Count == 4)

//                return portInfoList.ToArray();

//            else

//                return null;

//        }



//        public string toString()

//        {

//            string s = string.Empty;



//            s += ("ModelNumber=" + modelNumber + " (" + (int)getPanelSize() + "\" Panel)" + Environment.NewLine);

//            s += ("HardwareVersion=" + hardwareVersion + Environment.NewLine);

//            s += ("SerialNumber=" + serialNumber + Environment.NewLine);

//            s += ("FirmwareVersin=" + firmwareVersin + Environment.NewLine);

//            s += ("Num Options Present =" + optDataList.Count + Environment.NewLine);



//            for (int i = 0; i < optDataList.Count; i++)

//            {

//                s += ("   Option " + (i + 1) + optDataList.Count + Environment.NewLine);

//                s += optDataList[i].toString("   ");

//            }



//            return s;

//        }

//    }



//    public class PMGConfiguration

//    {

//        public EEPROM_Param_Data_t paramMapData = new EEPROM_Param_Data_t();

//        public List<PageFile> pageFiles = new List<PageFile>();

//        public List<ScheduledOperation> scheduledOperations = new List<ScheduledOperation>();

//        public PMDDisplaySize displayType = PMDDisplaySize.TwelveInchPMD;



//        private void resetData()

//        {

//            pageFiles.Clear();

//            scheduledOperations.Clear();

//            paramMapData = new EEPROM_Param_Data_t();

//        }



//        // Read configuration from Text

//        public Boolean readConfiguration(string s)

//        {

//            string name = string.Empty, value = string.Empty;

//            int i = 0;



//            string[] segList = Regex.Split(s, Environment.NewLine);



//            // Initialize

//            resetData();



//            // Reading Parameter Map

//            int mapStartIdx = -1;

//            int mapEndIdx = -1;



//            for (i = 0; i < segList.Length; i++)

//            {

//                // Page section is at the bottom part

//                if (segList[i].Contains("<ParameterMap"))

//                    mapStartIdx = i;

//                if (segList[i].Contains("</ParameterMap"))

//                    mapEndIdx = i;



//                if (mapStartIdx != -1 && mapEndIdx != -1 && mapEndIdx > mapStartIdx)

//                {

//                    string mapData = string.Empty;



//                    for (int k = mapStartIdx + 1; k < mapEndIdx; k++)

//                        mapData += (segList[k] + Environment.NewLine);



//                    if (!paramMapData.fromHexString(mapData))

//                        return false;



//                    break;

//                }



//                if (!Utils.GetNameValue(segList[i], ref name, ref value))

//                    continue;



//                if (name.Equals("DisplayType"))

//                    displayType = (PMDDisplaySize)Convert.ToInt16(value);

//            }



//            int pageStartIdx = -1;

//            int pageEndIdx = -1;



//            for (; i < segList.Length; i++)

//            {

//                if (segList[i].Contains("<Page"))

//                    pageStartIdx = i;

//                if (segList[i].Contains("</Page"))

//                    pageEndIdx = i;



//                if (pageEndIdx != -1 && pageStartIdx != -1 && pageEndIdx > pageStartIdx)

//                {

//                    string pageStringData = string.Empty;

//                    PageFile pageFile = null;



//                    for (int k = pageStartIdx + 1; k < pageEndIdx; k++)

//                        pageStringData += (segList[k] + Environment.NewLine);



//                    if (segList[pageStartIdx].Contains("Text"))

//                        pageFile = new PageTextFile(string.Empty, PMDDisplaySize.TwelveInchPMD, false);

//                    else if (segList[pageStartIdx].Contains("Graphic"))

//                        pageFile = new PageGraphicFile(string.Empty, PMDDisplaySize.TwelveInchPMD);

//                    else if (segList[pageStartIdx].Contains("Animation"))

//                        pageFile = new PageAnimationFile(string.Empty, PMDDisplaySize.TwelveInchPMD);

//                    else if (segList[pageStartIdx].Contains("Composite"))

//                        pageFile = new PageCompositeFile(string.Empty, PMDDisplaySize.TwelveInchPMD);

//                    else if (segList[pageStartIdx].Contains("Calendar"))

//                        pageFile = new PageCalendarFile();



//                    if (pageFile != null && pageFile.fromString(pageStringData))

//                    {

//                        pageFiles.Add(pageFile);

//                    }



//                    pageStartIdx = -1;

//                    pageEndIdx = -1;

//                }

//            }



//            pageStartIdx = -1;

//            pageEndIdx = -1;



//            for (i = 0; i < segList.Length; i++)

//            {

//                if (segList[i].Contains("<ScheduledOperation"))

//                    pageStartIdx = i;

//                if (segList[i].Contains("</ScheduledOperation"))

//                    pageEndIdx = i;



//                if (pageEndIdx != -1 && pageStartIdx != -1 && pageEndIdx > pageStartIdx)

//                {

//                    string timerData = string.Empty;



//                    ScheduledOperation opration = null;



//                    for (int k = pageStartIdx + 1; k < pageEndIdx; k++)

//                        timerData += (segList[k] + Environment.NewLine);



//                    opration = new ScheduledOperation();



//                    if (opration.fromString(timerData))

//                    {

//                        scheduledOperations.Add(opration);

//                    }



//                    pageStartIdx = -1;

//                    pageEndIdx = -1;

//                }

//            }



//            return true;

//        }



//        // Write configuration to Text

//        public Boolean writeConfiguration(ref string s)

//        {

//            int i;



//            s = string.Empty;



//            // Write comment part first

//            s = "#" + Environment.NewLine;



//            s += ("# Modified Time: " +

//                  DateTime.Now.ToLongDateString() + ", " +

//                  DateTime.Now.ToLongTimeString()) + Environment.NewLine;



//            s += ("#" + Environment.NewLine);



//            s += ("Version=2" + Environment.NewLine);

//            s += ("DisplayType=" + (int)displayType + Environment.NewLine);



//            string mapHexData = paramMapData.toHexString();



//            s += ("<ParameterMap>" + Environment.NewLine);

//            s += (mapHexData + Environment.NewLine);

//            s += ("</ParameterMap>" + Environment.NewLine + Environment.NewLine);



//            //

//            // Write scheduled operation

//            //               

//            for (i = 0; i < scheduledOperations.Count; i++)

//            {

//                s += ("<ScheduledOperation>" + Environment.NewLine);

//                s += (scheduledOperations[i].toString() + Environment.NewLine);

//                s += ("</ScheduledOperation>" + Environment.NewLine + Environment.NewLine);

//            }



//            //

//            // Write Text, Graphic first

//            //

//            for (i = 0; i < pageFiles.Count; i++)

//            {

//                if (pageFiles[i].pageType != PageType.Text &&

//                    pageFiles[i].pageType != PageType.Graphic)

//                    continue;



//                s += ("<Page " + pageFiles[i].pageType.ToString() + ">" + Environment.NewLine);

//                s += (pageFiles[i].toString() + Environment.NewLine);

//                s += ("</Page>" + Environment.NewLine + Environment.NewLine);

//            }



//            //

//            // Write rest of different types (Animation, Composite and Calendar)

//            //              

//            for (i = 0; i < pageFiles.Count; i++)

//            {

//                if (pageFiles[i].pageType == PageType.Text ||

//                    pageFiles[i].pageType == PageType.Graphic)

//                    continue;



//                s += ("<Page " + pageFiles[i].pageType.ToString() + ">" + Environment.NewLine);

//                s += (pageFiles[i].toString() + Environment.NewLine);

//                s += ("</Page>" + Environment.NewLine + Environment.NewLine);

//            }



//            return true;

//        }



//        public Boolean syntaxCheck(ref string errorMsg)

//        {

//            return paramMapData.syntaxCheck(ref errorMsg);



//        }

//    }



//    public class Util

//    {

//        #region

//        static public uint32_t GetUint32(byte[] bytes, ref int byteIdx)

//        {

//            uint32_t value = BitConverter.ToUInt32(bytes, byteIdx);

//            byteIdx += 4;



//            return value;

//        }



//        static public void SetUint32(uint32_t value, ref byte[] bytes, ref int byteIdx)

//        {

//            byte[] byteArray = BitConverter.GetBytes(value);



//            Buffer.BlockCopy(byteArray, 0, bytes, byteIdx, 4);

//            byteIdx += 4;

//        }



//        static public uint32_t GetUint32(byte[] bytes, int byteIdx)

//        {

//            uint32_t value = BitConverter.ToUInt32(bytes, byteIdx);

//            return value;

//        }



//        static public void SetUint32(uint32_t value, ref byte[] bytes, int byteIdx)



//        {

//            byte[] byteArray = BitConverter.GetBytes(value);



//            Buffer.BlockCopy(byteArray, 0, bytes, byteIdx, 4);

//        }



//        static public uint8_t GetUint8(byte[] bytes, ref int byteIdx)

//        {

//            byteIdx += 1;

//            return bytes[byteIdx - 1];

//        }



//        static public uint8_t GetUint8(byte[] bytes, int byteIdx)

//        {

//            return bytes[byteIdx];

//        }

//        static public void SetUint8(uint8_t value, ref byte[] bytes, ref int byteIdx)

//        {

//            bytes[byteIdx] = value;

//            byteIdx += 1;

//        }



//        static public void SetUint8(uint8_t value, ref byte[] bytes, int byteIdx)

//        {

//            bytes[byteIdx] = value;

//        }



//        static public int8_t GetInt8(byte[] bytes, ref int byteIdx)

//        {

//            byteIdx += 1;

//            return bytes[byteIdx - 1];

//        }



//        static public void SetInt8(int8_t value, ref byte[] bytes, ref int byteIdx)

//        {

//            bytes[byteIdx] = value;

//            byteIdx += 1;

//        }



//        static public float GetFloat(byte[] bytes, ref int byteIdx)

//        {

//            float value = BitConverter.ToSingle(bytes, byteIdx);

//            byteIdx += 4;

//            return value;

//        }



//        static public void SetFloat(float value, ref byte[] bytes, ref int byteIdx)

//        {

//            byte[] byteArray = BitConverter.GetBytes(value);



//            Buffer.BlockCopy(byteArray, 0, bytes, byteIdx, 4);

//            byteIdx += 4;

//        }



//        static public int16_t GetInt16(byte[] bytes, int byteIdx)

//        {

//            int16_t value = BitConverter.ToInt16(bytes, byteIdx);

//            byteIdx += 2;

//            return value;

//        }



//        static public void SetInt16(int16_t value, ref byte[] bytes, ref int byteIdx)

//        {

//            byte[] byteArray = BitConverter.GetBytes(value);



//            Buffer.BlockCopy(byteArray, 0, bytes, byteIdx, 2);

//            byteIdx += 2;

//        }



//        static public uint16_t GetUint16(byte[] bytes, ref int byteIdx)

//        {

//            uint16_t value = BitConverter.ToUInt16(bytes, byteIdx);

//            byteIdx += 2;

//            return value;

//        }



//        static public uint16_t GetUint16(byte[] bytes, int byteIdx)

//        {

//            uint16_t value = BitConverter.ToUInt16(bytes, byteIdx);

//            return value;

//        }



//        static public void SetUint16(uint16_t value, ref byte[] bytes, ref int byteIdx)

//        {

//            byte[] byteArray = BitConverter.GetBytes(value);



//            Buffer.BlockCopy(byteArray, 0, bytes, byteIdx, 2);

//            byteIdx += 2;

//        }



//        static public void SetUint16(uint16_t value, ref byte[] bytes, int byteIdx)

//        {

//            byte[] byteArray = BitConverter.GetBytes(value);

//            Buffer.BlockCopy(byteArray, 0, bytes, byteIdx, 2);

//        }



//        static public uint8_t[] GetByteArray(byte[] bytes, ref int byteIdx, int len)

//        {

//            uint8_t[] value = new uint8_t[len];



//            if (byteIdx + len <= bytes.Length)

//            {

//                Buffer.BlockCopy(bytes, byteIdx, value, 0, len);

//                byteIdx += len;



//                return value;

//            }

//            else

//            {

//                // Empty Value

//                return value;

//            }

//        }



//        static public uint8_t[] GetByteArray(byte[] bytes, int byteIdx, int len)

//        {

//            uint8_t[] value = new uint8_t[len];



//            if (byteIdx + len <= bytes.Length)

//            {

//                Buffer.BlockCopy(bytes, byteIdx, value, 0, len);

//                byteIdx += len;



//                return value;

//            }

//            else

//            {

//                // Empty Value

//                return value;

//            }

//        }



//        static public void SetByteArray(byte[] value, ref byte[] bytes, ref int byteIdx)

//        {



//            Buffer.BlockCopy(value, 0, bytes, byteIdx, value.Length);

//            byteIdx += value.Length;

//        }



//        static public void SetByteArray(byte[] value, ref byte[] bytes, int byteIdx)

//        {



//            Buffer.BlockCopy(value, 0, bytes, byteIdx, value.Length);

//            byteIdx += value.Length;

//        }



//        static public uint16_t U16ComputeCRC(byte[] data, int startIdx, int len)

//        {

//            int u8Bit, i;

//            uint16_t u16CRC = 0xFFFF;

//            uint16_t u16Odd;



//            for (i = startIdx; i < startIdx + len; i++)

//            {

//                u16CRC ^= ((uint16_t)(data[i] << 8));



//                for (u8Bit = 0; u8Bit < 8; u8Bit++)

//                {

//                    u16Odd = (uint16_t)(u16CRC & 0x8000);



//                    u16CRC <<= 1;



//                    if (u16Odd == 0x8000)

//                    {

//                        u16CRC ^= 0x1021;  //C13 + C6 + C1

//                    }

//                }

//            }



//            return (u16CRC);

//        }



//        //

//        // Recalculate new CRC from existing CRC when extra byte is added

//        // (This is used extra 9 padding is considered)

//        //

//        static public uint16_t U16ComputeCRC(uint16_t u16CRC, byte extraByteData)

//        {

//            int u8Bit;

//            uint16_t u16Odd;



//            u16CRC ^= ((uint16_t)(extraByteData << 8));



//            for (u8Bit = 0; u8Bit < 8; u8Bit++)

//            {

//                u16Odd = (uint16_t)(u16CRC & 0x8000);



//                u16CRC <<= 1;



//                if (u16Odd == 0x8000)

//                {

//                    u16CRC ^= 0x1021;  //C13 + C6 + C1

//                }

//            }



//            return (u16CRC);

//        }



//        // 

//        // Last 2 bytes are checksum

//        //

//        static public bool IsU16CheckSumValid(byte[] data, int startIdx, int len)

//        {

//            uint16_t value = U16ComputeCRC(data, startIdx, len - 2);



//            byte lsbCS = data[startIdx + len - 2];

//            byte msbCS = data[startIdx + len - 1];



//            if ((lsbCS == (value & 0xff)) && (msbCS == ((value >> 8) & 0xff)))

//                return true;

//            else

//                return false;

//        }

//        public static DateTime ConvertFromUnixTimestamp(uint32_t timestamp,

//                                                        DateTimeKind zone = DateTimeKind.Local)

//        {

//            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, zone);

//            return origin.AddSeconds(timestamp);

//        }



//        public static DateTime GetDayTimestamp(DateTime clock)

//        {

//            return new DateTime(clock.Year, clock.Month, clock.Day, 0, 0, 0, DateTimeKind.Local);

//        }

//        public static uint32_t ConvertToUnixTimestamp(DateTime date, DateTimeKind zone = DateTimeKind.Local)

//        {

//            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, zone);

//            TimeSpan diff = date.ToLocalTime() - origin;



//            return (uint32_t)diff.TotalSeconds;

//        }





//        public static void SetValue(ref byte[] data, int idx, int length, byte value)

//        {

//            int i;



//            for (i = idx; i < idx + length; i++)

//            {

//                if (i >= data.Length)

//                    return;



//                data[i] = value;

//            }

//        }

//        public static byte[] StringToByteArrayFastest(string hex, int minArraySize = 0)

//        {

//            if (hex.Length % 2 == 1)

//                return null;



//            int len = hex.Length >> 1;

//            int additionalSize = 0;



//            if (minArraySize != 0)

//            {

//                if (len < minArraySize)

//                    additionalSize = minArraySize - len;

//            }



//            byte[] arr = new byte[len + additionalSize];



//            for (int i = 0; i < len; ++i)

//            {

//                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));

//            }



//            return arr;

//        }



//        public static void StringToByteArray(string s, ref byte[] byteArray)

//        {

//            int i;



//            for (i = 0; i < byteArray.Length; i++)

//                byteArray[i] = 0;



//            if (s == string.Empty)

//                return;



//            byte[] data = Encoding.ASCII.GetBytes(s);

//            int len = data.Length;



//            if (len >= byteArray.Length - 1)

//                len = byteArray.Length - 1;



//            Buffer.BlockCopy(data, 0, byteArray, 0, len);

//        }



//        public static int GetHexVal(char hex)

//        {

//            int val = (int)hex;

//            return val - (val < 58 ? 48 : 55);

//        }



//        public static uint Hash(String s)

//        {

//            uint h = 0;

//            for (int i = 0; i < s.Length; i++)

//            {

//                h = 31 * h + s[i];

//            }

//            return h;

//        }



//        public static uint GetPageId(PageType pageType, string pageName, Boolean isSystemPage)

//        {

//            uint id = 0;



//            //

//            // 1st byte: 01 -- Text, 02 -- Graphic, 03 -- Animation

//            // 2nd byte: Bit 8 --> 1 For System Page, 0 --> User Page

//            // 3rd byte:

//            // 4rd byte:

//            //

//            if (pageType == PageType.Animation)

//                id = 0x03000000;

//            else if (pageType == PageType.Graphic)

//                id = 0x02000000;

//            else

//                id = 0x01000000;



//            if (isSystemPage)

//                id += (0x800000);



//            // Now generate additional id from name

//            uint value = Util.Hash(pageName);

//            value = value & 0x7FFFFF;



//            id += value;



//            return id;

//        }



//        public static Boolean GetNameValue(string s, ref string name, ref string value)

//        {

//            int idx = s.IndexOf('=');



//            if (idx == -1)

//                return false;



//            value = string.Empty;

//            name = s.Substring(0, idx);



//            if (idx + 1 < s.Length)

//            {

//                value = s.Substring(idx + 1, s.Length - idx - 1);

//            }



//            name = name.Trim();

//            value = value.Trim();



//            return true;

//        }

//        public static void AddArrayToList(ref List<byte> byteList, byte[] byteArray, int len = 0)

//        {

//            if (byteArray == null)

//                return;



//            if (len == 0)

//                len = byteArray.Length;



//            if (len > byteArray.Length)

//                len = byteArray.Length;



//            for (int i = 0; i < len; i++)

//            {

//                byteList.Add(byteArray[i]);

//            }

//        }



//        public static void AddArrayToList(ref List<byte> byteList, byte[] byteArray, int startIndex, int len)

//        {

//            if (byteArray == null)

//                return;



//            if (len == 0)

//                len = byteArray.Length - startIndex;



//            if (startIndex + len > byteArray.Length)

//            {

//                len = byteArray.Length - startIndex;

//            }



//            for (int i = startIndex; i < startIndex + len; i++)

//            {

//                byteList.Add(byteArray[i]);

//            }

//        }



//        public static string GetFilename(byte[] filenameData, Boolean checkInvalidFilenameChar = true)

//        {

//            if (filenameData == null)

//                return string.Empty;



//            string s = System.Text.Encoding.Default.GetString(filenameData);



//            s = s.Replace("\0", string.Empty);

//            s = s.Trim();



//            if (s.Length == 0)

//                return string.Empty;



//            if (checkInvalidFilenameChar)

//            {

//                if (s.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)

//                    return string.Empty;

//            }



//            return s;

//        }



//        public static string GetAsciiString(byte[] lineData)

//        {

//            string s = System.Text.Encoding.Default.GetString(lineData);



//            s = s.Replace("\0", string.Empty);

//            s = s.Trim();



//            if (s.Length == 0)

//                return string.Empty;

//            else

//                return s;

//        }



//        public static string GetAsciiSubString(byte[] lineData, int idx, int len)

//        {

//            string s = System.Text.Encoding.Default.GetString(lineData, idx, len);



//            s = s.Replace("\0", string.Empty);

//            s = s.Trim();



//            s = Regex.Replace(s, "[^A-Za-z0-9 _]", "");



//            if (s.Length == 0)

//                return string.Empty;

//            else

//                return s;

//        }



//        public static byte[] ConvertBitmapDataInOneDimentionArray(byte[,] mBitmapData)

//        {

//            int x, y;

//            int w, h;

//            int byteIdx, bitIdx;



//            w = mBitmapData.GetLength(0);

//            h = mBitmapData.GetLength(1);



//            int totalByte = (w * h + 7) / 8;



//            byte[] data = new byte[totalByte];

//            int pixelNo;



//            for (y = 1; y <= h; y++)

//            {

//                for (x = 1; x <= w; x++)

//                {

//                    if (mBitmapData[x - 1, y - 1] == 0)

//                        continue;



//                    pixelNo = (y - 1) * w + x;



//                    byteIdx = (pixelNo - 1) / 8;

//                    bitIdx = 8 - (pixelNo - byteIdx * 8);



//                    data[byteIdx] += (byte)(1 << bitIdx);

//                }

//            }



//            return data;

//        }



//        public static Boolean ConvertFromOneDimentionArrayToBitmapData(byte[] data, ref byte[,] bitmapData)

//        {

//            int x, y;

//            int w, h;

//            int byteIdx;

//            int bitIdx;



//            w = bitmapData.GetLength(0);

//            h = bitmapData.GetLength(1);



//            int totalByte = (w * h + 7) / 8;

//            int pixelNo;

//            byte pixelData;



//            // Not enough data

//            if (totalByte > data.Length)

//                return false;



//            for (y = 1; y <= h; y++)

//            {

//                for (x = 1; x <= w; x++)

//                {

//                    pixelNo = (y - 1) * w + x;



//                    byteIdx = (pixelNo - 1) / 8;

//                    bitIdx = 8 - (pixelNo - byteIdx * 8);



//                    pixelData = (byte)((data[byteIdx] >> bitIdx) & 0x01);

//                    if (pixelData != 0)

//                        bitmapData[x - 1, y - 1] = 1;

//                }

//            }



//            return true;

//        }



//        public static byte[] GetFixedLengthDataFromString(string str, int fixedLength)

//        {

//            byte[] data = new byte[fixedLength];



//            if (str != null && str != string.Empty && str.Length > 0)

//            {

//                byte[] strData = Encoding.ASCII.GetBytes(str);

//                int len = strData.Length;



//                if (len >= fixedLength)

//                    len = fixedLength - 1;



//                Buffer.BlockCopy(strData, 0, data, 0, len);

//            }



//            return data;

//        }





//        // Bit 0 -- Mon

//        // Bit 1 -- Tue

//        public static string GetDaysRepresentative(byte days)

//        {

//            string s = string.Empty;



//            if (days == 0x7F)

//                return "Daily";



//            if ((days & 0x01) != 0)

//                s += "M,";



//            if ((days & (0x01 << 1)) != 0)

//                s += "Tu,";



//            if ((days & (0x01 << 2)) != 0)

//                s += "W,";



//            if ((days & (0x01 << 3)) != 0)

//                s += "Th,";



//            if ((days & (0x01 << 4)) != 0)

//                s += "F,";



//            if ((days & (0x01 << 5)) != 0)

//                s += "Sa,";



//            if ((days & (0x01 << 6)) != 0)

//                s += "Su";



//            s = s.TrimEnd(',');



//            //if ((days & 0x01) != 0)

//            //    s += "M";



//            //if ((days & (0x01 << 1)) != 0)

//            //    s += "T";



//            //if ((days & (0x01 << 2)) != 0)

//            //    s += "W";



//            //if ((days & (0x01 << 3)) != 0)

//            //    s += "U";



//            //if ((days & (0x01 << 4)) != 0)

//            //    s += "F";



//            //if ((days & (0x01 << 5)) != 0)

//            //    s += "S";



//            //if ((days & (0x01 << 6)) != 0)

//            //    s += "N";



//            return s;

//        }



//        // The data is in format as following:

//        //

//        // Fileame Length

//        // Filename 

//        // Filename Length

//        // Filename

//        // ......

//        //

//        public static string[] GetFilenameArrayFromData(byte[] data, int startIdx = 0)

//        {

//            List<string> filenameList = new List<string>();



//            string filename;

//            int filenameLen;



//            while (true)

//            {

//                if (startIdx >= data.Length)

//                    break;



//                filenameLen = data[startIdx];



//                // Max filename allowed

//                if (filenameLen >= 35)

//                    break;



//                // Not enough data

//                if (startIdx + filenameLen >= data.Length)

//                    break;



//                byte[] filenameData = new byte[filenameLen];



//                Buffer.BlockCopy(data, startIdx + 1, filenameData, 0, filenameLen);



//                filename = GetFilename(filenameData, false);



//                if (filename != string.Empty)

//                    filenameList.Add(filename);



//                startIdx += (filenameLen + 1);

//            }



//            return filenameList.ToArray();

//        }



//        // The data is in format as following:

//        //

//        // 2 Byte Hash

//        // Fileame Length

//        // Filename 

//        //

//        // 2 Byte Hash

//        // Filename Length

//        // Filename

//        // ......

//        //

//        public static PageInfo[] GetPageInfoArrayFromData(byte[] data, int startIdx = 0)

//        {

//            List<PageInfo> pageInfoList = new List<PageInfo>();



//            string filename;

//            int filenameLen;

//            byte hashLSB;

//            byte hashMSB;



//            while (true)

//            {

//                if (startIdx + 2 >= data.Length)

//                    break;



//                hashLSB = data[startIdx];

//                hashMSB = data[startIdx + 1];

//                startIdx += 2;



//                filenameLen = data[startIdx];



//                // Max filename allowed

//                if (filenameLen >= 35)

//                    break;



//                // Not enough data

//                if (startIdx + filenameLen >= data.Length)

//                    break;



//                byte[] filenameData = new byte[filenameLen];



//                Buffer.BlockCopy(data, startIdx + 1, filenameData, 0, filenameLen);



//                filename = GetFilename(filenameData, false);



//                if (filename != string.Empty)

//                {

//                    PageInfo pageInfo = new PageInfo();

//                    pageInfoList.Add(new PageInfo(filename, hashLSB, hashMSB));

//                }



//                startIdx += (filenameLen + 1);

//            }



//            return pageInfoList.ToArray();

//        }



//        static public bool ByteArrayCompare(byte[] a1, byte[] a2)

//        {

//            if (a1.Length != a2.Length)

//                return false;



//            for (int i = 0; i < a1.Length; i++)

//                if (a1[i] != a2[i])

//                    return false;



//            return true;

//        }



//        public static bool IsWithinRange(int value, int minimum, int maximum)

//        {

//            return value >= minimum && value <= maximum;

//        }



//        static public string UintToIP(ulong longIP)

//        {

//            string ip = string.Empty;



//            ip = (longIP & 0xFF) + "." +

//                 ((longIP >> 8) & 0xFF) + "." +

//                 ((longIP >> 16) & 0xFF) + "." +

//                 ((longIP >> 24) & 0xFF);



//            return ip;

//        }



//        public static uint IP2UInt(string ip)

//        {

//            string[] ipBytes;

//            uint num = 0;



//            if (!string.IsNullOrEmpty(ip))

//            {

//                ipBytes = ip.Split('.');



//                if (ipBytes.Length == 4)

//                {

//                    for (int i = 0; i < 4; i++)

//                    {

//                        byte data = (byte)int.Parse(ipBytes[i]);



//                        num += (uint)(data << (i * 8));

//                    }

//                }

//            }



//            return (uint)num;

//        }



//        public static Boolean IsValidIPAddress(string text)

//        {

//            string pattern = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";



//            if (Regex.IsMatch(text, pattern))

//                return true;

//            else

//                return false;

//        }



//        #endregion

//    }

//}

