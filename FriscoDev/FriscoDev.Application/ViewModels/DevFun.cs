using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.ViewModels
{
    public class DevFun
    {
        public static string GetDeviceType(int type)
        {
            string res = string.Empty;
            switch (type)
            {
                default:
                case 1:
                    res = "PMG-12";
                    break;
                case 2:
                    res = "PMG-15";
                    break;
                case 3:
                    res = "PMG-18";
                    break;

            }
            return res;
        }

        public static string GetConvertStatus(bool? bo)
        {
            if (bo == null)
                return "No";
            if (bo == true)
                return "Yes";
            return "No";
        }

        public static string ConvertStrShowDate(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "---";

            return Convert.ToDateTime(str).ToString("MM-dd-yyyy HH:mm:ss");

        }

        public static string ConvertShowDate(DateTime dt)
        {
            if (dt == DateTime.MaxValue || dt == DateTime.MinValue)
                return "---";

            return dt.ToString("MM-dd-yyyy HH:mm:ss");

        }


    }
}
