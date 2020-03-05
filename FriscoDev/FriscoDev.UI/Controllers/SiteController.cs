using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using FriscoDev.Application.Common;
using FriscoDev.Application.Models;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using FriscoDev.UI.Common;

namespace FriscoDev.UI.Controllers
{
    public class SiteController : Controller
    {
        private readonly IUserService _userService;
        public SiteController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: Site
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetSiteConfig()
        {
            ViewBag.UserType = LoginHelper.UserType;

            var model = GetLogoInfo(LoginHelper.UserType, LoginHelper.UserID);
            if (model != null)
            {
                var site = GetSiteModel(model.Address);
                if (site != null)
                {
                    model.TimeZone = site.TimeZone;
                    model.Organization = site.Organization;
                    model.City = site.City;
                    model.State = site.State;
                    model.CountryName = site.CountryName;
                }
            }
            return Json(new { success = true, data = model });
        }
        [HttpPost]
        public ActionResult Save(SiteConfigVm site)
        {
            try
            {
                var siteConfig = _userService.GetSiteConfigByUserId(LoginHelper.UserID);
                if (siteConfig == null)
                {
                    siteConfig = new SiteConfig { Login_UR_ID = LoginHelper.UserID };
                }
                siteConfig.Address = GetJsonString(site);
                siteConfig.SiteName = site.SiteName;
                siteConfig.ProfileImgUrl = site.ProfileImgUrl;
                siteConfig.Default_Location = site.Default_Location;
                _userService.AddOrUpdateSiteConfig(siteConfig);
            }
            catch (Exception e)
            {
                Json(new { success = false });
            }
            return Json(new { success = true });
        }
        public SiteConfigVm GetLogoInfo(int UR_TYPE_ID, string UR_ID)
        {
            SiteConfigVm site = new SiteConfigVm();
            string userId = string.Empty;
            if (UR_TYPE_ID == 3)
            {
                userId = _userService.GetParentIDByUser(UR_ID);
            }
            else
            {//管理员
                userId = UR_ID;
            }
            var siteConfig = _userService.GetSiteConfigByUser(userId);
            //site = Mapper.Map<SiteConfigVm>(siteConfig);
            if (siteConfig!=null)
            {
                ObjectExpansion.AutoMapping(siteConfig, site);
            }
            return site;
        }
        public string GetJsonString(SiteConfigVm site)
        {
            SiteModel model = new SiteModel();
            if (site == null)
                return "";
            model.TimeZone = site.TimeZone;
            model.Organization = site.Organization;
            model.City = site.City;
            model.State = site.State;
            model.CountryName = site.CountryName;
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(model);
        }

        public SiteModel GetSiteModel(string address)
        {
            SiteModel model = new SiteModel();
            if (string.IsNullOrEmpty(address))
                return model;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<SiteModel>(address);
        }
    }
}