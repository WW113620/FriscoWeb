using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class GPIOViewModel
    {
        public int portNumber { get; set; }
        public string PortType { get; set; }
        public bool Enabled { get; set; }
        public int ActiveState { get; set; }
        public int Duration { get; set; }

        public int DisplayType { get; set; }
        public string PageName { get; set; }
        public int AlertAction { get; set; }
        public List<SelectOption> PageList { get; set; }

    }
}
