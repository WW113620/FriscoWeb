using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class DeviceLocationVm
    {
        public int PMDID { get; set; }
        public int ID { get; set; }
        public string IMSI { get; set; }
        public string PMDName { get; set; }
        public string DeviceType { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
