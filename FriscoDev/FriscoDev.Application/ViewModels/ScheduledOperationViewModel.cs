using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class ScheduledOperationViewModel
    {
        public int DisplayType { get; set; }
        public string OperationName { get; set; }
        public string DatePeriod { get; set; }
        public string TimePeriod { get; set; }
        public string Recurrence { get; set; }
        public string Description { get; set; }
    }
}
