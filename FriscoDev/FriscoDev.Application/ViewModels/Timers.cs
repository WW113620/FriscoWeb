using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class Timers
    {
        public int functionType { get; set; }
        public int modeType { get; set; }
        public DateTime startDate { get; set; }
        public DateTime startTime { get; set; }
        public DateTime stopDate { get; set; }
        public DateTime stopTime { get; set; }
        public int days { get; set; }
        public int speedLimit { get; set; }
        public int linkToCalendar { get; set; }

        public string daysName { get; set; }
    }
}
