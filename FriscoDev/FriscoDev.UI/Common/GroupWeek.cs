using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Common
{
    public class GroupWeek
    {
        #region Month
        /// <summary>
        /// 根据时间范围获取每年每月每周的分组
        /// </summary>
        /// <param name="strStartDate">起始时间</param>
        /// <param name="strEndDate">结束时间</param>
        /// <returns>返回每周起始结束键值对</returns>
        public static Dictionary<string, string> GetGroupWeekByDateRange(string strStartDate, string strEndDate)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            DateTime dtStartDate = DateTime.Parse(strStartDate);
            DateTime dtEndDate = DateTime.Parse(strEndDate);

            if (dtStartDate.Year == dtEndDate.Year && dtStartDate.Month == dtEndDate.Month && dtStartDate.Day == dtEndDate.Day)
            {
                return dict;
            }
            //同年
            if (dtStartDate.Year == dtEndDate.Year)
            {
                GetGroupWeekByYear(dict, dtStartDate, dtEndDate);
            }
            //不同年
            else
            {
                int WhileCount = dtEndDate.Year - dtStartDate.Year;

                //某年一共有多少天
                int YearDay = DateTime.IsLeapYear(dtStartDate.Year) ? 366 : 365;
                DateTime dtTempStartDate = dtStartDate;

                DateTime dtTempEndDate = dtTempStartDate.AddDays(YearDay - dtTempStartDate.DayOfYear);

                //根据时间范围获取每月每周的分组
                GetGroupWeekByYear(dict, dtTempStartDate, dtTempEndDate);

                for (int i = 1; i < (WhileCount + 1); i++)
                {
                    //某年某月一共有多少天
                    YearDay = DateTime.IsLeapYear(dtTempStartDate.Year + 1) ? 366 : 365;
                    dtTempStartDate = DateTime.Parse(DateTime.Parse((dtTempStartDate.Year + 1) + "." + dtTempStartDate.Month + "." + "01").ToString("yyyy.MM.dd"));
                    dtTempEndDate = dtTempStartDate.AddDays(YearDay - dtTempStartDate.DayOfYear);

                    //根据时间范围获取每月每周的分组
                    GetGroupWeekByYear(dict, dtTempStartDate, dtTempEndDate);

                }
            }

            return dict;
        }

        /// <summary>
        /// 根据时间范围(年)获取每月每周的分组
        /// </summary>
        /// <param name="dict">每周起始结束键值对</param>
        /// <param name="strStartDate">起始时间</param>
        /// <param name="strEndDate">结束时间</param>
        public static void GetGroupWeekByYear(Dictionary<string, string> dict, DateTime dtStartDate, DateTime dtEndDate)
        {
            //不同月
            if ((dtEndDate.Month - dtStartDate.Month) >= 1)
            {
                int WhileCount = dtEndDate.Month - dtStartDate.Month;

                //某年某月一共有多少天
                int MonthDay = DateTime.DaysInMonth(dtStartDate.Year, dtStartDate.Month);
                DateTime dtTempStartDate = dtStartDate;
                var startMonth = dtTempStartDate.AddDays(1 - dtTempStartDate.Day);
                DateTime dtTempEndDate = startMonth.AddMonths(1).AddDays(-1); ;

                //根据时间范围获取每月每周的分组
                GetGroupWeekByMonth(dict, dtTempStartDate, dtTempEndDate);

                for (int i = 1; i < (WhileCount + 1); i++)
                {
                    if (i == WhileCount)
                    {
                        dtTempStartDate = dtEndDate.AddDays(1 - dtEndDate.Day);
                        dtTempEndDate = dtEndDate;
                    }
                    else
                    {
                        //某年某月一共有多少天
                        MonthDay = DateTime.DaysInMonth(dtTempStartDate.Year, dtTempStartDate.Month + 1);
                        dtTempStartDate = DateTime.Parse(DateTime.Parse(dtTempStartDate.Year + "." + (dtTempStartDate.Month + 1) + "." + "01").ToString("yyyy.MM.dd"));
                        dtTempEndDate = startMonth.AddMonths(1).AddDays(-1);
                    }
                    //根据时间范围获取每月每周的分组
                    GetGroupWeekByMonth(dict, dtTempStartDate, dtTempEndDate);

                }
            }
            //同月
            else
            {
                //根据时间范围获取每月每周的分组
                GetGroupWeekByMonth(dict, dtStartDate, dtEndDate);
            }
        }

        /// <summary>
        /// 根据时间范围(月)获取每月每周的分组
        /// </summary>
        /// <param name="dict">每周起始结束键值对</param>
        /// <param name="strStartDate">起始时间</param>
        /// <param name="strEndDate">结束时间</param>
        public static void GetGroupWeekByMonth(Dictionary<string, string> dict, DateTime dtStartDate, DateTime dtEndDate)
        {
            //一周
            if ((dtEndDate.Day - dtStartDate.Day) < 7)
            {
                DayOfWeek day = dtStartDate.DayOfWeek;
                string dayString = day.ToString();

                DateTime dtTempStartDate = dtStartDate;
                DateTime dtTempEndDate = dtEndDate;
                DateTime dtTempDate = DateTime.Now;
                switch (dayString)
                {
                    case "Monday":
                        dict.Add(dtTempStartDate.ToString(), dtTempEndDate.ToString());
                        break;
                    case "Tuesday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+5);
                        break;
                    case "Wednesday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+4);
                        break;
                    case "Thursday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+3);
                        break;
                    case "Friday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+2);
                        break;
                    case "Saturday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+1);
                        break;
                    case "Sunday":
                        dtTempDate = dtTempStartDate;
                        break;
                }
                if (!dayString.Equals("Monday"))
                {
                    if (!dict.ContainsKey(dtTempStartDate.ToString()) && !dict.ContainsValue(dtTempDate.ToString()))
                    {
                        dict.Add(dtTempStartDate.ToString(), dtTempDate.ToString());
                    }
                    dtTempDate = dtTempDate.Date.AddDays(+1);
                    if (DateTime.Compare(dtTempDate, dtEndDate) <= 0)
                    {
                        if (!dict.ContainsKey(dtTempDate.ToString()) && !dict.ContainsValue(dtTempEndDate.ToString()))
                        {
                            dict.Add(dtTempDate.ToString(), dtTempEndDate.ToString());
                        }

                    }
                }
            }
            //多周
            else
            {
                DayOfWeek day = dtStartDate.DayOfWeek;
                string dayString = day.ToString();

                DateTime dtTempStartDate = dtStartDate;
                DateTime dtTempEndDate = dtEndDate;
                DateTime dtTempDate = DateTime.Now;

                #region 起始

                switch (dayString)
                {
                    case "Monday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+6);
                        break;
                    case "Tuesday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+5);
                        break;
                    case "Wednesday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+4);
                        break;
                    case "Thursday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+3);
                        break;
                    case "Friday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+2);
                        break;
                    case "Saturday":
                        dtTempDate = dtTempStartDate.Date.AddDays(+1);
                        break;
                    case "Sunday":
                        dtTempDate = dtTempStartDate;
                        break;
                }
                if (!dict.ContainsKey(dtTempStartDate.ToString()) && !dict.ContainsValue(dtTempDate.ToString()))
                {
                    dict.Add(dtTempStartDate.ToString(), dtTempDate.ToString());
                }

                dtTempStartDate = dtTempDate.Date.AddDays(+1);
                #endregion

                #region 结束

                day = dtEndDate.DayOfWeek;
                dayString = day.ToString();

                switch (dayString)
                {
                    case "Monday":
                        dtTempDate = dtEndDate;
                        break;
                    case "Tuesday":
                        dtTempDate = dtEndDate.Date.AddDays(-1);
                        break;
                    case "Wednesday":
                        dtTempDate = dtEndDate.Date.AddDays(-2);
                        break;
                    case "Thursday":
                        dtTempDate = dtEndDate.Date.AddDays(-3);
                        break;
                    case "Friday":
                        dtTempDate = dtEndDate.Date.AddDays(-4);
                        break;
                    case "Saturday":
                        dtTempDate = dtEndDate.Date.AddDays(-5);
                        break;
                    case "Sunday":
                        dtTempDate = dtEndDate.Date.AddDays(-6);
                        break;
                }
                if (!dict.ContainsKey(dtTempDate.ToString()) && !dict.ContainsValue(dtEndDate.ToString()))
                {
                    dict.Add(dtTempDate.ToString(), dtEndDate.ToString());
                }

                dtTempEndDate = dtTempDate.Date.AddDays(-1);

                #endregion

                int WhileCount = ((dtTempEndDate.Day - dtTempStartDate.Day) / 7);
                if (WhileCount == 0)
                {
                    if (!dict.ContainsKey(dtTempStartDate.ToString()) && !dict.ContainsValue(dtTempEndDate.ToString()))
                    {
                        dict.Add(dtTempStartDate.ToString(), dtTempEndDate.ToString());
                    }

                }
                else
                {
                    for (int i = 0; i < (WhileCount + 1); i++)
                    {
                        dtTempDate = dtTempStartDate.Date.AddDays(+6);
                        if (!dict.ContainsKey(dtTempStartDate.ToString()) && !dict.ContainsValue(dtTempDate.ToString()))
                        {
                            dict.Add(dtTempStartDate.ToString(), dtTempDate.ToString());
                        }
                        dtTempStartDate = dtTempDate.Date.AddDays(+1); ;
                    }
                }
            }
        }

        #endregion
    }
    public class WeekEntity
    {
        public string WeekNum { get; set; }
        public string ShowWeek { get; set; }
        public DateTime StartDate { get; set; }
        public string Start { get; set; }
        public string End { get; set; }

    }
}