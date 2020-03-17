using Application.Common;
using AutoMapper;
using Data.Dapper;
using FriscoDev.Application.Common;
using FriscoDev.Application.Models;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using FriscoDev.UI.Common;
using FriscoDev.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace FriscoDev.UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ITestService _testService;
        private readonly IPMGService _pmgService;
        private readonly IDeviceService _deviceService;
        private readonly IPmdService _pmdService;
        private readonly IUserService _userService;
        public HomeController(ITestService testService, IPMGService pmgService, IDeviceService deviceService, IPmdService pmdService, IUserService userService)
        {
            this._testService = testService;
            this._pmgService = pmgService;
            this._deviceService = deviceService;
            _pmdService = pmdService;
            _userService = userService;
        }
        public ActionResult Index()
        {
            return View();
        }

        #region map
        [HttpPost]
        public JsonResult MapDevices()
        {
            List<PMGModel> list = this._pmgService.GetPMGList(string.Empty, 0);
            //list = list.Where(p => p.IMSI == "89148000004701184353").ToList();
            if (list.Count == 0)
            {
                return Json(new { code = 10 });
            }

            var viewList = new
            {
                ZoomLevel = 13,
                Positions = list.Select(d => new { address = ForAddress(d.Address), name = d.PMDName, imsi = d.IMSI, x = CommonUtils.SplitStringToDecimal(d.Location)[0], y = CommonUtils.SplitStringToDecimal(d.Location)[1], t = d.DeviceType, s = GetDeviceIcon(d.IMSI, d.PMD_ID.ToInt(0)), direction = GetDirection(d.Address) })
            };
            return Json(new { code = 0, data = viewList });

        }

        public ActionResult UserManger()
        {
            var userId = LoginHelper.UserID;
            UserModel account = _userService.GetRelugarAccounts(userId).FirstOrDefault();

            ViewBag.User = account;
            return View();
        }
        public static string GetDirection(string address)
        {
            var model = ForAddressModel(address);
            if (model != null)
            {
                return CommonUtils.ConvertDirection(model.Direction);
            }
            else
            {
                return "None";
            }
        }

        public string GetDeviceIcon(string IMSI, int PMD_ID)
        {
            string stateName = "red";
            bool bo1 = false;
            PMGModel model = this._pmgService.GetPMGModel(IMSI);
            if (model != null)
            {
                bo1 = model.Connection.ObjToBool();
            }
            bool bo2 = this._deviceService.CheckDeviceHasMsg(PMD_ID);
            if (bo1)
            {
                if (bo2)
                {
                    stateName = "yellow";
                }
                else
                {
                    stateName = "green";
                }
            }
            else
            {
                //offline red 
                stateName = "red";
            }
            return stateName;
        }

        public static string ForAddress(string result)
        {
            AddressViewModel pmd = new AddressViewModel();
            if (string.IsNullOrEmpty(result))
            {
                return "";
            }

            var arrAddress = result.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            if (arrAddress.Length > 0)
            {
                pmd.Address = arrAddress[0].Replace("Address:", string.Empty);
                return pmd.Address;
            }
            return "";
        }

        public static AddressViewModel ForAddressModel(string result)
        {
            AddressViewModel pmd = new AddressViewModel();
            if (string.IsNullOrEmpty(result))
            {
                return pmd;
            }

            var arrAddress = result.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            if (arrAddress.Length > 1)
            {
                pmd.Address = arrAddress[0].Replace("Address:", string.Empty);
                pmd.Direction = arrAddress[1].Replace("Direction:", string.Empty);
            }
            return pmd;
        }
        #endregion


        [HttpPost]
        public JsonResult GetPMGDeviceList()
        {
            DataEnitity<PMGModel> result = new DataEnitity<PMGModel>();
            List<PMGModel> list = this._pmgService.GetOnlinePMGList();
            if (list.Count == 0)
            {
                result.code = 10;
                result.msg = "no data";
                return Json(result);
            }
            result.code = 0;
            result.data = list;
            return Json(result);
        }

        [HttpPost]
        public JsonResult SetSelectedPMGDev(string imsi)
        {
            ModelEnitity<PMGModel> result = new ModelEnitity<PMGModel>();
            PMGModel model = this._pmgService.GetPMGModel(imsi);
            if (model == null)
            {
                System.Web.HttpContext.Current.Session["SelectPMG"] = null;
                result.code = 10;
                return Json(result);
            }

            System.Web.HttpContext.Current.Session["SelectPMG"] = model;
            result.code = 0;
            result.model = model;
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetSelectedPMGDev()
        {
            ModelEnitity<PMGModel> result = new ModelEnitity<PMGModel>();
            if (System.Web.HttpContext.Current.Session["SelectPMG"] == null)
            {
                result.code = 10;
                return Json(result);
            }
            else
            {
                result.model = System.Web.HttpContext.Current.Session["SelectPMG"] as PMGModel;
                result.code = 0;
                return Json(result);
            }
          
        }

        public ActionResult About()
        {
            using (var db = new PMGDATABASEEntities())
            {
                var user = db.Account.ToList();
            }

            string sql = @" select * from Account where UR_NAME=@UR_NAME ";
            var model = ExecuteDapper.QueryList<Application.Models.Account>(sql, new { UR_NAME = "test" });

            var viewModel = Mapper.Map<List<AccountViewModel>>(model);

            var list = this._testService.GetList();

            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public JsonResult GetDeviceType(string imsi)
        {
            int type = _pmdService.GetDeviceTypeById(imsi);
            return Json(new { errorCode = 200, deviceType = type });
        }
        public string SetPmdSession(string strPmd)
        {
            bool stateName = true;
            System.Web.HttpContext.Current.Session["curpmdid"] = strPmd;
            return "{\"Success\":" + stateName + "}";
        }
        public ActionResult Location(string imis)
        {
            ViewBag.StartDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd" + " 00:00");
            ViewBag.EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            Pmd pmdModel = _pmdService.GetPmd(imis);

            ViewBag.XValue = Commons.splitStringToDecimal(pmdModel.Location)[0].ToString("0");
            ViewBag.YValue = Commons.splitStringToDecimal(pmdModel.Location)[0].ToString("0");
            ViewBag.ZoomLevel = LoginHelper.ZoomLevel;
            return View(pmdModel);
        }
        public JsonNetResult GetDeviceLocation(string imis, string startDate, string endDate)
        {
            var location = _pmdService.GetDevicesLocation(imis, startDate, endDate);
            if (location != null && location.Count() > 0)
            {
                return new JsonNetResult
                {
                    Data = new
                    {
                        Success = true,
                        ZoomLevel = LoginHelper.ZoomLevel,
                        Positions = location.Select(d => new { address = ForAddress(d.Address), id = d.ID, name = d.PMDName, IMSI = d.IMSI, PMDID = d.PMDID, x = Commons.splitStringToDecimal(d.Location)[0], y = Commons.splitStringToDecimal(d.Location)[1], startDate = d.StartDate.ToString("yyyy-MM-dd HH:mm"), endDate = Commons.GetStartDate(d.EndDate) })
                    }
                };
            }
            else
            {
                return new JsonNetResult
                {
                    Data = new
                    {
                        Success = false,
                    }
                };
            }
        }
        public string GetCurUserTimeZoneTimeNew()
        {
            string start = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 00:00");
            string end = Commons.getLocalTime(LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd 23:59");
            return "{\"start\":\"" + start + "\",\"end\":\"" + end + "\"}";
        }
    }
}