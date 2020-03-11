using Application.Common;
using FriscoDev.Application.Common;
using FriscoDev.Application.Interface;
using FriscoDev.Application.Models;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using FriscoDev.UI.Common;
using PMDCellularInterface;
using PMGConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using static FriscoDev.Application.Interface.PMGDataPacketProtocol;

namespace FriscoDev.UI.Controllers
{
    public class PMGController : Controller
    {
        private readonly IPmdService _pmdService;
        private readonly IPMGConfigurationService _service;
        private readonly PMGDATABASEEntities _context;
        public PMGController(IPMGConfigurationService service, IPmdService pmdService, PMGDATABASEEntities context)
        {
            this._pmdService = pmdService;
            this._service = service;
            this._context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Quick Setup
        public ActionResult QuickSetup()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPageDisplay(int pmgInch, int actionType)
        {
            int pageType = 0;

            switch (actionType)
            {
                default:
                case 2:
                    pageType = 0;
                    break;
                case 3:
                    pageType = 1;
                    break;
                case 4:
                    pageType = 2;
                    break;
                case 7:
                    pageType = 4;
                    break;
            }

            List<SelectOption> list = new List<SelectOption>();
            string username = LoginHelper.UserName;
            var listPages = this._service.GetDisplayPagesByActionType(pmgInch, pageType, username);
            foreach (var item in listPages)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(item.PageName);
                list.Add(new SelectOption { value = item.PageName, Text = name });
            }
            return Json(list);
        }

        [HttpPost]
        public JsonResult SaveQuickSetup(QuickSetupModel model)
        {
            try
            {
                if (model == null)
                    return Json(new BaseResult(1, "Parameters error"));

                Pmd pmdModel = _pmdService.GetPmgById(model.pmgid);
                if (pmdModel == null)
                    return Json(new BaseResult(1, "The PMG does not exist"));

                var paramaterIdArray = new int[] { (int)ParamaterId.IdleDisplay,(int)ParamaterId.IdleDisplayPage,
                    (int)ParamaterId.SpeedLimit,(int)ParamaterId.SpeedLimitDisplay,(int)ParamaterId.SpeedLimitDisplayPage,(int)ParamaterId.SpeedLimitAlertAction,
                    (int)ParamaterId.AlertLimit,(int)ParamaterId.AlertLimitDisplay, (int)ParamaterId.AlertLimitDisplayPage,(int)ParamaterId.AlertLimitAlertAction};

                var paramaterIds = string.Join(",", paramaterIdArray);
                this._service.DeleteConfigurationByPmgid(model.pmgid, paramaterIds);

                List<PMGConfiguration> paramConfigureEntryList = model.ToQuickSetup();
                bool bo = SaveDB(paramConfigureEntryList);
                if (!bo)
                    return Json(new BaseResult(1, "Save failly"));

                string message = string.Empty;
                bool isSend = SendDataToServer(pmdModel.IMSI, pmdModel.PMDID, out message);
                if (isSend && string.IsNullOrEmpty(message))
                    return Json(new BaseResult(0, "Data is successfully written to PMG!"));

                return Json(new BaseResult(1, message));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, "Exception: " + e.Message));
            }

        }

        [HttpPost]
        public JsonResult GetQuickSetup()
        {
            ModelEnitity<QuickSetupModel> result = new ModelEnitity<QuickSetupModel>() { model = new QuickSetupModel() };
            var pmdModel = DeviceOptions.GetSelectedPMG();
            if (pmdModel == null)
            {
                result.code = 1;
                result.msg = "Please select a device";
                return Json(result);
            }

            var paramaterIdArray = new int[] { (int)ParamaterId.IdleDisplay,(int)ParamaterId.IdleDisplayPage,
                    (int)ParamaterId.SpeedLimit,(int)ParamaterId.SpeedLimitDisplay,(int)ParamaterId.SpeedLimitDisplayPage,(int)ParamaterId.SpeedLimitAlertAction,
                    (int)ParamaterId.AlertLimit,(int)ParamaterId.AlertLimitDisplay, (int)ParamaterId.AlertLimitDisplayPage,(int)ParamaterId.AlertLimitAlertAction};

            var paramaterIds = string.Join(",", paramaterIdArray);

            List<PMGConfiguration> list = this._service.GetConfigurationByPmgid(pmdModel.PMD_ID.ToInt(0), paramaterIds);
            if (list == null || list.Count == 0)
            {
                result.code = 1;
                result.msg = "The PMG does not configuration data";
                return Json(result);
            }

            QuickSetupModel model = new QuickSetupModel();
            model.pmgid = pmdModel.PMD_ID.ToInt(0);
            model.actionTypeIdle = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.IdleDisplay).Value.ToInt(0);
            model.pageTypeIdle = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.IdleDisplayPage).Value;

            model.pmgInch = FriscoDev.Application.Interface.PMGDataPacketProtocol.GetPMDDisplaySize(model.pageTypeIdle);

            model.limitSpeed = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.SpeedLimit).Value.ToInt(0);
            model.actionTypeLimit = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.SpeedLimitDisplay).Value.ToInt(0);
            model.pageTypeLimit = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.SpeedLimitDisplayPage).Value;
            model.alertActionLimit = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.SpeedLimitAlertAction).Value.ToInt(0);

            model.alertSpeed = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.AlertLimit).Value.ToInt(0);
            model.actionTypeAlert = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.AlertLimitDisplay).Value.ToInt(0);
            model.pageTypeAlert = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.AlertLimitDisplayPage).Value;
            model.alertActionAlert = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.AlertLimitAlertAction).Value.ToInt(0);


            model.IdlePageList = GetSelectPages(model.pageTypeIdle, model.pmgInch);
            model.LimitPageList = GetSelectPages(model.pageTypeLimit, model.pmgInch);
            model.AlertPageList = GetSelectPages(model.pageTypeAlert, model.pmgInch);

            result.code = 0;
            result.msg = "ok";
            result.model = model;
            return Json(result);
        }

        public List<SelectOption> GetSelectPages(string pageName, int displayType)
        {
            List<SelectOption> listPages = new List<SelectOption>();
            string username = LoginHelper.UserName;
            var page = this._service.GetDisplayPagesByPageName(pageName, username);
            if (page != null)
            {
                int PageType = FriscoDev.Application.Interface.PMGDataPacketProtocol.byte2Int(page.PageType);
                List<Pages> list = this._service.GetDisplayPagesByActionType(displayType, PageType, username);
                foreach (var item in list)
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(item.PageName);
                    listPages.Add(new SelectOption { value = item.PageName, Text = name });
                }
            }
            return listPages;
        }
        #endregion

        #region Configuration
        public ActionResult Configuration()
        {
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.Time = DateTime.Now.ToString("HH:mm:ss");
            return View();
        }

        [HttpPost]
        public JsonResult GetConfiguration()
        {
            ModelEnitity<ConfigurationModel> result = new ModelEnitity<ConfigurationModel>() { model = new ConfigurationModel() };
            var pmdModel = DeviceOptions.GetSelectedPMG();
            if (pmdModel == null)
            {
                result.code = 1;
                result.msg = "Please select a device";
                return Json(result);
            }

            var paramaterIdArray = new int[] { (int)ParamaterId.Clock,(int)ParamaterId.MinLimit,(int)ParamaterId.MaxLimit,(int)ParamaterId.SpeedUnit,
                    (int)ParamaterId.TemperatureUnit,(int)ParamaterId.Brightness, (int)ParamaterId.EnableMUTCDCompliance};
            var paramaterIds = string.Join(",", paramaterIdArray);

            List<PMGConfiguration> list = this._service.GetConfigurationByPmgid(pmdModel.PMD_ID.ToInt(0), paramaterIds);
            if (list == null || list.Count == 0)
            {
                result.code = 1;
                result.msg = "The PMG does not configuration data";
                return Json(result);
            }

            //DateTime pmgClock = new DateTime(DateTime.Now.Ticks - pmgConnection.currentPMGClock);

            //string Text = "PMG Clock: " + pmgClock.ToShortDateString() + ", " +
            //                      pmgClock.ToLongTimeString();

            ConfigurationModel model = new ConfigurationModel();
            model.minSpeed = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.MinLimit).Value.ToInt(0);
            model.maxSpeed = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.MaxLimit).Value.ToInt(0);
            model.speedUnit = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.SpeedUnit).Value;
            model.temperatureUnit = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.TemperatureUnit).Value;
            model.numBright = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.Brightness).Value.ToInt(0);
            model.mutcd = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.EnableMUTCDCompliance).Value;

            var dateModel = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.Clock);
            if (dateModel == null)
            {
                model.date = DateTime.Now.ToString("yyyy-MM-dd");
                model.time = DateTime.Now.ToString("HH:mm:ss");
            }

            result.code = 0;
            result.msg = "ok";
            result.model = model;
            return Json(result);
        }
        [HttpPost]
        public JsonResult SaveConfiguration(ConfigurationModel model)
        {
            try
            {
                if (model == null || model.pmgid <= 0)
                    return Json(new BaseResult(1, "Parameters error"));

                Pmd pmdModel = _pmdService.GetPmgById(model.pmgid);
                if (pmdModel == null)
                    return Json(new BaseResult(1, "The PMG does not exist"));

                var paramaterIdArray = new int[] { (int)ParamaterId.Clock,(int)ParamaterId.MinLimit,(int)ParamaterId.MaxLimit,(int)ParamaterId.SpeedUnit,
                    (int)ParamaterId.TemperatureUnit,(int)ParamaterId.Brightness, (int)ParamaterId.EnableMUTCDCompliance};
                var paramaterIds = string.Join(",", paramaterIdArray);

                this._service.DeleteConfigurationByPmgid(model.pmgid, paramaterIds);

                List<PMGConfiguration> paramConfigureEntryList = model.ToConfigurations();

                bool bo = SaveDB(paramConfigureEntryList);
                if (!bo)
                    return Json(new BaseResult(1, "Save failly"));

                string message = string.Empty;
                bool isSend = SendDataToServer(pmdModel.IMSI, pmdModel.PMDID, out message);
                if (isSend && string.IsNullOrEmpty(message))
                    return Json(new BaseResult(0, "Data is successfully written to PMG!"));

                return Json(new BaseResult(1, message));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, "Exception: " + e.Message));
            }

        }
        #endregion

        public ActionResult TextOptions()
        {
            return View();
        }

        public ActionResult Graphic()
        {
            return View();
        }

        public ActionResult Animation()
        {
            return View();
        }
        public ActionResult ScheduleOperation()
        {
            return View();
        }
        public ActionResult Communication()
        {
            return View();
        }
        public ActionResult Radar()
        {
            return View();
        }
        public ActionResult TraffocData()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FactoryReset(string sid)
        {
            try
            {
                ProtocolHelper.InitConnection();
                Thread.Sleep(1000);
                List<PMGConnection.PMGEntry> lstPMG = ProtocolHelper.GetPMGList();
                foreach (var item in lstPMG)
                {
                    string pmgName = item.name;
                    PMGConnection.PMGConnectionStatus status = item.connectinStatus;
                }
                //ProtocolHelper.ResetPMG(lstPMG[2]);
                var str = string.Join(",", lstPMG.ToArray().Select(p => p.name));
                return Json(new { code = 200, msg = $"Factory successfully {str}" });
            }
            catch (Exception e)
            {
                return Json(new { code = 400, msg = e.Message });
            }
        }

        #region method
        private bool SaveDB(List<PMGConfiguration> paramConfigureEntryList)
        {
            this._context.PMGConfiguration.AddRange(paramConfigureEntryList);
            int i = this._context.SaveChanges();
            return i > 0;
        }

        public bool SendDataToServer(string imsi, int pmgId, out string message)
        {
            message = string.Empty;
            TimeSpan dateTime = DateTime.Now - new DateTime(2000, 1, 1);
            long transactionId = (long)dateTime.TotalSeconds;
            bool bo = PMDInterface.ServerConnection.SendDataToServer(PMDConfiguration.TableID.PMG, PMDConfiguration.NotificationType.Update, transactionId, imsi);
            System.Threading.Thread.Sleep(200);
            var model = this._context.ConfigurationLog.FirstOrDefault(p => p.PMG_ID == pmgId && p.Transaction_ID == transactionId);
            if (model == null)
            {
                message = "Data is successfully written to PMG!";
                return false;
            }
            if (model.Status == 1)
            {
                message = model.Message;
                return false;
            }
            message = "";
            return true;
        }
        #endregion

    }
}
