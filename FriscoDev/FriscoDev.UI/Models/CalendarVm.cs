using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Models
{
    public class CalendarVm
    {
        public DateTime startDate { get; set; }
        public string value { get; set; }
        public DateTime daylightSavingStartDate { get; set; }
        public DateTime daylightSavingEndDate { get; set; }
        public bool enableDaylightSavingTime { get; set; }
        public bool calendarEnabled { get; set; }

        public int isweekend { get; set; }

        public string calendararray { get; set; }
    }
}