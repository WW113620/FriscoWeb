using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class PmgViewModel
    {
        public string Id { get; set; }
        public int PMGID { get; set; }
        public string IMSI { get; set; }
        public string PMDName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CountryName { get; set; }
        public string DevCoordinateX { get; set; }
        public string DevCoordinateY { get; set; }
        public string Direction { get; set; }
        public int StatsCollection { get; set; }
        public int DeviceType { get; set; }
        public int HighSpeedAlert { get; set; }
        public int LowSpeedAlert { get; set; }
        public int Connection { get; set; }

    }
}
