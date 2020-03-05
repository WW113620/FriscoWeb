using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Common;
using FriscoDev.Application.Common;
using FriscoDev.Application.Models;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using FriscoDev.UI.Common;
using FriscoDev.UI.Models;
using TimeZoneInfo = System.TimeZoneInfo;

namespace FriscoDev.UI.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IStatsLogService _statsLogService;

        public ReportController(IStatsLogService statsLogService)
        {
            _statsLogService = statsLogService;
        }

        public ActionResult Index(decimal xvalue = 0, decimal yvalue = 0, string pid = "", int pmdid = 0)
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
            }

            var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(LoginHelper.TIME_ZONE));
            string Start = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm");
            string End = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm");
            ViewBag.StartDate = Start;
            ViewBag.EndDate = End;

            return View();
        }


        [HttpPost]
        public JsonResult GetStatsLogsToDataLog(string pmdId, string startTime, string endTime, int pageIndex, int pageSize = 15)
        {
            long iCount = 0;
            if (string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(endTime))
            {
                var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(LoginHelper.TIME_ZONE));
                string Start = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0).ToString("yyyy-MM-dd HH:mm");
                string End = new DateTime(today.Year, today.Month, today.Day, 23, 59, 59).ToString("yyyy-MM-dd HH:mm");
                if (string.IsNullOrEmpty(startTime))
                    startTime = Start;
                if (string.IsNullOrEmpty(endTime))
                    endTime = End;
            }

            var model = _statsLogService.GetStatsLogsToDataLog(startTime, endTime, pageIndex, pageSize, pmdId.ToInt(0), LoginHelper.TIME_ZONE, out iCount);
            foreach (var item in model)
            {
                item.StrTimestamp = item.Timestamp.ToString("yyyy-MM-dd HH:mm");
                item.Direction = item.Direction != "AWAY" ? "CLOSE" : "AWAY";
            }
            #region 页面分页
            int pageCount = (int)Math.Ceiling(iCount / Convert.ToDouble(pageSize));
            if (pageCount > 0 && pageCount < pageIndex)
            {
                pageIndex = pageCount;
            }
            #endregion
            return Json(new { errorCode = 200, list = model, Count = iCount, PageCount = pageCount, PageIndex = pageIndex });
        }

        #region Time vs Count
        public ActionResult TimeCount(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            if (string.IsNullOrEmpty(endDate))
            {
                ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            }
            else
            {
                ViewBag.EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd HH:mm");
            }
            DateTime now = DateTime.Now.Date;
            //ViewBag.start = now.AddDays(1 - now.Day);
            ViewBag.start = now.AddMonths(-1);
            ViewBag.end = now;
            return View();
        }

        public string DeviceTimeCountCharts(int id, string startDate = "", string endDate = "")
        {

            List<CharModel> _lst = new List<CharModel>();
            try
            {
                var statsLogs = _statsLogService.GetStatsLogsToRealTimeCharts(id, startDate, endDate, LoginHelper.TIME_ZONE);

                if (statsLogs.Any())
                {
                    var minDate = string.IsNullOrEmpty(startDate) ? DateTime.Now.AddDays(-1) : Convert.ToDateTime(startDate);
                    var maxDate = string.IsNullOrEmpty(endDate) ? DateTime.Now : Convert.ToDateTime(endDate);
                    TimeSpan ts = maxDate.Subtract(minDate);//时间差 
                    var totalMinutes = ts.Days * 24 * 60 + ts.Hours * 60 + ts.Minutes;//分钟为单位
                    var eval = Math.Round((double)totalMinutes / 96);
                    IEnumerable<StatsLogVm> statsSub = new List<StatsLogVm>();
                    for (DateTime dt = minDate; dt < maxDate; dt = dt.AddMinutes(eval))
                    {
                        string curDate = dt.ToString("yyyy-MM-dd HH:mm");
                        CharModel rModel = new CharModel();
                        rModel.Date = Convert.ToDateTime(curDate);
                        if (dt == minDate)
                        {
                            statsSub = statsLogs.Where(p => p.Timestamp <= dt);
                        }
                        else
                        {
                            statsSub = statsLogs.Where(p => p.Timestamp > dt.AddMinutes(-eval) && p.Timestamp <= dt);
                        }
                        rModel.CLOS = statsSub.Where(p => p.Direction.Equals("CLOS")).Count();
                        rModel.AWAY = statsSub.Where(p => p.Direction.Equals("AWAY")).Count();
                        _lst.Add(rModel);
                    }

                }

            }
            catch (Exception ex)
            {
                _lst = null;
            }
            return _lst.ToJson();
        }

        public class CharModel
        {
            public DateTime Date { get; set; }
            public double CLOS { get; set; }
            public double AWAY { get; set; }
        }
        #endregion

        #region Pie

        public ActionResult Pie(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            if (string.IsNullOrEmpty(endDate))
            {
                ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            }
            else
            {
                ViewBag.EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd HH:mm");
            }

            return View();
        }

        public string DevicePieCharts(int id, string startDate = "", string endDate = "", int speedLimit = 35)
        {

            List<CharModelPie> _lst = new List<CharModelPie>();
            try
            {
                endDate = endDate + "";
                var statsLogs = _statsLogService.GetStatsLogsToReport(id, startDate, endDate, LoginHelper.TIME_ZONE).OrderBy(x => x.Timestamp);

                if (statsLogs.Any())
                {
                    //Strength
                    var groupBySpeed = statsLogs.GroupBy(s => s.Strength);

                    DateTime dtStart = Convert.ToDateTime(startDate);
                    DateTime dtEnd = Convert.ToDateTime(endDate);

                    CharModelPie rModel = new CharModelPie();
                    rModel.ModelType = "Normal";
                    rModel.ModelValue = statsLogs.Count(s => s.PeakSpeed <= speedLimit);
                    _lst.Add(rModel);

                    CharModelPie rModel2 = new CharModelPie();
                    rModel2.ModelType = "Moderately";
                    rModel2.ModelValue = statsLogs.Count(s => s.PeakSpeed > speedLimit && s.PeakSpeed <= speedLimit + 10);
                    _lst.Add(rModel2);

                    CharModelPie rModel3 = new CharModelPie();
                    rModel3.ModelType = "Excessively ";
                    rModel3.ModelValue = statsLogs.Count(s => s.PeakSpeed > speedLimit + 10);
                    _lst.Add(rModel3);

                }

            }
            catch (Exception ex)
            {
                _lst = null;
            }
            return _lst.ToJson();
        }

        public class CharModelPie
        {
            public string ModelType { get; set; }
            public double ModelValue { get; set; }
        }
        #endregion

        #region Speed vs Count
        public ActionResult SpeedCount(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            if (string.IsNullOrEmpty(endDate))
            {
                ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            }
            else
            {
                ViewBag.EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd HH:mm");
            }

            return View();
        }
        //lidar@gmail.com  a1b2c3
        public JsonResult DeviceSpeedCountCharts(int id, string startDate, string endDate, int speedLimit = 35)
        {
            EchartsData<ResultData<int[]>> resultList = new EchartsData<ResultData<int[]>>() { ResultCode = 500 };
            try
            {
                List<SpeedCount> list = new List<SpeedCount>();
                double aveg = 0;
                list = _statsLogService.GetSpeedCountFromStatsLogs(id, startDate, endDate, LoginHelper.TIME_ZONE, out aveg).ToList();
                if (list != null && list.Count > 0)
                {
                    var arrXData = list.Select(x => x.AverageSpeed.ToString()).ToArray();
                    resultList = new EchartsData<ResultData<int[]>>()
                    {
                        ResultCode = 200,
                        xAxisData = arrXData,
                        series = new List<ResultData<int[]>>
                        {
                          new ResultData<int[]>(){  name="Closing", data=list.Select(y=>y.ClosCount).ToArray()},
                           new ResultData<int[]>(){ name="Away", data=list.Select(y=>y.AwayCount).ToArray()}
                        }
                    };
                }
            }
            catch (Exception ex)
            {
            }
            return Json(resultList);
        }

        public JsonResult DeviceCharts(int id, string startDate = "", string endDate = "")
        {
            if (string.IsNullOrEmpty(startDate))
                startDate = DateTime.Now.AddDays(-1).ToString();
            if (string.IsNullOrEmpty(endDate))
                endDate = DateTime.Now.ToString();

            EchartsData<ResultData<int[]>> resultList = new EchartsData<ResultData<int[]>>() { ResultCode = 500, ReportData = new ReportData() };
            var statsLogs = _statsLogService.GetStatsLogsToRealTimeCharts(id, startDate, endDate, LoginHelper.TIME_ZONE).OrderByDescending(x => x.Timestamp);

            DateTime dt1 = Convert.ToDateTime(startDate);
            DateTime dt2 = Convert.ToDateTime(endDate);

            TimeSpan ts = dt2.Subtract(dt1).Duration();
            int _day = ts.Hours;
            _day = _day > 0 ? _day : 1;

            if (statsLogs.Any())
            {
                var groupBySpeed = statsLogs.OrderBy(s => s.AverageSpeed).GroupBy(s => s.AverageSpeed);
                int _count = statsLogs.Count();
                var reportData = new ReportData
                {
                    TotalCount = statsLogs.Count(),
                    Closing = statsLogs.Count(s => s.Direction == "CLOS"),
                    Away = statsLogs.Count(s => s.Direction == "AWAY"),
                    Average = Math.Round(statsLogs.Sum(s => s.AverageSpeed) / statsLogs.Count()),
                    Last = statsLogs.Min(s => s.LastSpeed),
                    HighAlam = statsLogs.Max(s => s.PeakSpeed),
                    LowAlam = statsLogs.Min(s => s.PeakSpeed),
                    AverageTotal = _count / _day,
                    LastDate = statsLogs.Max(s => s.Timestamp).ToString("yyyy-MM-dd HH:mm"),
                };

                List<int> closList = new List<int>();
                List<int> awayList = new List<int>();
                foreach (var item in groupBySpeed)
                {
                    var groupByDirection = item.GroupBy(x => x.Direction).ToDictionary(g => g.Key, g => g.ToList());
                    closList.Add(groupByDirection.ContainsKey("CLOS") ? groupByDirection["CLOS"].Count : 0);
                    awayList.Add(groupByDirection.ContainsKey("AWAY") ? groupByDirection["AWAY"].Count : 0);
                }
                resultList = new EchartsData<ResultData<int[]>>()
                {
                    ResultCode = 200,
                    ReportData = reportData,
                    xAxisData = groupBySpeed.Select(item => item.Key.ToString()).ToArray(),
                    series = new List<ResultData<int[]>>
                    {
                        new ResultData<int[]>(){  name="Closing", data=closList.ToArray()},
                        new ResultData<int[]>(){ name="Away", data=awayList.ToArray()}
                    }
                };
            }
            return Json(resultList);
        }

        public class EchartsData<T>
        {
            public int ResultCode { get; set; }//200 成功  500 失败 
            public string ResultEsg { get; set; }//200 成功  500 失败 
            public ReportData ReportData { get; set; }
            public string[] xAxisData { get; set; }
            public int[] xIntAxisData { get; set; }
            public List<T> series { get; set; }
        }

        public class ResultData<T>
        {
            public string name { get; set; }
            public T data { get; set; }
        }

        #endregion

        #region Count vs Speed Percentile
        public ActionResult CountSpeedPercentile(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            if (string.IsNullOrEmpty(endDate))
            {
                ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            }
            else
            {
                ViewBag.EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd HH:mm");
            }
            return View();
        }

        public JsonResult GetSpeedPercentile(int id, string startDate = "", string endDate = "")
        {
            int calculation = 0;
            Dictionary<int, int> PercentitleCollection = new Dictionary<int, int>();
            int i85PerSpeed = 0;
            int inumInPace = 0;
            int i10MphPaceSpeed = 0;
            int sumVehicleCount = 0;
            int sumVehicleSpeed = 0;
            double averageSpeed = 0;
            int i85thPercntActualCount = 0;
            List<int> lisSpeed = new List<int>();
            List<int> listCount = new List<int>();
            EchartsData<ResultData<int[]>> resultList = new EchartsData<ResultData<int[]>>() { ResultCode = 500 };
            try
            {
                List<CountPercentileEntity> list = new List<CountPercentileEntity>();
                list = _statsLogService.GetSpeedPercentileList(id, startDate, endDate, LoginHelper.TIME_ZONE).ToList();
                if (list != null && list.Count > 0)
                {
                    sumVehicleCount = list.Select(x => x.iCount).Sum();
                    sumVehicleSpeed = list.Select(x => x.AverageSpeed).Sum();
                    if (sumVehicleCount > 0)
                    {
                        calculation = sumVehicleSpeed / sumVehicleCount;
                    }
                    PercentitleCollection.Clear();

                    foreach (var item in list)
                    {
                        if (!PercentitleCollection.ContainsKey(item.AverageSpeed))
                        {
                            PercentitleCollection.Add(item.AverageSpeed, item.iCount);
                        }
                        else
                        {
                            int value = 0;
                            if (PercentitleCollection.TryGetValue(item.AverageSpeed, out value))
                            {
                                PercentitleCollection[item.AverageSpeed] = value + item.iCount;
                            }
                        }
                    }
                    Calculate85PercentileSpeed(PercentitleCollection, (int)(sumVehicleCount * 0.85),
                  ref i85thPercntActualCount,
                  calculation, ref i85PerSpeed, ref inumInPace, ref i10MphPaceSpeed);
                    int yMaxValue = 0;
                    float percentY = 0;

                    foreach (var item in PercentitleCollection.OrderBy(i => i.Key))
                    {
                        yMaxValue += item.Value;
                        percentY = ((float)yMaxValue * 100) / sumVehicleCount;// currentCollection.iTotalVehicleCount;
                        lisSpeed.Add(item.Key);
                        listCount.Add((int)percentY);
                    }
                    var statsLogs = _statsLogService.GetStatsLogsToRealTimeCharts(id, startDate, endDate, LoginHelper.TIME_ZONE).OrderByDescending(x => x.Timestamp);

                    if (statsLogs.Any())
                    {
                        averageSpeed = Math.Round(statsLogs.Sum(s => s.AverageSpeed) / statsLogs.Count());
                    }
                }
            }
            catch (Exception ex)
            {
                resultList.ResultCode = 300;
            }
            resultList = new EchartsData<ResultData<int[]>>()
            {
                ResultCode = 200,
                xIntAxisData = lisSpeed.Select(x => x).ToArray(),
                ReportData = new ReportData() { HighAlam = i85PerSpeed, Average = averageSpeed, },
                series = new List<ResultData<int[]>>
                        {
                            new ResultData<int[]>(){  name="Count vs th Percentile Speed", data=listCount.ToArray()  }
                        }
            };
            return Json(resultList);
        }

        public static void Calculate85PercentileSpeed(Dictionary<int, int> PercentitleCollection, Int32 i85thPercntPredictionCount, ref int i85thPercntActualCount, float averageSpeed, ref int i85PerSpeed, ref int inumInPace, ref int i10MphPaceSpeed)
        {
            //Calculate the 10mph Pace. based on 
            //http://www.ehow.com/how_8458412_do-calculate-85th-percentile-speed.html

            Int32 i85thCount = 0;
            //10 mph Pace
            int pace10SpeedTemp = 0;
            int pace10CountTemp = 0;

            float AveSpeedTemp = averageSpeed > 4 ? averageSpeed - 4 : 0;
            int i85Percent = 0;
            int pace10Count = 0;
            foreach (var item in PercentitleCollection.OrderBy(i => i.Key))
            {
                pace10SpeedTemp = item.Key;
                pace10CountTemp = item.Value;
                if (pace10SpeedTemp >= AveSpeedTemp && pace10SpeedTemp <= AveSpeedTemp + 9)
                {
                    pace10Count += pace10CountTemp;
                }

                i85thCount += item.Value;
                if (i85thCount >= i85thPercntPredictionCount && i85Percent == 0)
                {
                    i85thPercntActualCount = i85thCount;
                    i85Percent = pace10SpeedTemp;
                }
            }
            i85PerSpeed = i85Percent;
            inumInPace = pace10Count;
            i10MphPaceSpeed = (int)AveSpeedTemp;
        }
        #endregion

        #region Weekly Count vs Time
        public ActionResult WeeklyCountTime(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
            }

            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            if (string.IsNullOrEmpty(endDate))
            {
                ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            }
            else
            {
                ViewBag.EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd HH:mm");
            }
            return View();
        }

        public JsonResult GetWeeklyCountTime(int id, string startDate = "", string endDate = "")
        {
            EchartsData<ResultData<int[]>> resultList = new EchartsData<ResultData<int[]>>() { ResultCode = 500 };
            try
            {
                List<SpeedPercentileEntity> list = new List<SpeedPercentileEntity>();
                var listWeek = _statsLogService.GetStatsLogListNew(id, startDate, endDate, LoginHelper.TIME_ZONE).OrderBy(p => p.TimeSplit);
                if (listWeek.Any())
                {

                    var Sunday = listWeek.Where(p => p.WeekDate == 1).GroupBy(p => p.TimeSplit).Select(g => new { name = g.Key, count = g.Count() });
                    var Monday = listWeek.Where(p => p.WeekDate == 2).GroupBy(p => p.TimeSplit).Select(g => new { name = g.Key, count = g.Count() });
                    var Tuesday = listWeek.Where(p => p.WeekDate == 3).GroupBy(p => p.TimeSplit).Select(g => new { name = g.Key, count = g.Count() });
                    var Wednesday = listWeek.Where(p => p.WeekDate == 4).GroupBy(p => p.TimeSplit).Select(g => new { name = g.Key, count = g.Count() });
                    var Thursday = listWeek.Where(p => p.WeekDate == 5).GroupBy(p => p.TimeSplit).Select(g => new { name = g.Key, count = g.Count() });
                    var Friday = listWeek.Where(p => p.WeekDate == 6).GroupBy(p => p.TimeSplit).Select(g => new { name = g.Key, count = g.Count() });
                    var Saturday = listWeek.Where(p => p.WeekDate == 7).GroupBy(p => p.TimeSplit).Select(g => new { name = g.Key, count = g.Count() });

                    resultList = new EchartsData<ResultData<int[]>>()
                    {
                        ResultCode = 200,
                        xAxisData = listWeek.Select(x => x.TimeSplit).Distinct().ToArray(),
                        series = new List<ResultData<int[]>>
                        {
                          new ResultData<int[]>(){  name="Monday", data=Monday.Select(y=>y.count).ToArray()},
                          new ResultData<int[]>(){  name="Tuesday", data=Tuesday.Select(y=>y.count).ToArray()},
                          new ResultData<int[]>(){  name="Wednesday", data=Wednesday.Select(y=>y.count).ToArray()},
                          new ResultData<int[]>(){  name="Thursday", data=Thursday.Select(y=>y.count).ToArray()},
                          new ResultData<int[]>(){  name="Friday", data=Friday.Select(y=>y.count).ToArray()},
                          new ResultData<int[]>(){  name="Saturday", data=Saturday.Select(y=>y.count).ToArray()},
                          new ResultData<int[]>(){  name="Sunday", data=Sunday.Select(y=>y.count).ToArray()}
                        }
                    };
                }
                else
                {
                    resultList.ResultCode = 300;//无数据
                }

            }
            catch (Exception ex)
            {
            }
            return Json(resultList);
        }

        public JsonResult GetWeeklyCountTime_1(int id, string startDate = "", string endDate = "")
        {
            EchartsData<ResultData<int[]>> resultList = new EchartsData<ResultData<int[]>>() { ResultCode = 500 };
            try
            {
                List<SpeedPercentileEntity> list = new List<SpeedPercentileEntity>();
                list = _statsLogService.GetWeeklyCountTimeList(id, startDate, endDate, LoginHelper.TIME_ZONE).ToList();
                if (list != null && list.Count > 0)
                {
                    resultList = new EchartsData<ResultData<int[]>>()
                    {
                        ResultCode = 200,
                        xAxisData = list.Select(x => x.TimeSplit).ToArray(),
                        series = new List<ResultData<int[]>>
                        {
                          new ResultData<int[]>(){  name="Monday", data=list.Select(y=>y.MondayCount).ToArray()},
                          new ResultData<int[]>(){  name="Tuesday", data=list.Select(y=>y.TuesdayCount).ToArray()},
                          new ResultData<int[]>(){  name="Wednesday", data=list.Select(y=>y.WednesdayCount).ToArray()},
                          new ResultData<int[]>(){  name="Thursday", data=list.Select(y=>y.ThursdayCount).ToArray()},
                          new ResultData<int[]>(){  name="Friday", data=list.Select(y=>y.FridayCount).ToArray()},
                          new ResultData<int[]>(){  name="Saturday", data=list.Select(y=>y.SaturdayCount).ToArray()},
                          new ResultData<int[]>(){  name="Sunday", data=list.Select(y=>y.SundayCount).ToArray()}
                        }
                    };
                }
            }
            catch (Exception ex)
            {
            }
            return Json(resultList);
        }

        #endregion

        #region Average Speed vs Time
        public ActionResult AverageSpeedTime(decimal xvalue = 0, decimal yvalue = 0, string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            if (string.IsNullOrEmpty(endDate))
            {
                ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            }
            else
            {
                ViewBag.EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd HH:mm");
            }
            return View();
        }

        [HttpPost]
        public JsonResult AverageVSPercentitleSpeed(int id, string startDate = "", string endDate = "")
        {
            EchartsData<ResultData<double[]>> resultList = new EchartsData<ResultData<double[]>>() { ResultCode = 500 };
            List<ResultModel> result = new List<ResultModel>();
            int eval = 855;//14h 15m
            var statsData = _statsLogService.GetStatsLogsToRealTimeCharts(id, startDate, endDate, LoginHelper.TIME_ZONE);

            if (statsData.Any())
            {
                var minDate = Convert.ToDateTime(startDate);
                var maxDate = Convert.ToDateTime(endDate);
                for (DateTime dt = minDate; dt < maxDate; dt = dt.AddMinutes(eval))
                {
                    string curDate = dt.ToString("MM-dd HH:mm");
                    ResultModel model = new ResultModel();
                    model.Date = curDate;
                    if (dt == minDate)
                    {
                        model.Average = 0;
                        model.Percentile = 0;
                    }
                    else
                    {
                        double deveCount = statsData.Where(t => t.Timestamp >= dt.AddMinutes(-eval) && t.Timestamp <= dt).Sum(s => s.AverageSpeed);
                        int iCount = statsData.Where(t => t.Timestamp >= dt.AddMinutes(-eval) && t.Timestamp <= dt).Count();

                        model.Average = iCount > 0 ? Math.Round(deveCount / iCount) : 0;
                        var staList = statsData.Where(t => t.Timestamp >= dt.AddMinutes(-eval) && t.Timestamp <= dt).ToList();
                        model.Percentile = ComPerentileSpeed(staList);
                    }
                    result.Add(model);
                }
            }
            resultList = new EchartsData<ResultData<double[]>>()
            {
                ResultCode = 200,
                xAxisData = result.Select(item => item.Date).ToArray(),
                series = new List<ResultData<double[]>>
                    {
                        new ResultData<double[]>(){  name="Average Speed vs Time", data=result.Select(p=>p.Average).ToArray()},
                        new ResultData<double[]>(){ name="85 Percentile Speed vs Time", data=result.Select(p=>p.Percentile).ToArray()}
                    }
            };
            return Json(resultList);
        }
        public static double ComPerentileSpeed(List<StatsLogVm> list)
        {
            int i85PerSpeed = 0;
            int inumInPace = 0;
            int i10MphPaceSpeed = 0;
            int i85thPercntActualCount = 0;
            Dictionary<int, int> PercentitleCollection = new Dictionary<int, int>();
            if (list != null && list.Count > 0)
            {
                double calculation = 0;
                int sumVehicleCount = list.Count();
                double sumVehicleSpeed = list.Sum(s => s.AverageSpeed);
                if (sumVehicleCount > 0)
                {
                    calculation = sumVehicleSpeed / sumVehicleCount;
                }
                PercentitleCollection.Clear();
                foreach (var item in list)
                {
                    int average = (int)item.AverageSpeed;
                    if (!PercentitleCollection.ContainsKey(average))
                    {
                        PercentitleCollection.Add(average, 1);
                    }
                    else
                    {
                        int value = 0;
                        if (PercentitleCollection.TryGetValue(average, out value))
                        {
                            PercentitleCollection[average] = value + 1;
                        }
                    }
                }
                Calculate85PercentileSpeed(PercentitleCollection, (int)(sumVehicleCount * 0.85),
             ref i85thPercntActualCount,
             (float)calculation, ref i85PerSpeed, ref inumInPace, ref i10MphPaceSpeed);
            }
            return i85PerSpeed;
        }
        public class ResultModel
        {
            public string Date { get; set; }
            public double Average { get; set; }
            public double Percentile { get; set; }
        }
        #endregion

        #region Enforcement Schedule
        public ActionResult EnforcementSchedule(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            if (string.IsNullOrEmpty(endDate))
            {
                ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            }
            else
            {
                ViewBag.EndDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd HH:mm");
            }
            return View();
        }

        public ActionResult GetSurveyData(int id = 0, string pid = "", string startDate = "", string endDate = "")
        {
            List<SurveyDataEntity> surList = new List<SurveyDataEntity>();
            try
            {
                string Location = string.Empty;
                var pmdService = new PmdService();
                Pmd pmdModel = pmdService.GetPmd(pid);
                if (pmdModel != null)
                    Location = pmdModel.Location;
                surList = _statsLogService.GetSurveyDataList(id, startDate, endDate, LoginHelper.TIME_ZONE).ToList();
                if (surList != null && surList.Count > 0)
                {
                    foreach (var item in surList)
                    {
                        item.SurveyDate = Commons.GetConvertWeekday(item.weeddate);
                        if (item.weeddate == 1)
                            item.weeddate = item.weeddate + 7;
                    }
                    surList = surList.OrderBy(p => p.weeddate).ToList();
                }
                ViewBag.Location = Location;
            }
            catch (Exception e)
            {

            }
            return PartialView(surList);
        }

        #endregion


        #region Method
        [HttpPost]
        public JsonResult GroupWeekMethod(string start, string end)
        {
            List<WeekEntity> list = new List<WeekEntity>();
            if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
            {
                return Json(new { errorCode = 200, list = list });
            }

            Dictionary<string, string> dict = GroupWeek.GetGroupWeekByDateRange(start, end);
            WeekEntity en = null;
            foreach (var item in dict)
            {
                en = new WeekEntity();
                en.StartDate = Convert.ToDateTime(item.Key);
                en.Start = en.StartDate.ToString("yyyy-MM-dd");
                en.End = Convert.ToDateTime(item.Value).ToString("yyyy-MM-dd");
                en.ShowWeek = string.Format("{0}/{1}", en.Start, en.End);
                list.Add(en);
            }
            list = list.OrderBy(p => p.StartDate).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].WeekNum = string.Format("Week {0}", i + 1);
            }
            return Json(new { errorCode = 200, list = list });
        }
        #endregion
    }
}