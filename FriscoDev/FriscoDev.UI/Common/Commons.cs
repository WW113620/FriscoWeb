using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using FriscoDev.Application.Enum;
using Newtonsoft.Json;
using Application.Common;

namespace FriscoDev.UI.Common
{
    public static class Commons
    {

        public static decimal GetDevCoordinateX(string location)
        {
            if (string.IsNullOrEmpty(location))
                return 0;

            var arr = location.Split(',');
            if (arr.Length > 0)
                return arr[0].ToDecimal(0);

            return 0;
        }

        public static decimal GetDevCoordinateY(string location)
        {
            if (string.IsNullOrEmpty(location))
                return 0;

            var arr = location.Split(',');
            if (arr.Length > 1)
                return arr[1].ToDecimal(0);

            return 0;
        }

        public static List<decimal> splitStringToDecimal(string location)
        {
            List<decimal> _list = new List<decimal>();
            try
            {
                if (string.IsNullOrEmpty(location))
                {
                    _list.Add(0);
                    _list.Add(0);
                    return _list;
                }

                var arr = location.Split(',');
                if (arr.Length == 2)
                {
                    _list.Add(Convert.ToDecimal(arr[0]));
                    _list.Add(Convert.ToDecimal(arr[0]));
                    return _list;
                }

                _list.Add(0);
                _list.Add(0);
                return _list;
            }
            catch (Exception ex)
            {
                _list.Add(0);
                _list.Add(0);
            }
            return _list;

        }
        public static string ConvertDirection(string strDirection, int type = 0)
        {
            string result = string.Empty;
            switch (strDirection)
            {
                default:
                case "":
                case "none":
                //result = "None";
                //break;
                case "0":
                    result = "North";
                    break;
                case "45":
                    result = "North East";
                    break;
                case "90":
                    result = "East";
                    break;
                case "135":
                    result = "South East";
                    break;
                case "180":
                    result = "South";
                    break;
                case "225":
                    result = "South West";
                    break;
                case "270":
                    result = "West";
                    break;
                case "315":
                    result = "North West";
                    break;
            }
            if (type == 1)
                result = result.Replace(" ", string.Empty);
            return result.Trim();
        }
        public static string ConvertShowDate(DateTime dt)
        {
            if (dt == null || dt == DateTime.MinValue || dt == DateTime.MaxValue)
            {
                return getLocalTime().AddYears(-1).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public static DateTime getLocalTime(string TimeZoneId = "Pacific Standard Time")
        {
            try
            {
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));
            }
            catch (Exception ex)
            {
                return DateTime.UtcNow;
            }
        }
        public static DateTime getLocalTime(DateTime curDate, string TimeZoneId = "Pacific Standard Time")
        {
            try
            {
                return TimeZoneInfo.ConvertTimeFromUtc(curDate, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));
            }
            catch (Exception ex)
            {
                return DateTime.UtcNow;
            }
        }
        public static int GetLast7Digits(string id)
        {
            int len = id.Length;
            if (len > 7)
                id = id.Remove(0, len - 7);
            int value = Convert.ToInt32(id);
            return value;
        }
        public static string SendGetHttpRequest(string url, string contentType = "application/x-www-form-urlencoded")
        {
            string result = "";
            try
            {
                WebRequest request = (WebRequest)HttpWebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = contentType;
                using (WebResponse response = request.GetResponse())
                {
                    if (response != null)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = "";
            }
            return result;
        }
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }
        public static bool CheckDateIsWorkend(DateTime dtime)
        {
            bool bl = false;
            string str = Convert.ToString(dtime.DayOfWeek);
            if (str == "Sunday" || str == "Saturday")
            {
                bl = true;
            }
            return bl;
        }
        public static TimerFunctionTypeEnum GetTimerFunctionTypeEnumName(int curVal)
        {
            TimerFunctionTypeEnum _type = new TimerFunctionTypeEnum();
            switch (curVal)
            {
                case 0:
                    _type = TimerFunctionTypeEnum.SpeedDisplay;
                    break;
                case 1:
                    _type = TimerFunctionTypeEnum.SpeedLimit;
                    break;
                case 2:
                    _type = TimerFunctionTypeEnum.TrafficStatistics;
                    break;
                case 3:
                    _type = TimerFunctionTypeEnum.Message1;
                    break;
                case 4:
                    _type = TimerFunctionTypeEnum.Message2;
                    break;
                case 5:
                    _type = TimerFunctionTypeEnum.Message3;
                    break;
                default:
                    _type = TimerFunctionTypeEnum.SpeedDisplay;
                    break;
            }
            return _type;
        }
        public static TimerModeEnum GetTimerModeEnumName(int curVal)
        {
            TimerModeEnum _type = new TimerModeEnum();
            switch (curVal)
            {
                case 0:
                    _type = TimerModeEnum.Off;
                    break;
                case 1:
                    _type = TimerModeEnum.Period;
                    break;
                case 2:
                    _type = TimerModeEnum.Daily;
                    break;
                case 3:
                    _type = TimerModeEnum.SelectedDays;
                    break;
                case 4:
                    _type = TimerModeEnum.AlwaysOn;
                    break;
                default:
                    _type = TimerModeEnum.Off;
                    break;
            }
            return _type;
        }
        public static int getNCiNum(string days = "")
        {
            int cint = 0;
            string[] _arr = days.Split(',');
            foreach (string ch in _arr)
            {
                if (!string.IsNullOrEmpty(ch))
                {
                    cint = cint + getDaysNum(ch);
                }
            }
            return cint;
        }
        public static int getDaysNum(string _dayName)
        {
            int cint = 0;
            if (_dayName == "Sun")
            {
                cint = 128;
            }
            else if (_dayName == "Mon")
            {
                cint = 64;
            }
            else if (_dayName == "Tue")
            {
                cint = 32;
            }
            else if (_dayName == "Wed")
            {
                cint = 16;
            }
            else if (_dayName == "Thu")
            {
                cint = 8;
            }
            else if (_dayName == "Fri")
            {
                cint = 4;
            }
            else if (_dayName == "Sat")
            {
                cint = 2;
            }
            else
            {
                cint = 0;
            }
            return cint;
        }
        public static TimerCalendarControlEnum GetTimerCalendarControlEnumName(int curVal)
        {
            TimerCalendarControlEnum _type = new TimerCalendarControlEnum();
            switch (curVal)
            {
                case 0:
                    _type = TimerCalendarControlEnum.Off;
                    break;
                case 1:
                    _type = TimerCalendarControlEnum.On;
                    break;
                default:
                    _type = TimerCalendarControlEnum.Off;
                    break;
            }
            return _type;
        }
        public static string getNCiString(int num = 0)
        {
            string str = "";
            String strtemp = Convert.ToString(num, 2);
            int index = strtemp.Length - 1;
            foreach (char ch in strtemp)
            {
                if (ch == '1')
                {
                    int sint = Convert.ToInt32(Math.Pow(2, index));
                    if (str == "")
                    {
                        str = getDaysName(sint);
                    }
                    else
                    {
                        str = str + "," + getDaysName(sint);
                    }
                }
                index--;
            }
            return str;
        }
        public static string getDaysName(int num)
        {
            string strtemp = "";
            if (num == 128)
            {
                strtemp = "Sun";
            }
            else if (num == 64)
            {
                strtemp = "Mon";
            }
            else if (num == 32)
            {
                strtemp = "Tue";
            }
            else if (num == 16)
            {
                strtemp = "Wed";
            }
            else if (num == 8)
            {
                strtemp = "Thu";
            }
            else if (num == 4)
            {
                strtemp = "Fri";
            }
            else if (num == 2)
            {
                strtemp = "Sat";
            }
            else
            {
                strtemp = "";
            }
            return strtemp;
        }
        public static string GetConvertWeekday(int item)
        {
            string result = string.Empty;
            switch (item)
            {
                default:
                case 1:
                    result = "Sunday";
                    break;
                case 2:
                    result = "Monday";
                    break;
                case 3:
                    result = "Tuesday";

                    break;
                case 4:
                    result = "Wednesday";
                    break;
                case 5:
                    result = "Thursday";

                    break;
                case 6:
                    result = "Friday";
                    break;
                case 7:
                    result = "Saturday";
                    break;
            }
            return result;
        }
        public static string GetStartDate(DateTime dt)
        {
            if (dt == null || dt == DateTime.MinValue || dt == DateTime.MaxValue)
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            else
                return dt.ToString("yyyy-MM-dd HH:mm");
        }
    }
}