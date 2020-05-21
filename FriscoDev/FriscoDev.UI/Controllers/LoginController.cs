using Application.Common;
using FriscoDev.Application.Common;
using FriscoDev.Application.Models;
using FriscoDev.Application.ViewModels;
using FriscoDev.Data.Services;
using FriscoDev.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FriscoDev.UI.Common;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace FriscoDev.UI.Controllers
{
    public class LoginController : Controller
    {

        private readonly IUserService _userService;
        public LoginController(IUserService userService)
        {
            this._userService = userService;
        }

        public ActionResult Index()
        {

            string sourceUrl = CommonHelper.GetPostValue("p");
            if (!string.IsNullOrEmpty(sourceUrl))
                sourceUrl = HttpUtility.UrlDecode(sourceUrl);

            ViewBag.SourceUrl = sourceUrl;
            return View();
        }

        [HttpPost]
        public JsonResult Verify(LoginCommand command)
        {
            if (string.IsNullOrEmpty(command.UserName) || string.IsNullOrEmpty(command.Password))
                return Json(new BaseEnitity { code = 300 });

            Application.Models.Account user = this._userService.GetModel(command.UserName, command.Password);
            if (user == null)
                return Json(new BaseEnitity { code = 500 });

            if (!user.UR_ACTIVE.ObjToBool())
                return Json(new BaseEnitity { code = 502 });

            var model = GetLogoInfo(user.UR_ID);
            var site = new SiteModel();
            if (model != null)
            {
                site = GetSiteModel(model.Address);
            }
            else
            {
                site.TimeZone = "Pacific Standard Time";
            }
            LoginHelper.SetLoginCookie(user, site, 60 * 2);

            return Json(new BaseEnitity { code = 200 });

        }

        public ActionResult LoginOut()
        {
            LoginHelper.Logout();
            return RedirectToAction("Index", "Login");
        }
        public SiteConfigVm GetLogoInfo(string UR_ID)
        {
            SiteConfigVm site = new SiteConfigVm();
            var siteConfig = _userService.GetSiteConfigByUser(UR_ID);
            if (siteConfig != null)
            {
                ObjectExpansion.AutoMapping(siteConfig, site);
            }
            return site;
        }
        public SiteModel GetSiteModel(string address)
        {
            SiteModel model = new SiteModel();
            if (string.IsNullOrEmpty(address))
                return model;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<SiteModel>(address);
        }


        public ActionResult Test()
        {
            GoogleGeoCodeResponse geo = new GoogleGeoCodeResponse();
            string address = "beijing";
            string url = string.Format(@"https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", address, ConfigHelper.GeocodeAddress);

            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(url);
                geo = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(json);
            }
            return Content(JsonConvert.SerializeObject(geo) + "&url" + url);
        }

    }
}