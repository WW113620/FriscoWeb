using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Models
{
    public class DeviceManagementViewModel
    {
        public IEnumerable<DeviceViewModel> Devices { get; set; }

        public class DeviceViewModel
        {
            public string Direction { get; set; }
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
            public string StatsCollection { get; set; }
            public int PMDID { get; set; }
            public byte[] CurrentConfiguration { get; set; }
            public DateTime CurrrentConfigurationTime { get; set; }
            public byte[] NewConfiguration { get; set; }
            public string NewConfigurationTime { get; set; }
            public string Clock { get; set; }
            public string isClock { get; set; }
            public string FirmwareVersion { get; set; }
            public string NewFirmwareId { get; set; }
            public int NumFirmwareUpdateAttempts { get; set; }
            public int KeepAliveMessageInterval { get; set; }
            public string DeviceType { get; set; }

            public string DevCoordinateX { get; set; }
            public string DevCoordinateY { get; set; }

            public int HighSpeedAlert { get; set; }
            public int LowSpeedAlert { get; set; }
            public int IntDeviceType { get; set; }

        }

        public class AddDevice
        {

            public string PMDName { get; set; }
            public string IMSI { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public string CountryName { get; set; }
            public string ZipCode { get; set; }
            public string Direction { get; set; }
            public string Username { get; set; }
            public string Location { get; set; }
            public bool Connection { get; set; }
            public int StatsCollection { get; set; }
            public int PMDID { get; set; }
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
            public string DevCoordinateX { get; set; }
            public string DevCoordinateY { get; set; }

            public int HighSpeedAlert { get; set; }
            public int LowSpeedAlert { get; set; }
        }

        public class EditDevice
        {
            public string Id { get; set; }
            public string BelongName { get; set; }
            public string LeasedStartDate { get; set; }
            public string LeasedEndDate { get; set; }

            public string Direction { get; set; }
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
            public int StatsCollection { get; set; }
            public int PMDID { get; set; }
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

            public string DevCoordinateX { get; set; }
            public string DevCoordinateY { get; set; }

            public int HighSpeedAlert { get; set; }
            public int LowSpeedAlert { get; set; }
        }

    }
}