using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FriscoDev.Application.Interface.PacketProtocol;

namespace FriscoDev.Application.Models
{
    public partial class PMGConfiguration
    {
        public PMGConfiguration() { }
        public PMGConfiguration(int pmgid, ParamaterId paramIdIn, string value, byte state)
        {

            PMG_ID = pmgid;
            Parameter_ID = (int)paramIdIn;
            Value = value;
            State = state;
        }

    }
}
