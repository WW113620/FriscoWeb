using AutoMapper;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FriscoDev.Application.Common;
using FriscoDev.Application.Enum;
using FriscoDev.Application.Models;
using FriscoDev.UI.Common;
using FriscoDev.UI.Models;
using Newtonsoft.Json;
using TimeZoneInfo = System.TimeZoneInfo;

namespace FriscoDev.UI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ITimeZoneInfoService _timeZoneInfoService;
        public UserController(IUserService userService, ITimeZoneInfoService timeZoneInfoService)
        {
            this._userService = userService;
            _timeZoneInfoService = timeZoneInfoService;
        }
        public ActionResult Index()
        {
            ViewBag.UserType = LoginHelper.UserType;
            return View();
        }

        [HttpPost]
        public JsonResult GetUserList(string userName)
        {
            int iCount = 0;
            var account = _userService.GetAccount(LoginHelper.UserID);
            var list = this._userService.GetAccountsByCustomerId(account).OrderByDescending(p => p.UR_ADDTIME).ToList();
            if (!string.IsNullOrEmpty(userName))
            {
                list = list.Where(d => d.UR_Name.Contains(userName)).ToList();
            }
            List<AccountViewModel> viewModel = new List<AccountViewModel>();
            string lastLoginTime = string.Empty;
            foreach (var item in list)
            {
                AccountViewModel accountViewModel = new AccountViewModel();
                lastLoginTime = "---";
                accountViewModel.UserId = item.UR_ID;
                accountViewModel.UserName = item.UR_Name;
                accountViewModel.RealName = item.UR_RealName;
                accountViewModel.UserType = ((UserType)item.UR_TYPE_ID).ToString();
                accountViewModel.HideUserType = item.UR_TYPE_ID;
                accountViewModel.Customer = item.CS_Name;
                accountViewModel.Active = item.UR_ACTIVE == 1 ? "Active" : "Inactive";
                accountViewModel.ActiveType = item.UR_ACTIVE;
                accountViewModel.ImgUrl = item.IMG_URL;
                accountViewModel.TimeZone = item.TIME_ZONE;
                accountViewModel.SiteName = item.SiteName;
                accountViewModel.ProfileImgUrl = item.ProfileImgUrl;
                UserLoginInfo login = _userService.GetUserLoginInfo(item.UR_ID);
                if (login != null && login.Id > 0)
                {
                    lastLoginTime = login.LoginTime?.GetDateTimeFormats('r')[0].ToString();//Fri, 21 Dec 2012 15:14:35 GMT
                }
                accountViewModel.LastLoginTime = lastLoginTime;
                viewModel.Add(accountViewModel);
            }
            return Json(new { list = viewModel });
        }
        public ActionResult Add()
        {
            ViewBag.UserType = LoginHelper.UserType;
            return View();
        }
        [HttpPost]
        public JsonResult IsExistName(string value)
        {
            ResultEntity result = new ResultEntity() { errorCode = 500, errorStr = "" };
            Account account = _userService.GetAccountByName(value);
            if (account != null && !string.IsNullOrWhiteSpace(account.UR_ID))
            {
                result.errorCode = 200;
            }
            else
            {
                result.errorCode = 300;
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult AddNewUser(AddUserModel addUser)
        {
            ResultEntity result = new ResultEntity() { errorCode = 500, errorStr = "" };
            try
            {
                string strGuid = Guid.NewGuid().ToString().ToUpper();
                _userService.AddAccount(new Account
                {
                    UR_ID = strGuid,
                    UR_NAME = addUser.UserName,
                    UR_RealName = addUser.RealName,
                    CS_ID = LoginHelper.CS_ID,
                    LN_ID = "D6CD181A-0270-456F-BE6F-8134FC564A46",
                    UR_PASSWD = addUser.Password,
                    UR_TYPE_ID = addUser.SelectedUserType,
                    UR_ACTIVE = true,
                    UR_ADDTIME = DateTime.Now,
                    UR_UPTIME = null,
                    UR_STATUS = "0",
                    IS_ADMIN = false
                });
                if (addUser.SelectedUserType != 3)
                {
                    SiteConfig site = new SiteConfig();
                    site.Login_UR_ID = strGuid;
                    site.SiteName = addUser.SiteName;
                    site.ProfileImgUrl = addUser.ProfileImgUrl;
                    _userService.AddOrUpdateSiteConfig(site);
                }
                result.errorCode = 200;
            }
            catch (Exception e)
            {
                result.errorCode = 400;
                result.errorStr = e.Message;
            }
            return Json(result);
        }
        public ActionResult Delete(string id)
        {
            _userService.DeleteAccount(id);
            return RedirectToAction("Index");
        }
        public JsonResult GetTimeZoneList()
        {

            if (Session["TimeZone"] != null)
            {
                var lst = Session["TimeZone"] as List<TimeZoneInfo>;
                var list = JsonConvert.SerializeObject(lst);
                return Json(new { data = list });
            }
            else
            {
                var lst = _timeZoneInfoService.GeTimeZoneInfoList();
                var list = JsonConvert.SerializeObject(lst);
                return Json(new { data = list });
            }
        }

        public ActionResult UserProfile(string id)
        {
            ViewBag.UserType = LoginHelper.UserType;
            ViewBag.UserId = id;
            return View();
        }
        [HttpPost]
        public JsonResult GetUserProfile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json("Sorry,This user does not exist");
            }
            id = HttpUtility.UrlDecode(id);
            var user = _userService.GetAccount(id);
            if (user == null)
            {
                return Json("Sorry,This user does not exist");
            }
            var siteConfig = _userService.GetSiteConfigByUser(user.UR_ID);
            string lastLoginTime = "---";
            UserLoginInfo login = _userService.GetUserLoginInfo(user.UR_ID);
            if (login != null && login.Id > 0)
            {
                lastLoginTime = login.LoginTime?.GetDateTimeFormats('r')[0].ToString();//Fri, 21 Dec 2012 15:14:35 GMT
            }
            var timeZoneInfo = _timeZoneInfoService.GeTimeZoneInfo(d => d.ZoneValue == user.TIME_ZONE);
            var viewModel = new EditUserModel
            {
                ShowLoginTime = lastLoginTime,
                RealName = user.UR_RealName,
                UserId = user.UR_ID,
                UserName = user.UR_NAME,
                SelectedUserType = user.UR_TYPE_ID,
                ImgUrl = user.IMG_URL,
                TimeZone = timeZoneInfo?.ZoneName,
                UR_TYPE_ID = user.UR_TYPE_ID,
                SiteName = siteConfig?.SiteName,
                ProfileImgUrl = siteConfig?.ProfileImgUrl,
                ActiveType = user.UR_ACTIVE
            };
            return Json(viewModel);
        }
        [HttpPost]
        public JsonResult Check(string userId, bool active)
        {
            var user = _userService.GetAccount(userId);
            user.UR_ACTIVE = active;
            bool check = _userService.EditAccount(user);
            return Json(check);
        }

        public ActionResult Edit(string userId)
        {
            ViewBag.UserType = LoginHelper.UserType;
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        public JsonResult GetEditUser(string userId)
        {
            AccountVm accountVm = new AccountVm();
            if (string.IsNullOrEmpty(userId))
                return Json("Parameter error");
            var user = _userService.GetAccount(userId);
            ObjectExpansion.AutoMapping(user, accountVm);
            if (user != null)
            {
                accountVm.UR_Name = user.UR_NAME;
                var siteConfig = _userService.GetSiteConfigByUser(user.UR_ID);
                if (siteConfig != null)
                {
                    ObjectExpansion.AutoMapping(siteConfig, accountVm);
                }
            }
            return Json(accountVm);
        }
        [HttpPost]
        public JsonResult EditUserInfo(AccountVm model)
        {
            ResultEntity result = new ResultEntity() { errorCode = 500, errorStr = "" };
            try
            {
                var user = _userService.GetAccount(model.UR_ID);
                if (!string.IsNullOrWhiteSpace(model.ProfileImgUrl) && !string.IsNullOrWhiteSpace(model.SiteName))
                {
                    var site = _userService.GetSiteConfigByUserId(model.UR_ID);
                    site.SiteName = model.SiteName;
                    site.ProfileImgUrl = model.ProfileImgUrl;
                    site.Login_UR_ID = user.UR_ID;
                    _userService.AddOrUpdateSiteConfig(site);
                }
                int SelectedUserType = string.IsNullOrEmpty(model.UR_TYPE_ID.ToString()) || model.UR_TYPE_ID == 0 ? user.UR_TYPE_ID : model.UR_TYPE_ID;
                user.UR_UPTIME = DateTime.Now;
                user.UR_RealName = model.UR_RealName;
                user.UR_TYPE_ID = model.UR_TYPE_ID;
                if (!string.IsNullOrWhiteSpace(model.UR_PASSWD))
                {
                    user.UR_PASSWD = model.UR_PASSWD;
                }
                _userService.EditAccount(user);
                result.errorCode = 200;
            }
            catch (Exception e)
            {
                result.errorCode = 400;
                result.errorStr = e.Message;
            }
            return Json(result);
        }
    }
}