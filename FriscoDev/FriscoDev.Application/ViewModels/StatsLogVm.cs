using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class StatsLogVm
    {
        public long RowNumber { get; set; }
        public int TargetId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Direction { get; set; }
        public double LastSpeed { get; set; }
        public double PeakSpeed { get; set; }
        public double AverageSpeed { get; set; }
        public int Strength { get; set; }
        public string Classfication { get; set; }
        public int Duration { get; set; }
        public int PMDID { get; set; }

        public string StatDate { get; set; }

        public string StatDate2 { get; set; }

        public string StrTimestamp { get; set; }
    }
    public class SpeedCount
    {
        public int AverageSpeed { get; set; }
        public int AwayCount { get; set; }
        public int ClosCount { get; set; }
    }
    public class AverageSpeed
    {
        public double AvgSpeed { get; set; }
    }
    public class SurveyDataEntity
    {
        public int weeddate { get; set; }
        public string SurveyDate { get; set; }
        public string aStartDate { get; set; }
        public string aEndDate { get; set; }
        public string cStartDate { get; set; }
        public string cEndDate { get; set; }

    }
}
