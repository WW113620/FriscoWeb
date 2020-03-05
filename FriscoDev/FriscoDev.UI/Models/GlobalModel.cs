using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Models
{
    public class GlobalModel
    {
        public int minSpeed { get; set; }
        public int maxSpeed { get; set; }
        public int alertFlashType { get; set; }
        public int alertMessageType { get; set; }
        public int dataFormat { get; set; }

        public int userClock { get; set; }
        public string clockDate { get; set; }
        public string clockTime { get; set; }
    }
}