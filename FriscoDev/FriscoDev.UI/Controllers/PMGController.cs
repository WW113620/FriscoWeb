using Application.Common;
using FriscoDev.Application.Common;
using FriscoDev.Application.Interface;
using FriscoDev.Application.Models;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using FriscoDev.UI.Attribute;
using FriscoDev.UI.Common;
using FriscoDev.UI.Utils;
using PMDCellularInterface;
using PMDInterface;
using PMGConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using static FriscoDev.Application.Interface.PacketProtocol;

namespace FriscoDev.UI.Controllers
{
    [CheckLogin]
    public class PMGController : Controller
    {

        private readonly IPmdService _pmdService;
        private readonly IPMGConfigurationService _service;
        private readonly PMGDATABASEEntities _context;
        private readonly IPMGService _pmgService;
        public PMGController(IPMGConfigurationService service, IPmdService pmdService, PMGDATABASEEntities context, IPMGService pmgService)
        {
            this._pmdService = pmdService;
            this._service = service;
            this._context = context;
            this._pmgService = pmgService;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Quick Setup
        public ActionResult QuickSetup()
        {
            ViewBag.CurrentPageCode = "B1";
            return View();
        }

        [HttpPost]
        public JsonResult GetPageDisplay(int pmgInch, int actionType)
        {
            List<SelectOption> list = new List<SelectOption>();
            list = GetPageDisplayList(pmgInch, actionType);

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

                if (!pmdModel.Connection)
                    return Json(new BaseResult(1, "The PMG does not online"));

                string errorMsg = string.Empty;
                bool issave = isSave(model.pmgid, out errorMsg);
                if (!issave)
                    return Json(new BaseResult(1, errorMsg));

                var paramaterIdArray = new int[] { (int)ParamaterId.IdleDisplay,(int)ParamaterId.IdleDisplayPage,
                    (int)ParamaterId.SpeedLimit,(int)ParamaterId.SpeedLimitDisplay,(int)ParamaterId.SpeedLimitDisplayPage,(int)ParamaterId.SpeedLimitAlertAction,
                    (int)ParamaterId.AlertLimit,(int)ParamaterId.AlertLimitDisplay, (int)ParamaterId.AlertLimitDisplayPage,(int)ParamaterId.AlertLimitAlertAction};

                var paramaterIds = string.Join(",", paramaterIdArray);
                int i = this._service.DeleteConfigurationByPmgid(model.pmgid, paramaterIds);

                List<PMGConfiguration> paramConfigureEntryList = model.ToQuickSetup();
                bool bo = SaveDB(paramConfigureEntryList);
                if (!bo)
                    return Json(new BaseResult(1, "Save failly"));

                string message = string.Empty;
                bool isSend = SendDataToServer(pmdModel.IMSI, pmdModel.PMDID, out message);
                if (isSend && string.IsNullOrEmpty(message))
                    return Json(new BaseResult(0, "Data is successfully written to PMG"));

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
            var pmgModel = DeviceOptions.GetSelectedPMG();
            if (pmgModel == null || string.IsNullOrEmpty(pmgModel.IMSI))
            {
                result.code = 1;
                result.msg = "Please select a device";
                return Json(result);
            }

            var paramaterIdArray = new int[] { (int)ParamaterId.IdleDisplay,(int)ParamaterId.IdleDisplayPage,
                    (int)ParamaterId.SpeedLimit,(int)ParamaterId.SpeedLimitDisplay,(int)ParamaterId.SpeedLimitDisplayPage,(int)ParamaterId.SpeedLimitAlertAction,
                    (int)ParamaterId.AlertLimit,(int)ParamaterId.AlertLimitDisplay, (int)ParamaterId.AlertLimitDisplayPage,(int)ParamaterId.AlertLimitAlertAction};

            var paramaterIds = string.Join(",", paramaterIdArray);

            List<PMGConfiguration> list = this._service.GetConfigurationByPmgid(pmgModel.PMD_ID.ToInt(0), paramaterIds);
            if (list == null || list.Count == 0)
            {
                result.code = 1;
                result.msg = "The PMG does not configuration data";
                return Json(result);
            }

            QuickSetupModel model = new QuickSetupModel();
            model.pmgid = pmgModel.PMD_ID.ToInt(0);
            model.actionTypeIdle = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.IdleDisplay).Value.ToInt(0);
            model.pageTypeIdle = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.IdleDisplayPage).Value;

            model.pmgInch = FriscoDev.Application.Interface.PacketProtocol.GetPMDDisplaySize(model.pageTypeIdle);

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
                int PageType = FriscoDev.Application.Interface.PacketProtocol.byte2Int(page.PageType);
                List<Pages> list = this._service.GetDisplayPagesByActionType(displayType, PageType, username);
                foreach (var item in list)
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(item.PageName);
                    listPages.Add(new SelectOption { value = item.PageName, Text = name });
                }
            }
            return listPages;
        }

        public List<SelectOption> GetPageDisplayList(int pmgInch, int actionType)
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
            return list;
        }
        #endregion

        #region Configuration
        public ActionResult Configuration()
        {
            ViewBag.CurrentPageCode = "B2";
            ViewBag.Date = DateTime.Now.ToEnUsDateTime();
            ViewBag.Time = DateTime.Now.ToLongTimeString();//ToString("HH:mm:ss");
            return View();
        }

        [HttpPost]
        public JsonResult GetConfiguration()
        {
            ModelEnitity<ConfigurationModel> result = new ModelEnitity<ConfigurationModel>() { model = new ConfigurationModel() };
            var pmgModel = DeviceOptions.GetSelectedPMG();
            if (pmgModel == null || string.IsNullOrEmpty(pmgModel.IMSI))
            {
                result.code = 1;
                result.msg = "Please select a device";
                return Json(result);
            }

            var paramaterIdArray = new int[] { (int)ParamaterId.Clock,(int)ParamaterId.MinLimit,(int)ParamaterId.MaxLimit,(int)ParamaterId.SpeedUnit,
                    (int)ParamaterId.TemperatureUnit,(int)ParamaterId.Brightness, (int)ParamaterId.EnableMUTCDCompliance};
            var paramaterIds = string.Join(",", paramaterIdArray);

            List<PMGConfiguration> list = this._service.GetConfigurationByPmgid(pmgModel.PMD_ID.ToInt(0), paramaterIds);
            if (list == null || list.Count == 0)
            {
                result.code = 1;
                result.msg = "The PMG does not configuration data";
                return Json(result);
            }

            ConfigurationModel model = new ConfigurationModel();
            model.minSpeed = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.MinLimit).Value.ToInt(0);
            model.maxSpeed = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.MaxLimit).Value.ToInt(0);
            model.speedUnit = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.SpeedUnit).Value;
            model.temperatureUnit = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.TemperatureUnit).Value;
            model.numBright = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.Brightness).Value.ToInt(0);
            model.mutcd = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.EnableMUTCDCompliance).Value;

            var dateModel = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.Clock);
            if (dateModel != null)
            {
                long value = dateModel.Value.ToLong(0);
                DateTime pmgClock = DateTime.Now.AddTicks(-value);
                model.pmgClock = pmgClock.ToEnUsDateTime() + ", " + pmgClock.ToLongTimeString();
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

                if (!pmdModel.Connection)
                    return Json(new BaseResult(1, "The PMG does not online"));

                string errorMsg = string.Empty;
                bool issave = isSave(model.pmgid, out errorMsg);
                if (!issave)
                    return Json(new BaseResult(1, errorMsg));

                var paramaterIdArray = new int[] { (int)ParamaterId.MinLimit,(int)ParamaterId.MaxLimit,(int)ParamaterId.SpeedUnit,
                    (int)ParamaterId.TemperatureUnit,(int)ParamaterId.Brightness, (int)ParamaterId.EnableMUTCDCompliance};
                var paramaterIds = string.Join(",", paramaterIdArray);


                int i = this._service.DeleteConfigurationByPmgid(model.pmgid, paramaterIds);

                List<PMGConfiguration> paramConfigureEntryList = model.ToConfigurations();

                bool bo = SaveDB(paramConfigureEntryList);
                if (!bo)
                    return Json(new BaseResult(1, "Save failly"));

                string message = string.Empty;
                bool isSend = SendDataToServer(pmdModel.IMSI, pmdModel.PMDID, out message);
                if (isSend && string.IsNullOrEmpty(message))
                    return Json(new BaseResult(0, "Data is successfully written to PMG"));

                return Json(new BaseResult(1, message));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, "Exception: " + e.Message));
            }

        }

        [HttpPost]
        public JsonResult ChangeConfigurationTime(ConfigurationTime model)
        {
            if (model == null || model.pmgid <= 0)
                return Json(new BaseResult(1, "Parameters error"));

            Pmd pmdModel = _pmdService.GetPmgById(model.pmgid);
            if (pmdModel == null)
                return Json(new BaseResult(1, "The PMG does not exist"));

            var paramaterIdArray = new int[] { (int)ParamaterId.Clock };
            var paramaterIds = string.Join(",", paramaterIdArray);

            int i = this._service.DeleteConfigurationByPmgid(model.pmgid, paramaterIds);

            DateTime date = Convert.ToDateTime(model.date);
            DateTime time = Convert.ToDateTime(model.time);

            DateTime clock = new DateTime(date.Year, date.Month, date.Day,
                                  time.Hour, time.Minute, time.Second);

            long value = DateTime.Now.Ticks - clock.Ticks;

            List<PMGConfiguration> paramList = new List<PMGConfiguration>();
            paramList.Add(new PMGConfiguration(model.pmgid, ParamaterId.Clock, value.ToString(), 1));

            bool bo = SaveDB(paramList);
            if (!bo)
                return Json(new BaseResult(1, "Save failly"));


            string message = string.Empty;
            bool isSend = SendDataToServer(pmdModel.IMSI, pmdModel.PMDID, out message, PMDConfiguration.NotificationType.SetClock);
            if (isSend && string.IsNullOrEmpty(message))
                return Json(new BaseResult(0, "Date and time is successfully written to PMG"));

            return Json(new BaseResult(1, "Date and time is successfully written to PMG"));
        }
        #endregion

        #region Text
        public ActionResult Text()
        {
            ViewBag.CurrentPageCode = "B3";
            return View();
        }

        [HttpPost]
        public JsonResult GetPageList(int displaySize, int pageType = 0)
        {
            List<SelectOption> list = new List<SelectOption>();
            string username = LoginHelper.UserName;
            var listPages = this._service.GetDisplayPagesByActionType(displaySize, pageType, username);
            foreach (var item in listPages)
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(item.PageName.Trim());
                list.Add(new SelectOption { value = item.PageName.Trim(), Text = name });
            }
            return Json(list);
        }

        [HttpPost]
        public JsonResult GetPageByName(string name, int pageType = 0)
        {
            int displaySize = FriscoDev.Application.Interface.PacketProtocol.GetPMDDisplaySize(name);
            PMDInterface.PageTextFile pageFile = new PMDInterface.PageTextFile();
            string username = LoginHelper.UserName;
            var page = this._service.GetDisplayPagesByPageName(name, displaySize, pageType, username);
            if (page != null)
            {
                Boolean status = pageFile.fromString(page.Content);
                if (string.IsNullOrEmpty(pageFile.pageName))
                    pageFile.pageName = System.IO.Path.GetFileNameWithoutExtension(page.PageName.Trim());
            }

            return Json(pageFile);
        }

        [HttpPost]
        public JsonResult SaveTextOptions(PMDInterface.PageTextFile model)
        {
            try
            {
                string pageName = model.selectedValue;// model.getFilename();
                string content = model.toString();
                int hash = model.getHashValue();
                string username = LoginHelper.UserName;
                int i = this._service.UpdatePage(pageName, (int)model.displayType, (int)model.pageType, username, content, hash);

                return Json(new BaseResult(0, model.pageName));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, e.Message));
            }

        }

        [HttpPost]
        public JsonResult CreateNewPage(string name, int displayType, int pageType = 0)
        {
            try
            {
                PMDDisplaySize displaySize = (PMDDisplaySize)displayType;
                PageType pageSize = (PageType)pageType;
                string pageName = name + PMDInterface.PageTag.getFileExtension(pageSize, displaySize, false);
                int hash = 0;
                string username = LoginHelper.UserName;
                var i = this._service.InsertPage(pageName, displayType, pageType, "", hash, username);
                return Json(new BaseResult(0, pageName));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, e.Message));
            }

        }



        [HttpPost]
        public JsonResult DeletePage(string name, int pageType = 0)
        {
            try
            {
                int displaySize = FriscoDev.Application.Interface.PacketProtocol.GetPMDDisplaySize(name);
                string username = LoginHelper.UserName;
                var i = this._service.DeletePage(name, displaySize, pageType, username);
                return Json(new BaseResult(0, "Delete successfully"));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, e.Message));
            }

        }

        #endregion

        #region Graphics

        public ActionResult TestGraphic(string name = "Happy_RK.G12", int pageType = 1)
        {
            ImageHelper image = new ImageHelper();
            int displaySize = FriscoDev.Application.Interface.PacketProtocol.GetPMDDisplaySize(name);
            PMDInterface.PageGraphicFile pageFile = new PMDInterface.PageGraphicFile((PMDDisplaySize)displaySize);
            string username = LoginHelper.UserName;
            var page = this._service.GetDisplayPagesByPageName(name, displaySize, pageType, username);
            if (page != null)
            {
                Boolean status = pageFile.fromString(page.Content);
                if (string.IsNullOrEmpty(pageFile.pageName))
                    pageFile.pageName = System.IO.Path.GetFileNameWithoutExtension(page.PageName.Trim());

                var mBitmapData = pageFile.mBitmapData;
                var type = (FriscoTab.PMDDisplaySize)displaySize;
                string fileName = string.Format("{0}.{1}.png", page.PageName.Trim(), username.Trim());
                image.MakeImg(fileName, mBitmapData, type);

            }
            return View();
        }

        public ActionResult Graphics()
        {
            ViewBag.CurrentPageCode = "B4";
            return View();
        }


        [HttpPost]
        public JsonResult GetGraphicPageByName(string name, int pageType)
        {
            int displaySize = FriscoDev.Application.Interface.PacketProtocol.GetPMDDisplaySize(name);
            PMDInterface.PageGraphicFile pageFile = new PMDInterface.PageGraphicFile((PMDDisplaySize)displaySize);
            string username = LoginHelper.UserName;
            var page = this._service.GetDisplayPagesByPageName(name, displaySize, pageType, username);
            if (page != null)
            {
                ImageHelper image = new ImageHelper();
                Boolean status = pageFile.fromString(page.Content);
                if (string.IsNullOrEmpty(pageFile.pageName))
                    pageFile.pageName = System.IO.Path.GetFileNameWithoutExtension(page.PageName.Trim());

                var mBitmapData = pageFile.mBitmapData;
                var type = (FriscoTab.PMDDisplaySize)displaySize;
                string fileName = string.Format("{0}.{1}.png", page.PageName.Trim(), username.Trim());
                image.MakeImg(fileName, mBitmapData, type);

                pageFile.ImageUrl = Url.Content("~/Images/Graphic/" + fileName);

            }

            return Json(pageFile);
        }

        #endregion

        #region Animations
        public ActionResult Animation()
        {
            ViewBag.CurrentPageCode = "B5";
            return View();
        }
        #endregion

        #region Scheduled Operation

        public ActionResult ScheduledOperation()
        {
            ViewBag.CurrentPageCode = "B6";
            List<PMGModel> list = this._pmgService.GetPMGList(LoginHelper.UserName, 1);
            DateTime now = DateTime.Now;
            ViewBag.StartDate = now.Date.ToString("yyyy/MM/dd");
            ViewBag.EndDate = now.AddMonths(1).Date.ToString("yyyy/MM/dd");

            DateTime nowTime = new DateTime(2020, 1, 1, 8, 0, 0, DateTimeKind.Local);
            ViewBag.StartTime = nowTime.ToString("HH:mm");
            ViewBag.EndTime = nowTime.AddHours(8.00).ToString("HH:mm");
            return View(list);
        }

        [HttpPost]
        public JsonResult AddScheduledOperation(FriscoTab.ScheduledOperation model)
        {
            try
            {

                string errorMsg = string.Empty;
                if (!model.syntaxCheck(ref errorMsg))
                {
                    return Json(new BaseResult(1, errorMsg));
                }
                model.days = model.toDays();
                var displayType = (byte)model.displayType;
                string name = model.getFilename();

                var exist = this._context.ScheduleOperations.FirstOrDefault(p => p.Name == name && p.DisplayType == displayType && p.PMG_ID == model.PMGID);
                if (exist != null && !string.IsNullOrEmpty(exist.Name))
                    return Json(new BaseResult(1, "Scheduled operation is already"));

                model.idleDisplayPage = model.GetPageTag(model.idleDisplayPageName, model.idleDisplayMode);
                model.limitDisplayPage = model.GetPageTag(model.limitDisplayPageName, model.limitDisplayMode);
                model.alertDisplayPage = model.GetPageTag(model.alertDisplayPageName, model.alertDisplayMode);

                string content = model.toString();
                int hash = model.getHashValue();
                ScheduleOperations schedule = new ScheduleOperations();
                schedule.Name = name;
                schedule.DisplayType = displayType;
                schedule.Content = content;
                schedule.Hash = hash;
                schedule.PMG_ID = model.PMGID;
                this._context.ScheduleOperations.Add(schedule);
                int i = this._context.SaveChanges();
                return Json(new BaseResult(0, name));

            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, e.Message));
            }
        }

        [HttpPost]
        public JsonResult SaveScheduledOperation(FriscoTab.ScheduledOperation model)
        {
            try
            {
                string errorMsg = string.Empty;
                if (!model.syntaxCheck(ref errorMsg))
                {
                    return Json(new BaseResult(1, errorMsg));
                }
                model.days = model.toDays();
                var displayType = (byte)model.displayType;
                string name = model.name;
                var schedule = this._context.ScheduleOperations.FirstOrDefault(p => p.Name == name && p.DisplayType == displayType && p.PMG_ID == model.PMGID);
                if (schedule == null || string.IsNullOrEmpty(schedule.Name))
                    return Json(new BaseResult(1, "Scheduled Operations  is empty!"));

                model.idleDisplayPage = model.GetPageTag(model.idleDisplayPageName, model.idleDisplayMode);
                model.limitDisplayPage = model.GetPageTag(model.limitDisplayPageName, model.limitDisplayMode);
                model.alertDisplayPage = model.GetPageTag(model.alertDisplayPageName, model.alertDisplayMode);
                string content = model.toString();
                int hash = model.getHashValue();

                schedule.Content = content;
                schedule.Hash = hash;
                int i = this._context.SaveChanges();
                return Json(new BaseResult(0, model.operationName));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, e.Message));
            }
        }

        [HttpPost]
        public ActionResult GetScheduledByOperationName(string operationName, FriscoTab.PMDDisplaySize displaySize, int PMGID)
        {
            FriscoTab.ScheduledOperation model = new FriscoTab.ScheduledOperation();

            var displayType = (byte)displaySize;
            var schedule = this._context.ScheduleOperations.FirstOrDefault(p => p.Name == operationName && p.DisplayType == displayType && p.PMG_ID == PMGID);
            if (schedule != null)
            {
                var bo = model.fromString(schedule.Content);
                model.PMGID = schedule.PMG_ID;
                model.displayType = displaySize;
                model.selectedDays = model.fromDays();

                model.IdlePageList = GetPageDisplayList((int)model.idleDisplayPage.displaySize, (int)model.idleDisplayMode);
                model.LimitPageList = GetPageDisplayList((int)model.limitDisplayPage.displaySize, (int)model.limitDisplayMode);
                model.AlertPageList = GetPageDisplayList((int)model.alertDisplayPage.displaySize, (int)model.alertDisplayMode);

                return Json(new { code = 0, model = model });
            }
            return Json(new { code = 1 });
        }


        [HttpPost]
        public ActionResult DeleteScheduledOperation(string operationName, FriscoTab.PMDDisplaySize displaySize, int PMGID)
        {
            var displayType = (byte)displaySize;
            var schedule = this._context.ScheduleOperations.FirstOrDefault(p => p.Name == operationName && p.DisplayType == displayType && p.PMG_ID == PMGID);
            if (schedule == null)
                return Json(new BaseResult(1, "Scheduled Operations  is empty!"));
            this._context.ScheduleOperations.Remove(schedule);
            this._context.SaveChanges();
            return Json(new BaseResult(0, "Ok"));
        }

        [HttpPost]
        public JsonResult GetScheduledOperationList()
        {
            List<ScheduledOperationViewModel> viewList = new List<ScheduledOperationViewModel>();
            var list = this._context.ScheduleOperations.ToList();
            FriscoTab.ScheduledOperation model = null;
            foreach (var item in list)
            {
                model = new FriscoTab.ScheduledOperation();
                var bo = model.fromString(item.Content);
                var displayColumn = model.getDisplayColumnString();
                displayColumn.Name = item.Name;
                displayColumn.PMGID = item.PMG_ID;
                viewList.Add(displayColumn);
            }
            return Json(viewList);
        }


        #endregion

        #region Communication
        public ActionResult Communication()
        {
            ViewBag.CurrentPageCode = "B7";
            return View();
        }
        #endregion

        #region Radar
        public ActionResult Radar()
        {
            ViewBag.CurrentPageCode = "B8";
            return View();
        }

        [HttpPost]
        public JsonResult SaveRadarData(RadarViewModel model)
        {
            try
            {
                if (model == null || model.PMGID <= 0)
                    return Json(new BaseResult(1, "Parameters error"));

                Pmd pmdModel = _pmdService.GetPmgById(model.PMGID);
                if (pmdModel == null)
                    return Json(new BaseResult(1, "The PMG does not exist"));

                if (!pmdModel.Connection)
                    return Json(new BaseResult(1, "The PMG does not online"));

                string errorMsg = string.Empty;
                bool issave = isSave(model.PMGID, out errorMsg);
                if (!issave)
                    return Json(new BaseResult(1, errorMsg));


                var paramaterIdArray = new int[] { (int)ParamaterId.Radar,(int)ParamaterId.RadarHoldoverTime,(int)ParamaterId.RadarCosine,
                    (int)ParamaterId.RadarUnitResolution,(int)ParamaterId.RadarSensitivity,(int)ParamaterId.RadarTargetStrength,(int)ParamaterId.RadarTargetAcceptance,
                    (int)ParamaterId.RadarTargetHoldOn,(int)ParamaterId.RadarOperationDirection,(int)ParamaterId.RadarExternalRadarSpeed,
                    (int)ParamaterId.RadarExternalEchoPanRadarData};
                var paramaterIds = string.Join(",", paramaterIdArray);


                int i = this._service.DeleteConfigurationByPmgid(model.PMGID, paramaterIds);

                List<PMGConfiguration> paramConfigureEntryList = model.ToRadarData();

                bool bo = SaveDB(paramConfigureEntryList);
                if (!bo)
                    return Json(new BaseResult(1, "Save failly"));

                string message = string.Empty;
                bool isSend = SendDataToServer(pmdModel.IMSI, pmdModel.PMDID, out message);
                if (isSend && string.IsNullOrEmpty(message))
                    return Json(new BaseResult(0, "Data is successfully written to PMG"));

                return Json(new BaseResult(1, message));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, "Exception: " + e.Message));
            }

        }


        [HttpPost]
        public JsonResult GetRadarData()
        {
            ModelEnitity<RadarViewModel> result = new ModelEnitity<RadarViewModel>() { model = new RadarViewModel() };
            var pmgModel = DeviceOptions.GetSelectedPMG();
            if (pmgModel == null || string.IsNullOrEmpty(pmgModel.IMSI))
            {
                result.code = 1;
                result.msg = "Please select a device";
                return Json(result);
            }

            var paramaterIdArray = new int[] { (int)ParamaterId.Radar,(int)ParamaterId.RadarHoldoverTime,(int)ParamaterId.RadarCosine,
                    (int)ParamaterId.RadarUnitResolution,(int)ParamaterId.RadarSensitivity,(int)ParamaterId.RadarTargetStrength,(int)ParamaterId.RadarTargetAcceptance,
                    (int)ParamaterId.RadarTargetHoldOn,(int)ParamaterId.RadarOperationDirection,(int)ParamaterId.RadarExternalRadarSpeed,
                    (int)ParamaterId.RadarExternalEchoPanRadarData};
            var paramaterIds = string.Join(",", paramaterIdArray);

            List<PMGConfiguration> list = this._service.GetConfigurationByPmgid(pmgModel.PMD_ID.ToInt(0), paramaterIds);
            if (list == null || list.Count == 0)
            {
                result.code = 1;
                result.msg = "The PMG does not configuration data";
                return Json(result);
            }

            RadarViewModel model = new RadarViewModel();
            model.Radar = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.Radar).Value.ToInt(0);
            model.RadarHoldoverTime = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.RadarHoldoverTime).Value.ToInt(0);
            model.RadarCosine = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.RadarCosine).Value.ToInt(0);
            model.RadarUnitResolution = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.RadarUnitResolution).Value.ToInt(0);
            model.RadarSensitivity = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.RadarSensitivity).Value.ToInt(0);

            model.RadarTargetStrength = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.RadarTargetStrength).Value.ToInt(0);
            model.RadarTargetAcceptance = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.RadarTargetAcceptance).Value.ToInt(0);
            model.RadarTargetHoldOn = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.RadarTargetHoldOn).Value.ToInt(0);
            model.RadarOperationDirection = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.RadarOperationDirection).Value.ToInt(0);
            model.RadarExternalRadarSpeed = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.RadarExternalRadarSpeed).Value.ToInt(0);
            model.RadarExternalEchoPanRadarData = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.RadarExternalEchoPanRadarData).Value.ToInt(0);

            model.PMGID = pmgModel.PMD_ID.ToInt(0);
            result.code = 0;
            result.msg = "ok";
            result.model = model;
            return Json(result);
        }
        #endregion

        #region Traffic Data
        public ActionResult TrafficData()
        {
            ViewBag.CurrentPageCode = "B10";
            return View();
        }

        [HttpPost]
        public JsonResult SaveTrafficData(TrafficDataViewModel model)
        {
            try
            {
                if (model == null || model.PMGID <= 0)
                    return Json(new BaseResult(1, "Parameters error"));

                Pmd pmdModel = _pmdService.GetPmgById(model.PMGID);
                if (pmdModel == null)
                    return Json(new BaseResult(1, "The PMG does not exist"));

                if (!pmdModel.Connection)
                    return Json(new BaseResult(1, "The PMG does not online"));

                string errorMsg = string.Empty;
                bool issave = isSave(model.PMGID, out errorMsg);
                if (!issave)
                    return Json(new BaseResult(1, errorMsg));

                var paramaterIdArray = new int[] { (int)ParamaterId.TrafficEnableRecording,(int)ParamaterId.TrafficTargetStrength,
                    (int)ParamaterId.TrafficMinimumTrackingDistance,(int)ParamaterId.TrafficMinimumFollowingTime,(int)ParamaterId.TrafficDataOnDemand};
                var paramaterIds = string.Join(",", paramaterIdArray);


                int i = this._service.DeleteConfigurationByPmgid(model.PMGID, paramaterIds);

                List<PMGConfiguration> paramConfigureEntryList = model.ToTrafficData();

                bool bo = SaveDB(paramConfigureEntryList);
                if (!bo)
                    return Json(new BaseResult(1, "Save failly"));

                string message = string.Empty;
                bool isSend = SendDataToServer(pmdModel.IMSI, pmdModel.PMDID, out message);
                if (isSend && string.IsNullOrEmpty(message))
                    return Json(new BaseResult(0, "Data is successfully written to PMG"));

                return Json(new BaseResult(1, message));
            }
            catch (Exception e)
            {
                return Json(new BaseResult(1, "Exception: " + e.Message));
            }

        }

        [HttpPost]
        public JsonResult GetTrafficData()
        {
            ModelEnitity<TrafficDataViewModel> result = new ModelEnitity<TrafficDataViewModel>() { model = new TrafficDataViewModel() };
            var pmgModel = DeviceOptions.GetSelectedPMG();
            if (pmgModel == null || string.IsNullOrEmpty(pmgModel.IMSI))
            {
                result.code = 1;
                result.msg = "Please select a device";
                return Json(result);
            }

            var paramaterIdArray = new int[] { (int)ParamaterId.TrafficEnableRecording,(int)ParamaterId.TrafficTargetStrength,
                    (int)ParamaterId.TrafficMinimumTrackingDistance,(int)ParamaterId.TrafficMinimumFollowingTime,(int)ParamaterId.TrafficDataOnDemand};
            var paramaterIds = string.Join(",", paramaterIdArray);

            List<PMGConfiguration> list = this._service.GetConfigurationByPmgid(pmgModel.PMD_ID.ToInt(0), paramaterIds);
            if (list == null || list.Count == 0)
            {
                result.code = 1;
                result.msg = "The PMG does not configuration data";
                return Json(result);
            }

            TrafficDataViewModel model = new TrafficDataViewModel();
            model.TrafficEnableRecording = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.TrafficEnableRecording).Value.ToInt(0);
            model.TrafficTargetStrength = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.TrafficTargetStrength).Value.ToInt(0);
            model.TrafficMinimumTrackingDistance = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.TrafficMinimumTrackingDistance).Value.ToInt(0);
            model.TrafficMinimumFollowingTime = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.TrafficMinimumFollowingTime).Value.ToInt(0);
            model.TrafficDataOnDemand = list.FirstOrDefault(p => p.Parameter_ID == (int)ParamaterId.TrafficDataOnDemand).Value.ToInt(0);
            model.PMGID = pmgModel.PMD_ID.ToInt(0);
            result.code = 0;
            result.msg = "ok";
            result.model = model;
            return Json(result);
        }
        #endregion

        #region About
        public ActionResult About()
        {
            ViewBag.CurrentPageCode = "B11";
            return View();
        }
        #endregion

        #region FactoryReset
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
        #endregion

        #region method
        private bool SaveDB(List<PMGConfiguration> paramConfigureEntryList)
        {
            this._context.PMGConfiguration.AddRange(paramConfigureEntryList);
            int i = this._context.SaveChanges();
            return i > 0;
        }

        public bool isSave(int PMGID, out string errorMsg)
        {
            errorMsg = "";
            bool result = false;
            int count = 0;
            while (!result && count < 6)
            {
                ConfigurationLog log = ConfigurationLog(PMGID);
                if (log == null)
                {
                    Thread.Sleep(5 * 1000);
                    count++;
                    if (count == 6) errorMsg = "Server is still configuring PMG";
                }
                else
                {
                    count = 6;
                    if (log.Status == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        errorMsg = log.Message;
                    }
                }
            }

            return result;
        }

        public ConfigurationLog ConfigurationLog(int PMGID)
        {
            var model = this._context.ConfigurationLog.Where(p => p.PMG_ID == PMGID).OrderByDescending(p => p.Transaction_ID).Take(1).FirstOrDefault();
            return model;
        }

        public bool SendDataToServer(string imsi, int pmgId, out string message,
            PMDConfiguration.NotificationType notificationType = PMDConfiguration.NotificationType.Update)
        {
            message = string.Empty;
            TimeSpan dateTime = DateTime.Now - new DateTime(2000, 1, 1);
            long transactionId = (long)dateTime.TotalSeconds;
            bool bo = PMDInterface.ServerConnection.SendDataToServer(PMDConfiguration.TableID.PMG, notificationType, transactionId, imsi);
            System.Threading.Thread.Sleep(200);
            var model = this._context.ConfigurationLog.FirstOrDefault(p => p.PMG_ID == pmgId && p.Transaction_ID == transactionId);
            if (model == null)
            {
                message = "Data is successfully written to PMG";
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
