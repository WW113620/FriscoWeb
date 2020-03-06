using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class QuickSetupModel
    {
        public int pmgid { get; set; }
        public int pmgInch { get; set; }
        public int actionTypeIdle { get; set; }
        public string pageTypeIdle { get; set; }

        public int limitSpeed { get; set; }
        public int actionTypeLimit { get; set; }
        public string pageTypeLimit { get; set; }
        public int alertActionLimit { get; set; }

        public int alertSpeed { get; set; }
        public int actionTypeAlert { get; set; }
        public string pageTypeAlert { get; set; }
        public int alertActionAlert { get; set; }

    }
}
