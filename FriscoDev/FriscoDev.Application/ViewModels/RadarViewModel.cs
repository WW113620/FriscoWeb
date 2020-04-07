using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class RadarViewModel
    {
        public int PMGID { get; set; }
        public int Radar { get; set; }
        public int RadarHoldoverTime { get; set; }
        public int RadarCosine { get; set; }
        public int RadarUnitResolution { get; set; }
        public int RadarSensitivity { get; set; }
        public int RadarTargetStrength { get; set; }
        public int RadarTargetAcceptance { get; set; }
        public int RadarTargetHoldOn { get; set; }
        public int RadarOperationDirection { get; set; }
        public int RadarExternalRadarSpeed { get; set; }
        public int RadarExternalEchoPanRadarData { get; set; }

    }
}
