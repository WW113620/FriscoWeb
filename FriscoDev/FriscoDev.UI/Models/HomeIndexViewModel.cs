using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Models
{
    public class HomeIndexViewModel
    {
        public List<List<double>> ChartData { get; set; }
        public ReportData Report { get; set; }
        public List<List<string>> TimeChartData { get; set; }
    }
    public class ReportData
    {
        public int TotalCount { get; set; }
        public int Closing { get; set; }
        public int Away { get; set; }
        public double Average { get; set; }
        public double Last { get; set; }
        public double HighAlam { get; set; }
        public double LowAlam { get; set; }

        public double AverageTotal { get; set; }

        public string LastDate { get; set; }

    }
}