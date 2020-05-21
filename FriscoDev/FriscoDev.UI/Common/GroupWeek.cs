using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Common
{
    public class GroupWeek
    {
        #region Month
      
        public static Dictionary<string, string> GetGroupWeekByDateRange(string strStartDate, string strEndDate)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            DateTime dtStartDate = DateTime.Parse(strStartDate);
            DateTime dtEndDate = DateTime.Parse(strEndDate);

            if (dtStartDate.Year == dtEndDate.Year && dtStartDate.Month == dtEndDate.Month && dtStartDate.Day == dtEndDate.Day)
            {
                return dict;
            }
           
            if (dtStartDate.Year == dtEndDate.Year)
            {
                GetGroupWeekByYear(dict, dtStartDate, dtEndDate);
            }
            
            else
            {
                int WhileCount = dtEndDate.Year - dtStartDate.Year;

               
                int YearDay = DateTime.IsLeapYear(dtStartDate.Year) ? 366 : 365;
                DateTime dtTempStartDate = dtStartDate;

                DateTime dtTempEndDate = dtTempStartDate.AddDays(YearDay - dtTempStartDate.DayOfYear);

                GetGroupWeekByYear(dict, dtTempStartDate, dtTempEndDate);

                for (int i = 1; i < (WhileCount + 1); i++)
                {
                   
                    YearDay = DateTime.IsLeapYear(dtTempStartDate.Year + 1) ? 366 : 365;
                    dtTempStartDate = DateTime.Parse(DateTime.Parse((dtTempStartDate.Year + 1) + "." + dtTempStartDate.Month + "." + "01").ToString("yyyy.MM.dd"));
                    dtTempEndDate = dtTempStartDate.AddDays(YearDay - dtTempStartDate.DayOfYear);

                  
                    GetGroupWeekByYear(dict, dtTempStartDate, dtTempEndDate);

                }
            }

            return dict;
        }

      
        public static void GetGroupWeekByYear(Dictionary<string, string> dict, DateTime dtStartDate, DateTime dtEndDate)
        {
           
            if ((dtEndDate.Month - dtStartDate.Month) >= 1)
            {
                int WhileCount = dtEndDate.Month - dtStartDate.Month;

               
                int MonthDay = DateTime.DaysInMonth(dtStartDate.Year, dtStartDate.Month);
                DateTime dtTempStartDate = dtStartDate;
                var startMonth = dtTempStartDate.AddDays(1 - dtTempStartDate.Day);
                DateTime dtTempEndDate = startMonth.AddMonths(1).AddDays(-1); ;

               
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
                        MonthDay = DateTime.DaysInMonth(dtTempStartDate.Year, dtTempStartDate.Month + 1);
                        dtTempStartDate = DateTime.Parse(DateTime.Parse(dtTempStartDate.Year + "." + (dtTempStartDate.Month + 1) + "." + "01").ToString("yyyy.MM.dd"));
                        dtTempEndDate = startMonth.AddMonths(1).AddDays(-1);
                    }
                   
                    GetGroupWeekByMonth(dict, dtTempStartDate, dtTempEndDate);

                }
            }
          
            else
            {
                
                GetGroupWeekByMonth(dict, dtStartDate, dtEndDate);
            }
        }

       
        public static void GetGroupWeekByMonth(Dictionary<string, string> dict, DateTime dtStartDate, DateTime dtEndDate)
        {
           
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
        
            else
            {
                DayOfWeek day = dtStartDate.DayOfWeek;
                string dayString = day.ToString();

                DateTime dtTempStartDate = dtStartDate;
                DateTime dtTempEndDate = dtEndDate;
                DateTime dtTempDate = DateTime.Now;

                #region

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

                #region 

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