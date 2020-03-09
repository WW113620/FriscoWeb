//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FriscoDev.Application.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PMD
    {
        public string PMDName { get; set; }
        public string IMSI { get; set; }
        public Nullable<int> DeviceType { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public Nullable<bool> Connection { get; set; }
        public Nullable<bool> StatsCollection { get; set; }
        public Nullable<int> PMD_ID { get; set; }
        public string Clock { get; set; }
        public string FirmwareVersion { get; set; }
        public string NewFirmwareId { get; set; }
        public Nullable<int> NumFirmwareUpdateAttempts { get; set; }
        public Nullable<int> KeepAliveMessageInterval { get; set; }
        public string CS_ID { get; set; }
        public Nullable<byte> HighSpeedAlert { get; set; }
        public Nullable<byte> LowSpeedAlert { get; set; }
        public Nullable<int> CurrentPMGConfigurationHash { get; set; }
        public string CurrrentConfigurationTime { get; set; }
        public byte[] CurrentPMGConfiguration { get; set; }
        public string CurrentPMGPageFileList { get; set; }
        public string SocketAddress { get; set; }
    }
}
