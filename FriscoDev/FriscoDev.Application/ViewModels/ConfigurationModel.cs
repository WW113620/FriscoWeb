using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class ConfigurationModel
    {
        public int pmgid { get; set; }
        public string pmgClock { get; set; }
        public int minSpeed { get; set; }
        public int maxSpeed { get; set; }
        public string speedUnit { get; set; }
        public string temperatureUnit { get; set; }
        /// <summary>
        ///  Set to 0 for automatic brightness.
        /// </summary>
        public int numBright { get; set; }
        public string mutcd { get; set; }
    }

    public class ConfigurationTime
    {
        public int pmgid { get; set; }
        public string time { get; set; }
        public string date { get; set; }
    }
}
