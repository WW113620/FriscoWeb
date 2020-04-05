using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class TrafficDataModel
    {
        public string BtnName { get; set; }
        public string BtnStartText { get; set; }


    }

    public class TrafficDataViewModel
    {
        public int PMGID { get; set; }

        public int TrafficEnableRecording { get; set; }

        public int TrafficTargetStrength { get; set; }
        public int TrafficMinimumTrackingDistance { get; set; }
        public int TrafficMinimumFollowingTime { get; set; }
        /// <summary>
        /// 0=OnDemand,1=Streaming
        /// </summary>
        public int TrafficDataOnDemand { get; set; }
    }
}
