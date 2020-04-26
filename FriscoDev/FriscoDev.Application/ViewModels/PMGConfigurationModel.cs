using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class PMGConfigurationModel
    {
        public int PMG_ID { get; set; }
        public int Parameter_ID { get; set; }
        public string Value { get; set; }
        public byte[] ValueByte { get; set; }
        public byte State { get; set; }
    }
}
