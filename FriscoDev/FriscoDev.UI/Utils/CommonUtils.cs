using FriscoDev.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Utils
{
    public class CommonUtils
    {



        public static List<decimal> SplitStringToDecimal(string location)
        {
            List<decimal> _list = new List<decimal>();
            try
            {
                _list.Add(Convert.ToDecimal(location.Split(',')[0]));
                _list.Add(Convert.ToDecimal(location.Split(',')[1]));
            }
            catch (Exception ex)
            {
                _list.Add(0);
                _list.Add(0);
            }
            return _list;

        }

        public static string ConvertShowDate(DateTime dt)
        {
            if (dt == null || dt == DateTime.MinValue || dt == DateTime.MaxValue)
            {
                return GetLocalTime().AddYears(-1).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }


        public static DateTime GetLocalTime(string TimeZoneId = "Pacific Standard Time")
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

        public static DateTime GetLocalTime(DateTime curDate, string TimeZoneId = "Pacific Standard Time")
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

        public static string ConvertDirection(string strDirection, int type = 0)
        {
            string result = string.Empty;
            switch (strDirection)
            {
                default:
                case "":
                case "none":
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

        public static AddressModel ForAddress(string result)
        {
            AddressModel pmd = new AddressModel();
            if (string.IsNullOrEmpty(result))
            {
                return pmd;
            }
            else
            {
                var arrAddress = result.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                if (arrAddress.Length == 7)
                {
                    pmd.Address = arrAddress[0].Replace("Address:", string.Empty);
                    pmd.City = arrAddress[1].Replace("City:", string.Empty);
                    pmd.State = arrAddress[2].Replace("State:", string.Empty);
                    pmd.Country = arrAddress[3].Replace("Country:", string.Empty);
                    pmd.ZipCode = arrAddress[4].Replace("ZipCode:", string.Empty);
                    pmd.Direction = arrAddress[5].Replace("Direction:", string.Empty);
                    pmd.CountryName = arrAddress[6].Replace("CountryName:", string.Empty);
                    return pmd;
                }
                else
                {
                    return pmd;
                }
            }
        }
        public static string ToAddress(AddressModel address)
        {
            if (address == null)
                return "";

            if (string.IsNullOrEmpty(address.Direction))
                address.Direction = "0";

            return string.Format("Address:{0}||City:{1}||State:{2}||Country:{3}||ZipCode:{4}||Direction:{5}||CountryName:{6}", address.Address, address.City, address.State, address.Country, address.ZipCode, address.Direction, address.CountryName);

        }

        public static string DateTimeValue(DateTime? dt, string ret = "")
        {
            if (dt == null || dt == DateTime.MinValue || dt == DateTime.MaxValue)
                return ret;
            return Convert.ToDateTime(dt).ToString("yyyy-MM-dd HH:mm");
        }

    }
}