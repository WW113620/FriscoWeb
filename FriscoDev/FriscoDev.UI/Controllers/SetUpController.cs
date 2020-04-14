using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FriscoDev.Application.Common;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using FriscoDev.UI.Common;
using FriscoDev.UI.Models;
using PMDCellularInterface;

namespace FriscoDev.UI.Controllers
{
    public class SetUpController : Controller
    {
        private readonly IPmdService _pmdService;

        public SetUpController(IPmdService pmdService)
        {
            _pmdService = pmdService;
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
            ViewData["clockDate"] = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd");
            ViewData["clockTime"] = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("HH:mm");
            return View();
        }

        #region Get Setup Info
        public string GetDeviceSetupInfo(string pId = "")
        {
            bool result = false;

            PMDConfiguration configuration = null;

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
                        TimerSetting[] timerslist = new TimerSetting[5];
                        timerslist = configuration.pmdSetting.timers;
                        for (int i = 0; i < 5; i++)
                        {
                            int daynum = Convert.ToInt32(timerslist[i].days);
                            //daynum = 128 + 4 + 2;
                            configuration.pmdSetting.timers[i].daysName = Commons.getNCiString(daynum);
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
                rows = configuration.pmdSetting
            };

            return _result.ToJson("yyyy-MM-dd HH:mm:ss");
        }
        #endregion

        #region Save Setup Info
        public string SaveDeviceSetupInfo(string pId, string timersObj, string gloalObj, string msgTopObj, string msgBottomObj)
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
                        //json
                        IList<Timers> timerList = Commons.DeserializeJsonToObject<List<Timers>>(timersObj);
                        if (timerList != null)
                        {
                            int rowsindex = 0;
                            foreach (Timers tModel in timerList)
                            {
                                configuration.pmdSetting.timers[rowsindex].functionType = (PMDCellularInterface.TimerFunctionType)Commons.GetTimerFunctionTypeEnumName(tModel.functionType);
                                configuration.pmdSetting.timers[rowsindex].modeType = (PMDCellularInterface.TimerMode)Commons.GetTimerModeEnumName(tModel.modeType);
                                configuration.pmdSetting.timers[rowsindex].startDate = tModel.startDate;
                                configuration.pmdSetting.timers[rowsindex].startTime = tModel.startTime;
                                configuration.pmdSetting.timers[rowsindex].stopDate = tModel.stopDate;
                                configuration.pmdSetting.timers[rowsindex].stopTime = tModel.stopTime;
                                int cint = Commons.getNCiNum(tModel.daysName);
                                configuration.pmdSetting.timers[rowsindex].days = Convert.ToByte(cint);
                                configuration.pmdSetting.timers[rowsindex].speedLimit = tModel.speedLimit;
                                configuration.pmdSetting.timers[rowsindex].linkToCalendar = (PMDCellularInterface.TimerCalendarControl)Commons.GetTimerCalendarControlEnumName(tModel.linkToCalendar);
                                rowsindex++;
                            }
                        }
                        bool userClock = false;
                        string clockDate = "";
                        string clockTime = "";

                        GlobalModel globalModel = Commons.DeserializeJsonToObject<GlobalModel>(gloalObj);
                        if (globalModel != null)
                        {
                            configuration.pmdSetting.minSpeed = globalModel.minSpeed;
                            configuration.pmdSetting.maxSpeed = globalModel.maxSpeed;
                            configuration.pmdSetting.alertFlashType = globalModel.alertFlashType;
                            configuration.pmdSetting.alertMessageType = globalModel.alertMessageType;
                            configuration.pmdSetting.dataFormat = globalModel.dataFormat;

                            userClock = globalModel.userClock == 1 ? true : false;
                            clockDate = globalModel.clockDate;
                            clockTime = globalModel.clockTime;
                        }

                        //msgTopObj  ["!!!!!!"," ZONE\u0000"," DOWN\u0000"]
                        msgTopObj = msgTopObj.Replace("[", "");
                        msgTopObj = msgTopObj.Replace("]", "");
                        msgTopObj = msgTopObj.Replace("\"", "");
                        configuration.pmdSetting.topMsgs = msgTopObj.Split(',');
                        msgBottomObj = msgBottomObj.Replace("[", "");
                        msgBottomObj = msgBottomObj.Replace("]", "");
                        msgBottomObj = msgBottomObj.Replace("\"", "");
                        configuration.pmdSetting.bottomMsgs = msgBottomObj.Split(',');

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
                            //PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMD, PMDConfiguration.DatabaseOperationType.ClockUpdate, pmdModel.IMSI);
                            DateTime nDate = new DateTime();
                            if (userClock)
                            {
                                nDate = Commons.getLocalTime(LoginHelper.TIME_ZONE);
                            }
                            else
                            {
                                nDate = Convert.ToDateTime(clockDate + " " + clockTime);
                            }
                            PMDConfiguration.UpdatePMDClock(pmdModel.IMSI, nDate);
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