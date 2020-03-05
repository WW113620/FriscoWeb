using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FriscoDev.Application.Common;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using FriscoDev.UI.Common;
using FriscoDev.UI.Models;
using PMDCellularInterface;

namespace FriscoDev.UI.Controllers
{
    public class CalendarController : Controller
    {
        private readonly IPmdService _pmdService;
        private readonly IUserService _userService;

        public CalendarController(IPmdService pmdService, IUserService userService)
        {
            _pmdService = pmdService;
            _userService = userService;
        }

        public ActionResult Index(decimal xvalue = 0, decimal yvalue = 0, string pid = "", int pmdid = 0)
        {
            if (pmdid > 0)
            {
                System.Web.HttpContext.Current.Session["curpmdid"] = xvalue + "," + yvalue + "," + pid + "," + pmdid;
            }
            if (System.Web.HttpContext.Current.Session["curpmdid"] != null)
            {
                string str = Convert.ToString(System.Web.HttpContext.Current.Session["curpmdid"]);
                string[] arr = str.Split(',');
                ViewBag.xvalue = Convert.ToDecimal(arr[0]);
                ViewBag.yvalue = Convert.ToDecimal(arr[1]);
                ViewBag.pid = Convert.ToString(arr[2]);
                ViewBag.PmdId = Convert.ToInt32(arr[3]);
            }
            else
            {
                var pmd = _pmdService.GetDevicesByCustomerId(LoginHelper.CS_ID).FirstOrDefault();
                if (pmd != null)
                {
                    ViewBag.XValue = Commons.splitStringToDecimal(pmd.Location)[0];
                    ViewBag.YValue = Commons.splitStringToDecimal(pmd.Location)[1];
                    ViewBag.PId = pmd.IMSI;
                    ViewBag.PmdId = pmd.PMDID;
                }
            }
            ViewData["startDate"] = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd");
            ViewData["endDate"] = Commons.getLocalTime(LoginHelper.TIME_ZONE).AddMonths(1).ToString("yyyy-MM-dd");
            return View();
        }


        #region Get Calendar Info
        public string GetDeviceCalendarInfo(string pId = "")
        {
            bool result = false;

            PMDConfiguration configuration = null;

            string dateArray = "";
            Pmd pmdModel = _pmdService.GetPmd(pId);
            if (pmdModel != null)
            {

                byte[] configurationData = null;
                if (pmdModel.NewConfiguration != null)
                {
                    configurationData = pmdModel.NewConfiguration;
                }
                else
                {
                    configurationData = pmdModel.CurrentConfiguration;
                }
                //byte[] configurationData = pmdModel.CurrentConfiguration;
                result = true;
                if (configurationData != null)
                {
                    configuration = new PMDConfiguration(configurationData);

                    if (configuration != null)
                    {

                        List<DateTime> _listTime = new List<DateTime>();
                        _listTime = configuration.pmdCalenar.GetAllEventDays();
                        if (_listTime != null)
                        {
                            foreach (DateTime dtime in _listTime)
                            {
                                if (dateArray == "")
                                {
                                    dateArray = dtime.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    dateArray = dateArray + "," + dtime.ToString("yyyy-MM-dd");
                                }
                            }
                        }
                    }
                }
                else
                {
                    configuration = new PMDConfiguration();
                }
            }

            var _result = new
            {
                Success = result,
                rows = dateArray
            };

            return _result.ToJson("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region Save Calendar Info
        public string SaveDeviceCalendarInfo(string pId, string calendarObj)
        {
            bool result = false;

            PMDConfiguration configuration = null;

            Pmd pmdModel = _pmdService.GetPmd(pId);
            if (pmdModel != null)
            {
                byte[] configurationData = pmdModel.CurrentConfiguration;
                if (configurationData != null)
                {
                    configuration = new PMDConfiguration(configurationData);

                    if (configuration != null)
                    {
                        result = true;
                    }

                    if (configuration != null)
                    {
                        //解析json
                        CalendarVm calendarModel = Commons.DeserializeJsonToObject<CalendarVm>(calendarObj);
                        if (calendarModel != null)
                        {
                            int isend = calendarModel.isweekend;

                            List<DateTime> eventDays = new List<DateTime> { };
                            string strtemp = calendarModel.calendararray;
                            string[] arr = strtemp.Split(',');
                            foreach (string strin in arr)
                            {
                                if (!string.IsNullOrEmpty(strin))
                                {
                                    //0不包含周末
                                    if (isend <= 0)
                                    {
                                        if (!Commons.CheckDateIsWorkend(Convert.ToDateTime(strin)))
                                        {
                                            eventDays.Add(Convert.ToDateTime(strin));
                                        }
                                    }
                                    else
                                    {
                                        eventDays.Add(Convert.ToDateTime(strin));
                                    }
                                }
                            }
                            configuration.pmdCalenar.SetAllEventDays(eventDays);
                        }


                        byte[] newData = configuration.ToData();

                        if (newData != null)
                        {
                            Pmd pmdNewModel = new Pmd();
                            pmdNewModel = pmdModel;
                            pmdNewModel.NewConfiguration = newData;
                            pmdNewModel.NewConfigurationTime = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                            _pmdService.UpdatePmd(pmdNewModel);

                            // Notify the change of configuration
                            PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMG, PMDConfiguration.DatabaseOperationType.ConfigurationUpdate, pmdModel.IMSI);
                        }
                    }
                }
            }

            var _result = new
            {
                Success = result
            };

            return _result.ToJson("yyyy-MM-dd HH:mm:ss");
        }
        #endregion
    }
}