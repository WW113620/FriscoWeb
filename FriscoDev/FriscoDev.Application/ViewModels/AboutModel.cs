using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class AboutModel
    {
        public AboutModel()
        {
            ModuleList = new List<ModuleModel>();
        }
        public int PMGID { get; set; }
        public string LabelPMG { get; set; }
        public int PanelSize { get; set; }
        public string PMGSerialNumber { get; set; }
        public string PMGModel { get; set; }

        public List<ModuleModel> ModuleList { get; set; }
    }

    public class ModuleModel
    {
        public string moduleName { get; set; }
        public string serialNumber { get; set; }
        public string hardwareVersion { get; set; }
        public string firmwareVersion { get; set; }
        public string moduleLibFirmwareVersion { get; set; }
    }
}
