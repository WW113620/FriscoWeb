using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class CommunicationViewModel
    {
        public int PMGID { get; set; }
        public string WirelessPIN { get; set; }
        public int EthernetIPSetting { get; set; }
        public string EthernetIPAddress { get; set; }

        public string EthernetSubnetMask { get; set; }

        public string EthernetDefaultGateway { get; set; }

        public int WiFIMode { get; set; }

        public int WIFIAccessPointSecurity { get; set; }

        public string WIFIAccessPointPassword { get; set; }

        public int WIFIStationSecurity { get; set; }

        public string WIFIStationPassword { get; set; }
        public string WIFIStationSSID { get; set; }
        public int WIFIStationIPType { get; set; }
        public string WIFIStationIPAddress { get; set; }
        public string WIFIStationSubnetMask { get; set; }
        public string WIFIStationDefaultGateway { get; set; }
    }
}
