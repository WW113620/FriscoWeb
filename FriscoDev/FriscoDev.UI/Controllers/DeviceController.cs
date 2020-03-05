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

namespace FriscoDev.UI.Controllers
{
    public class DeviceController : BaseController
    {
        private readonly IDeviceService _deviceService;
        private readonly IMessageService _messageService;
        public DeviceController(IDeviceService deviceService, IMessageService messageService)
        {
            this._deviceService = deviceService;
            _messageService = messageService;
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
            //List<PMGModel> viewModel =new List<PMGModel>(); //Mapper.Map<List<PMGModel>>(list);
            AddressModel Entity = new AddressModel();
            foreach (var item in list)
            {
                Entity = ForAddress(item.Address);
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
                    Clock = d.Clock == "0" ? "Active" : "Inactive",
                    FirmwareVersion = d.FirmwareVersion,
                    NewFirmwareId = d.NewFirmwareId,
                    NumFirmwareUpdateAttempts = d.NumFirmwareUpdateAttempts,
                    KeepAliveMessageInterval = d.KeepAliveMessageInterval,
                    DeviceType = Enum.GetName(typeof(DeviceType), d.DeviceType),
                    IntDeviceType = Convert.ToInt32(d.DeviceType),
                    DevCoordinateX = Commons.splitStringToDecimal(d.Location)[0].ToString(CultureInfo.InvariantCulture),
                    DevCoordinateY = Commons.splitStringToDecimal(d.Location)[1].ToString(CultureInfo.InvariantCulture),
                    HighSpeedAlert = d.HighSpeedAlert,
                    LowSpeedAlert = d.LowSpeedAlert,
                    Direction = Commons.ConvertDirection(d.Direction.Trim()).Replace("_", "-")
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
                DevCoordinateX = Commons.splitStringToDecimal(d.Location)[0].ToString(CultureInfo.InvariantCulture),
                DevCoordinateY = Commons.splitStringToDecimal(d.Location)[1].ToString(CultureInfo.InvariantCulture),
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
        public JsonResult AddDevice(string BelongName, string IMSI, string PMDName, string Address, string City, string State, string Country, string CountryName,
            string DevCoordinateX, string DevCoordinateY, string Direction, int StatsCollection, int DeviceType, int HighSpeedAlert, int LowSpeedAlert, string StartDate = "", string EndDate = "")
        {
            ResultEntity result = new ResultEntity() { errorCode = 500, errorStr = "" };
            try
            {
                DateTime start = DateTime.MinValue;
                DateTime end = DateTime.MaxValue;
                string _code = IMSI;
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
                decimal _x = 0;
                decimal _y = 0;
                if (!string.IsNullOrEmpty(DevCoordinateX))
                {
                    _x = decimal.Parse(DevCoordinateX);
                }
                if (!string.IsNullOrEmpty(DevCoordinateY))
                {
                    _y = decimal.Parse(DevCoordinateY);
                }
                location = _x + "," + _y;
                AddressModel model = new AddressModel();
                model.Address = Address;
                model.City = City;
                model.State = State;
                model.Country = Country;
                model.ZipCode = "";
                model.Direction = Direction;
                model.CountryName = CountryName;
                var address = ToAddress(model);

                int i = _deviceService.Add(new Pmd
                {
                    PMDName = PMDName,
                    IMSI = IMSI,
                    Address = address,
                    Username = LoginHelper.UserName,
                    Location = location,
                    Connection = StatsCollection != 0,
                    StatsCollection = StatsCollection != 0,
                    PMDID = PmdId,
                    Clock = "0",
                    FirmwareVersion = "1.0",
                    NumFirmwareUpdateAttempts = 0,
                    KeepAliveMessageInterval = 10,
                    DeviceType = DeviceType,
                    CS_ID = LoginHelper.CS_ID,
                    HighSpeedAlert = HighSpeedAlert,
                    LowSpeedAlert = LowSpeedAlert,
                    LeasedStartDate = StartDate,
                    LeasedEndDate = EndDate,
                    BelongName = BelongName
                });
                if (i > 0)
                {
                    SendNotificationToCloudServer(IMSI, "Insert");
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
                SendNotificationToCloudServer(IMSI, "Delete");
            }
            return Json(i);
        }
        [HttpPost]
        public JsonResult Check(string IMSI, int activeId)
        {
            var pmdModel = _deviceService.CheckDevice(IMSI, activeId);
            return Json(pmdModel);
        }
        public ActionResult Edit(string id)
        {
            ViewBag.UserType = LoginHelper.UserType;
            ViewBag.id = id.ToString();
            return View();
        }

        public JsonResult GetEditDevice(string id)
        {
            Pmd pmd = new Pmd();
            int deviceType = _deviceService.GetDeviceType(id);
            if (deviceType == 3)
            {
                pmd = _deviceService.GetLeasedDevice(id);
            }
            else
            {
                pmd = _deviceService.Get(id);
            }
            AddressModel Entity = ForAddress(pmd.Address);
            var model = new DeviceManagementViewModel.EditDevice
            {
                Id = pmd.Id,
                PMDName = pmd.PMDName,
                IMSI = pmd.IMSI,
                BelongName = pmd.BelongName,
                LeasedStartDate = ToEmptyString(pmd.LeasedStartDate),
                LeasedEndDate = ToEmptyString(pmd.LeasedEndDate),
                Address = Entity.Address,
                City = Entity.City,
                State = Entity.State,
                Country = Entity.Country,
                ZipCode = Entity.ZipCode,
                Direction = Entity.Direction,
                CountryName = Entity.CountryName,
                Username = pmd.Username,
                Location = pmd.Location,
                Connection = pmd.Connection,
                StatsCollection = pmd.StatsCollection == true ? 1 : 0,
                PMDID = pmd.PMDID,
                Clock = pmd.Clock,
                DeviceType = pmd.DeviceType,
                DevCoordinateX = Commons.splitStringToDecimal(pmd.Location)[0].ToString(CultureInfo.InvariantCulture),
                DevCoordinateY = Commons.splitStringToDecimal(pmd.Location)[1].ToString(CultureInfo.InvariantCulture),
                HighSpeedAlert = pmd.HighSpeedAlert,
                LowSpeedAlert = pmd.LowSpeedAlert
            };
            return Json(model);
        }
        [HttpPost]
        public JsonResult EditDevice(string Id, string BelongName, string IMSI, int PMDID, string PMDName, string Address, string City, string State, string Country, string CountryName,
        string DevCoordinateX, string DevCoordinateY, string Direction, int StatsCollection, int DeviceType, int HighSpeedAlert, int LowSpeedAlert, string StartDate = "", string EndDate = "")
        {
            ResultEntity result = new ResultEntity() { errorCode = 500, errorStr = "" };
            try
            {
                string location = "0,0";
                decimal _x = 0;
                decimal _y = 0;

                if (!string.IsNullOrEmpty(DevCoordinateX))
                {
                    _x = decimal.Parse(DevCoordinateX);
                }
                if (!string.IsNullOrEmpty(DevCoordinateY))
                {
                    _y = decimal.Parse(DevCoordinateY);
                }
                location = _x + "," + _y;
                AddressModel model = new AddressModel();
                model.Address = Address;
                model.City = City;
                model.State = State;
                model.Country = Country;
                model.ZipCode = "";
                model.Direction = Direction;
                model.CountryName = CountryName;
                var address = ToAddress(model);
                int i = _deviceService.Update(new Pmd
                {
                    Id = Id,
                    BelongName = BelongName,
                    IMSI = IMSI,
                    PMDID = PMDID,
                    PMDName = PMDName,
                    Address = address,
                    Location = location,
                    Username = LoginHelper.UserName,
                    Connection = StatsCollection != 0,
                    StatsCollection = StatsCollection != 0,
                    DeviceType = DeviceType,
                    HighSpeedAlert = HighSpeedAlert,
                    LowSpeedAlert = LowSpeedAlert,
                    LeasedStartDate = StartDate,
                    LeasedEndDate = EndDate
                });
                if (i > 0)
                {
                    SendNotificationToCloudServer(IMSI, "Update");
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

        public ActionResult ViewMessage(int devType,int pmdId)
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
        public static AddressModel ForAddress(string result)
        {
            AddressModel pmd = new AddressModel();
            if (string.IsNullOrEmpty(result))
            {
                return pmd;
            }
            else
            {
                var arrAddress = result.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                if (arrAddress.Length == 7)
                {
                    pmd.Address = arrAddress[0].Replace("Address:", string.Empty);
                    pmd.City = arrAddress[1].Replace("City:", string.Empty);
                    pmd.State = arrAddress[2].Replace("State:", string.Empty);
                    pmd.Country = arrAddress[3].Replace("Country:", string.Empty);
                    pmd.ZipCode = arrAddress[4].Replace("ZipCode:", string.Empty);
                    pmd.Direction = arrAddress[5].Replace("Direction:", string.Empty);
                    pmd.CountryName = arrAddress[6].Replace("CountryName:", string.Empty);
                    return pmd;
                }
                else
                {
                    return pmd;
                }
            }
        }
        public static string ToAddress(AddressModel pmd)
        {
            if (pmd == null)
            {
                return "";
            }
            else
            {
                if (string.IsNullOrEmpty(pmd.Direction))
                    pmd.Direction = "0";
                return string.Format("Address:{0}||City:{1}||State:{2}||Country:{3}||ZipCode:{4}||Direction:{5}||CountryName:{6}", pmd.Address, pmd.City, pmd.State, pmd.Country, pmd.ZipCode, pmd.Direction, pmd.CountryName);
            }
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
        public void SendNotificationToCloudServer(string imsi, string type)
        {
            if (!string.IsNullOrEmpty(imsi))
            {
                if (type == "Insert")
                {
                    PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMG, PMDConfiguration.DatabaseOperationType.Insert, imsi);
                }
                else if (type == "Update")
                {
                    PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMG, PMDConfiguration.DatabaseOperationType.Update, imsi);
                }
                else if (type == "Delete")
                {
                    PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMG, PMDConfiguration.DatabaseOperationType.Delete, imsi);
                }
                else if (type == "ClockUpdate")
                {
                    PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMG, PMDConfiguration.DatabaseOperationType.ClockUpdate, imsi);
                }
                else if (type == "ConfigurationUpdate")
                {
                    PMDConfiguration.SendNotificationToCloudServer(PMDConfiguration.TableID.PMG, PMDConfiguration.DatabaseOperationType.ConfigurationUpdate, imsi);
                }
            }
        }
        public static string ToEmptyString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "---";
            DateTime dt = Convert.ToDateTime(str);
            return dt.ToString("yyyy-MM-dd");
        }
    }
}