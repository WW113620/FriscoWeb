using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FriscoDev.Application.Common;
using FriscoDev.Data.Services;
using FriscoDev.UI.Common;
using FriscoDev.UI.Models;

namespace FriscoDev.UI.Controllers
{
    public class StatsLogController : BaseController
    {
        private readonly IStatsLogService _statsLogService;
        private readonly IPmdService _pmdService;

        public StatsLogController(IStatsLogService statsLogService, IPmdService pmdService)
        {
            _statsLogService = statsLogService;
            _pmdService = pmdService;
        }

        public ActionResult Index(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).AddHours(-1).ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                ViewBag.StartDate = Commons.getLocalTime(Convert.ToDateTime(startDate)).ToString("yyyy-MM-dd HH:mm");
            }
            if (string.IsNullOrEmpty(endDate))
            {
                ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                ViewBag.EndDate = Commons.getLocalTime(Convert.ToDateTime(endDate)).ToString("yyyy-MM-dd HH:mm");
            }
            return View();
        }

        //Real Time Chars
        public JsonNetResult DeviceCharts(int id, string startDate = "", string endDate = "")
        {
            if (!string.IsNullOrEmpty(startDate))
            {
                startDate = Commons.getLocalTime(Convert.ToDateTime(startDate)).ToString("yyyy-MM-dd HH:mm");
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                endDate = Commons.getLocalTime(Convert.ToDateTime(endDate)).ToString("yyyy-MM-dd HH:mm");
            }
            var statsLogs = _statsLogService.GetStatsLogsToRealTimeCharts(id, startDate, endDate, LoginHelper.TIME_ZONE).OrderByDescending(x => x.Timestamp);

            DateTime dt1 = Commons.getLocalTime(Convert.ToDateTime(startDate));
            DateTime dt2 = Commons.getLocalTime(Convert.ToDateTime(endDate));

            TimeSpan ts = dt2.Subtract(dt1).Duration();
            int _day = ts.Hours;
            _day = _day > 0 ? _day : 1;

            if (statsLogs.Any())
            {
                var groupBySpeed = statsLogs.GroupBy(s => s.AverageSpeed);
                var chartData = new List<List<double>>();
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

                foreach (var item in groupBySpeed)
                {
                    var list = new List<double> { item.Key };
                    var groupByDirection = item.GroupBy(x => x.Direction).ToDictionary(g => g.Key, g => g.ToList());


                    list.Add(groupByDirection.ContainsKey("CLOS") ? groupByDirection["CLOS"].Count : 0);
                    list.Add(groupByDirection.ContainsKey("AWAY") ? groupByDirection["AWAY"].Count : 0);
                    chartData.Add(list);
                }

                return new JsonNetResult
                {
                    Data = new
                    {
                        Success = true,
                        ViewData = new HomeIndexViewModel
                        {
                            ChartData = chartData,
                            Report = reportData
                        }
                    }
                };
            }

            return new JsonNetResult { Data = new { Success = false } };
        }
    }
}