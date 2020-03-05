using System;
namespace FriscoDev.Application.ViewModels
{
    public class PMGModel
    {
        public string Id { get; set; }
        public string BelongName { get; set; }
        public string LeasedStartDate { get; set; }
        public string LeasedEndDate { get; set; }
        public string PMDName { get; set; }
        public string IMSI { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CountryName { get; set; }
        public string ZipCode { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public bool Connection { get; set; }
        public bool StatsCollection { get; set; }
        public Nullable<int> PMD_ID { get; set; }
        public byte[] CurrentConfiguration { get; set; }
        public DateTime CurrrentConfigurationTime { get; set; }
        public byte[] NewConfiguration { get; set; }
        public DateTime NewConfigurationTime { get; set; }
        public string Clock { get; set; }
        public string FirmwareVersion { get; set; }
        public string NewFirmwareId { get; set; }
        public int NumFirmwareUpdateAttempts { get; set; }
        public int KeepAliveMessageInterval { get; set; }
        public int DeviceType { get; set; }
        public string ShowDeviceType { get; set; }
        public string CS_ID { get; set; }

        public int HighSpeedAlert { get; set; }
        public int LowSpeedAlert { get; set; }
        public string Direction { get; set; }
    }

    public class AddressModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string ZipCode { get; set; }
        public string Direction { get; set; }
        public string CountryName { get; set; }
    }
    public enum DeviceType
    {
        All = 0,
        PMD = 1,
        TDC = 2,
        PMD_Leased = 3,
        PMG_12 = 4,
        PMG_15 = 5,
        PMG_18 = 6,
    }
}
