using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class InitChartEntity
    {
    }
    public class SpeedPercentileEntity
    {
        public string TimeSplit { get; set; }
        public int MondayCount { get; set; }
        public int TuesdayCount { get; set; }
        public int WednesdayCount { get; set; }
        public int ThursdayCount { get; set; }
        public int FridayCount { get; set; }
        public int SaturdayCount { get; set; }
        public int SundayCount { get; set; }

    }

    public class CountPercentileEntity
    {
        public int AverageSpeed { get; set; }
        public int iCount { get; set; }

    }

    public class StatsLogNew : StatsLogVm
    {
        public int WeekDate { get; set; }
        public string SubDate { get; set; }
        public string TimeSplit { get; set; }
    }
}
