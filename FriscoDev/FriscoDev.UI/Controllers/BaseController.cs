using FriscoDev.Application.Common;
using FriscoDev.Application.ViewModels;
using FriscoDev.UI.Utils;
using System;
using System.Linq;
using System.Web.Mvc;
using FriscoDev.Data.Services;
using FriscoDev.UI.Common;
using Newtonsoft.Json;

namespace FriscoDev.UI.Controllers
{
    public class BaseController : Controller
    {

        public BaseController()
        {
            string redirectUrl = GetRedirectUrl();

            if (!LoginHelper.IsOnline())
            {
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Redirect(redirectUrl);
                System.Web.HttpContext.Current.Response.End();
            }

            if (System.Web.HttpContext.Current.Session["SelectPMG"] != null)
            {
                PMGModel pmg = System.Web.HttpContext.Current.Session["SelectPMG"] as PMGModel;
                ViewBag.xvalue = Commons.splitStringToDecimal(pmg.Location)[0];
                ViewBag.yvalue = Commons.splitStringToDecimal(pmg.Location)[1];
                ViewBag.pid = pmg.IMSI;
                ViewBag.PmdId = pmg.PMD_ID;
            }
            else
            {
                var pmgService = new PMGService();
                var pmd = pmgService.GetPMGModelVm(LoginHelper.CS_ID);
                if (pmd != null)
                {
                    ViewBag.XValue = Commons.splitStringToDecimal(pmd.Location)[0];
                    ViewBag.YValue = Commons.splitStringToDecimal(pmd.Location)[1];
                    ViewBag.pid = pmd.IMSI;
                    ViewBag.PmdId = pmd.PMD_ID;
                }
            }
            
        }

        public string GetRedirectUrl()
        {
            string redirectUrl = string.Format("{0}/Login/Index?p={1}", HttpUtils.WebDomainPath(), System.Web.HttpUtility.UrlEncode(System.Web.HttpContext.Current.Request.Url.AbsoluteUri));
            return redirectUrl;
        }

        public PMGModel BaseSelectedPMG()
        {
            PMGModel model = new PMGModel();
            if (System.Web.HttpContext.Current.Session["SelectPMG"] != null)
            {
                model = System.Web.HttpContext.Current.Session["SelectPMG"] as PMGModel;
            }
            return model;
        }



    }
}