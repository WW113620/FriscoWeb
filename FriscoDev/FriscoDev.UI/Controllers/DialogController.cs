using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FriscoDev.Application.Common;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using FriscoDev.UI.Common;

namespace FriscoDev.UI.Controllers
{
    public class DialogController : BaseController
    {
        private readonly IPmdService _pmdService;

        public DialogController(IPmdService pmdService)
        {
            _pmdService = pmdService;
        }

        public ActionResult Report(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
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
            ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");

            Pmd pmdModel = _pmdService.GetPmd(pid);

            return View(pmdModel);
        }
        // GET: Dialog
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
            ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            return PartialView();
        }

        public ActionResult SpeedCount(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
                ViewBag.PmdId = pmdid;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");

            return PartialView();
        }

        public ActionResult Pie(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
                ViewBag.PmdId = pmdid;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            return PartialView();
        }

        public ActionResult WeeklyCountTime(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
                ViewBag.PmdId = pmdid;
            }

            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            return PartialView();
        }

        public ActionResult EnforcementSchedule(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
                ViewBag.PmdId = pmdid;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");

            return PartialView();
        }

        public ActionResult Index(string xvalue = "0", string yvalue = "0", string pid = "", int pmdid = 0, string startDate = "", string endDate = "")
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
                ViewBag.PmdId = pmdid;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                ViewBag.StartDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd" + " 00:00");
            }
            else
            {
                ViewBag.StartDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd" + " 00:00");
            }
            ViewBag.EndDate = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            return PartialView();
        }

        public ActionResult PartWeekList(string start, string end)
        {
            ViewBag.WeekStartDate = start;
            ViewBag.WeekEndDate = end;
            return PartialView();
        }

    }
}