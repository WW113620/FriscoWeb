using Application.Common;
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
        public PMGController(IPMGConfigurationService service, IPmdService pmdService)
        {
            this._pmdService = pmdService;
            this._service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QuickSetup()
        {
            return View();
        }

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

            List<PMGConfiguration> list = this._service.GetByPmgid(pmdModel.PMD_ID.ToInt(0), paramaterIds);
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
            if (dateModel == null)
            {
                model.date = DateTime.Now.ToString("yyyy-MM-dd");
                model.time = DateTime.Now.ToString("HH:mm:ss");
            }
            else {

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

                this._service.DeleteByPmgid(model.pmgid, paramaterIds);

                List<PMGConfiguration> paramConfigureEntryList = model.ToConfigurations();
                bool bo = SaveDB(paramConfigureEntryList);

                if (bo)
                {
                    //PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMG, PMDConfiguration.DatabaseOperationType.Update, pmdModel.IMSI);
                    PMDInterface.ServerConnection.SendDataToServer(PMDConfiguration.TableID.PMG, PMDConfiguration.NotificationType.Update, pmdModel.IMSI);
                    return Json(new BaseResult(0, "Save successfully "));
                }

                return Json(new BaseResult(1, "Save failly"));
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

        private static bool SaveDB(List<PMGConfiguration> paramConfigureEntryList)
        {
            using (var db = new PMGDATABASEEntities())
            {
                db.PMGConfiguration.AddRange(paramConfigureEntryList);
                int i = db.SaveChanges();
                return i > 0;
            }
        }

    }
}