using Application.Common;
using AutoMapper;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using FriscoDev.UI.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FriscoDev.Application.Common;
using FriscoDev.UI.Common;
using FriscoDev.UI.Models;
using PMDCellularInterface;
using FriscoDev.Application.Models;

namespace FriscoDev.UI.Controllers
{
    public class DeviceController : BaseController
    {
        private readonly IDeviceService _deviceService;
        private readonly IMessageService _messageService;
        private readonly PMGDATABASEEntities _context;
        public DeviceController(IDeviceService deviceService, IMessageService messageService, PMGDATABASEEntities context)
        {
            this._deviceService = deviceService;
            this._messageService = messageService;
            this._context = context;
        }

        public ActionResult Index()
        {
            ViewBag.UserType = LoginHelper.UserType;
            return View();
        }


        [HttpPost]
        public JsonResult GetDeviceList(int pageIndex, int pageSize)
        {
            int iCount = 0;
            List<PMGModel> list = this._deviceService.GetDeviceList("", pageIndex, pageSize, out iCount);
            AddressModel Entity = new AddressModel();
            foreach (var item in list)
            {
                Entity = CommonUtils.ForAddress(item.Address);
                item.Address = Entity.Address;
                item.City = Entity.City;
                item.State = Entity.State;
                item.Country = Entity.Country;
                item.ZipCode = Entity.ZipCode;
                item.Direction = Entity.Direction;
                item.CountryName = Entity.CountryName;
            }

            var model = new DeviceManagementViewModel
            {
                Devices = list.Select(d => new DeviceManagementViewModel.DeviceViewModel()
                {
                    PMDName = d.PMDName,
                    IMSI = d.IMSI,
                    Address = d.Address,
                    City = d.City,
                    State = d.State,
                    Country = d.Country,
                    ZipCode = d.ZipCode,
                    CountryName = d.CountryName,
                    Username = d.Username,
                    Location = d.Location,
                    Connection = d.Connection,
                    StatsCollection = d.StatsCollection ? "Yes" : "No",
                    PMDID = Convert.ToInt32(d.PMD_ID),
                    CurrentConfiguration = d.CurrentConfiguration,
                    CurrrentConfigurationTime = d.CurrrentConfigurationTime,
                    NewConfiguration = d.NewConfiguration,
                    NewConfigurationTime = Commons.ConvertShowDate(d.NewConfigurationTime),
                    isClock = d.Clock,
                    FirmwareVersion = d.FirmwareVersion,
                    NewFirmwareId = d.NewFirmwareId,
                    NumFirmwareUpdateAttempts = d.NumFirmwareUpdateAttempts,
                    KeepAliveMessageInterval = d.KeepAliveMessageInterval,
                    DeviceType = Enum.GetName(typeof(DeviceType), d.DeviceType),
                    IntDeviceType = Convert.ToInt32(d.DeviceType),
                    DevCoordinateX = Commons.GetDevCoordinateX(d.Location).ToString(),
                    DevCoordinateY = Commons.GetDevCoordinateY(d.Location).ToString(),
                    HighSpeedAlert = d.HighSpeedAlert,
                    LowSpeedAlert = d.LowSpeedAlert,
                    Direction = Commons.ConvertDirection(d.Direction.ToString("").Trim()).Replace("_", "-")
                })
            };
            #region page
            int pageCount = (int)Math.Ceiling(iCount / Convert.ToDouble(pageSize));
            if (pageCount > 0 && pageCount < pageIndex)
            {
                pageIndex = pageCount;
            }
            #endregion
            return Json(new { list = model.Devices?.ToList(), pageCount = pageCount, iCount = iCount });
        }

        public ActionResult DeviceLocation(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public JsonResult GetDeviceLocation(string id)
        {
            var locations = _deviceService.GetDevicesLocation(id);
            var model = locations.Select(d => new DeviceModel
            {
                ID = d.ID,
                IMSI = d.IMSI,
                PMDName = d.PMDName,
                DeviceType = ((DeviceType)(int.Parse(d.DeviceType))).ToString(),
                DevCoordinateX = Commons.GetDevCoordinateX(d.Location).ToString(),
                DevCoordinateY = Commons.GetDevCoordinateY(d.Location).ToString(),
                StrStartDate = ConvertDate(d.StartDate),
                StrEndDate = ConvertDate(d.EndDate),
            });
            return Json(model);
        }
        [HttpPost]
        public JsonResult DeleteLocation(string id)
        {
            int i = _deviceService.DeleteLocation(id);
            return Json(i);
        }


        #region add pmg
        public ActionResult Add()
        {
            ViewBag.UserType = LoginHelper.UserType;
            return View();
        }
        public JsonResult IsExistIMSI(string imsi)
        {
            Pmd pmd = _deviceService.GetPmd(imsi);
            if (pmd != null)
                return Json(1);
            else
                return Json(0);
        }
        [HttpPost]
        public JsonResult GetLatLngToAddress(string address)
        {
            string result = string.Empty;
            try
            {
                address = HttpUtility.UrlDecode(address);
                string strurl = string.Format("http://maps.google.com/maps/api/geocode/json?address={0}&sensor=false", address);
                result = Commons.SendGetHttpRequest(strurl);
            }
            catch (Exception e)
            {

            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult AddDevice(PmgViewModel viewModel)
        {
            ResultEntity result = new ResultEntity() { errorCode = 500, errorStr = "" };
            try
            {
                var exist = this._context.PMD.Count(p => p.IMSI == viewModel.IMSI);
                if (exist > 0)
                    return Json(new ResultEntity() { errorCode = 501, errorStr = "A device with this SIM # already exists." });

                string _code = viewModel.IMSI;
                int PmdId = 0;
                try
                {
                    if (!string.IsNullOrEmpty(_code) && _code.Length > 5)
                    {
                        PmdId = Commons.GetLast7Digits(_code);
                    }

                }
                catch (Exception ex)
                {
                }
                string location = "0,0";
                decimal _x = viewModel.DevCoordinateX.ToDecimal(0);
                decimal _y = viewModel.DevCoordinateY.ToDecimal(0);

                location = _x + "," + _y;
                AddressModel model = new AddressModel();
                model.Address = viewModel.Address;
                model.City = viewModel.City;
                model.State = viewModel.State;
                model.Country = viewModel.Country;
                model.ZipCode = "";
                model.Direction = viewModel.Direction;
                model.CountryName = viewModel.CountryName;
                var address = CommonUtils.ToAddress(model);

                FriscoDev.Application.Models.PMD pmg = new Application.Models.PMD()
                {
                    PMDName = viewModel.PMDName,
                    IMSI = viewModel.IMSI,
                    Address = address,
                    Username = LoginHelper.UserName,
                    DeviceType = viewModel.DeviceType,
                    Location = location,
                    Connection = viewModel.Connection == 1,
                    StatsCollection = viewModel.StatsCollection == 1,
                    PMD_ID = PmdId,
                    Clock = "0",
                    FirmwareVersion = "1.0",
                    NumFirmwareUpdateAttempts = 0,
                    KeepAliveMessageInterval = 10,
                    CS_ID = LoginHelper.CS_ID,
                    HighSpeedAlert = (byte)viewModel.HighSpeedAlert,
                    LowSpeedAlert = (byte)viewModel.LowSpeedAlert,
                };

                int i = _deviceService.Add(pmg);
                if (i > 0)
                {
                    SendNotificationToCloudServer(viewModel.IMSI, PMDConfiguration.NotificationType.PMGInsert);
                    result.errorCode = 200;
                    result.errorStr = "OK";
                    return Json(result);
                }
                else
                {
                    result.errorCode = 300;
                    result.errorStr = "Fail";
                    return Json(result);
                }

            }
            catch (Exception e)
            {
                result.errorCode = 400;
                result.errorStr = e.Message;
                return Json(result);
            }

        }

        [HttpPost]
        public JsonResult Delete(string IMSI)
        {
            int i = _deviceService.Delete(IMSI);
            if (i > 0)
            {
                SendNotificationToCloudServer(IMSI, PMDConfiguration.NotificationType.PMGDelete);
            }
            return Json(i);
        }
        #endregion

        #region edit

        public ActionResult Edit(string id)
        {
            var pmg = this._context.PMD.FirstOrDefault(p => p.IMSI == id);
            if (pmg == null)
                return HttpNotFound("pmg does not exist");
            ViewBag.UserType = LoginHelper.UserType;
            ViewBag.IMSI = id;
            return View();
        }

        public JsonResult GetEditDevice(string id)
        {
            var pmg = this._context.PMD.FirstOrDefault(p => p.IMSI == id);
            if (pmg == null)
                return Json(new PmgViewModel());

            AddressModel Entity = CommonUtils.ForAddress(pmg.Address);

            var model = new PmgViewModel
            {
                PMGID = pmg.PMD_ID.ToInt(0),
                IMSI = pmg.IMSI,
                PMDName = pmg.PMDName,
                Address = Entity.Address,
                City = Entity.City,
                State = Entity.State,
                Country = Entity.Country,
                Direction = Entity.Direction,
                CountryName = Entity.CountryName,
                StatsCollection = pmg.StatsCollection == true ? 1 : 0,
                DeviceType = pmg.DeviceType.ToInt(0),
                DevCoordinateX = Commons.GetDevCoordinateX(pmg.Location).ToString(),
                DevCoordinateY = Commons.GetDevCoordinateY(pmg.Location).ToString(),
                HighSpeedAlert = (int)pmg.HighSpeedAlert,
                LowSpeedAlert = (int)pmg.LowSpeedAlert,
                Connection = pmg.Connection == true ? 1 : 0
            };

            return Json(model);
        }


        [HttpPost]
        public JsonResult EditDevice(PmgViewModel viewModel)
        {
            ResultEntity result = new ResultEntity() { errorCode = 500, errorStr = "" };
            try
            {
                var exist = this._context.PMD.Count(p => p.IMSI == viewModel.IMSI);
                if (exist == 0)
                    return Json(new ResultEntity() { errorCode = 501, errorStr = "This pmg does not exists." });

                string location = "0,0";
                decimal _x = viewModel.DevCoordinateX.ToDecimal(0);
                decimal _y = viewModel.DevCoordinateY.ToDecimal(0);

                location = _x + "," + _y;


                AddressModel model = new AddressModel();
                model.Address = viewModel.Address;
                model.City = viewModel.City;
                model.State = viewModel.State;
                model.Country = viewModel.Country;
                model.ZipCode = "";
                model.Direction = viewModel.Direction;
                model.CountryName = viewModel.CountryName;
                var address = CommonUtils.ToAddress(model);

                FriscoDev.Application.Models.PMD pmg = new Application.Models.PMD()
                {
                    PMD_ID = viewModel.PMGID,
                    PMDName = viewModel.PMDName,
                    IMSI = viewModel.IMSI,
                    Address = address,
                    DeviceType = viewModel.DeviceType,
                    Location = location,
                    Connection = viewModel.Connection == 1,
                    StatsCollection = viewModel.StatsCollection == 1,
                    HighSpeedAlert = (byte)viewModel.HighSpeedAlert,
                    LowSpeedAlert = (byte)viewModel.LowSpeedAlert,
                };

                int i = _deviceService.Update(pmg);
                if (i > 0)
                {
                    SendNotificationToCloudServer(pmg.IMSI, PMDConfiguration.NotificationType.PMGUpdate);
                    result.errorCode = 200;
                    result.errorStr = "OK";
                }
                else
                {
                    result.errorCode = 300;
                    result.errorStr = "Fail";
                }
            }
            catch (Exception e)
            {
                result.errorCode = 400;
                result.errorStr = e.Message;
            }
            return Json(result);
        }

        #endregion

        [HttpPost]
        public JsonResult Check(string IMSI, int activeId)
        {
            var pmdModel = _deviceService.CheckDevice(IMSI, activeId);
            return Json(pmdModel);
        }



        public ActionResult ViewMessage(int devType, int pmdId)
        {
            ViewBag.devType = devType;
            ViewBag.pmdId = pmdId;
            return View();
        }
        [HttpPost]
        public JsonResult SearchMessageList(int devType, int pmdId, int pageIndex, int pageSize)
        {
            int iCount = 0;
            List<MessageEntityVm> newList = new List<MessageEntityVm>();
            var CS_ID = "";//loginUser.CS_ID;
            var list = _messageService.GetAertMessageList(devType, CS_ID, pmdId, pageIndex, pageSize, out iCount);
            foreach (var model in list)
            {
                model.TIMESTAMP2 = Commons.getLocalTime(model.TIMESTAMP, LoginHelper.TIME_ZONE).ToString("yyyy-MM-dd HH:mm:ss");
                newList.Add(model);
            }
            #region Ò³Ãæ·ÖÒ³
            int pageCount = (int)Math.Ceiling(iCount / Convert.ToDouble(pageSize));
            if (pageCount > 0 && pageCount < pageIndex)
            {
                pageIndex = pageCount;
            }
            #endregion
            return Json(new { errorCode = 200, list = list, Count = iCount, PageCount = pageCount, PageIndex = pageIndex });
        }
        [HttpPost]
        public JsonResult ClearAlerts(int devType, int pmdId)
        {
            _messageService.UpdateAllAlert(devType, pmdId);
            return Json(new { errorCode = 200 });
        }
        public static string ConvertDate(DateTime dt)
        {
            if (dt == null || dt == DateTime.MaxValue || dt == DateTime.MinValue)
                return "---";
            else
                return dt.ToString("yyyy-MM-dd");
        }

        public ActionResult Message()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDeviceAlertMsg(string pmgId, string daylimit, int pageIndex, int pageSize)
        {
            string startDate = string.Empty;
            if (!string.IsNullOrEmpty(daylimit))
            {
                startDate = CommonUtils.GetLocalTime().AddDays(-daylimit.ToInt(0)).ToString("yyyy-MM-dd 00:00");
            }
            int iCount = 0;
            var list = this._deviceService.GetDeviceMessageList(pmgId, startDate, pageIndex, pageSize, out iCount);
            #region page
            int pageCount = (int)Math.Ceiling(iCount / Convert.ToDouble(pageSize));
            if (pageCount > 0 && pageCount < pageIndex)
            {
                pageIndex = pageCount;
            }
            #endregion
            return Json(new { list = list, pageCount = pageCount, iCount = iCount });
        }

        [HttpPost]
        public JsonResult DeleteAllWarning(string pmgId)
        {
            if (!string.IsNullOrEmpty(pmgId))
            {
                int PMGID = pmgId.ToInt(0);
                this._deviceService.DeleteWarningMessage(PMGID);
            }
            return Json(0);
        }


        public void SendNotificationToCloudServer(string imsi,
            PMDConfiguration.NotificationType notificationType = PMDConfiguration.NotificationType.PMGInsert)
        {
            TimeSpan dateTime = DateTime.Now - new DateTime(2000, 1, 1);
            long transactionId = (long)dateTime.TotalSeconds;
            bool bo = PMDInterface.ServerConnection.SendDataToServer(PMDConfiguration.TableID.PMG, notificationType, transactionId, imsi);

            //PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMG, PMDConfiguration.DatabaseOperationType.Insert, imsi);

        }

    }
}