using FriscoDev.Application.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Common
{
    public class DeviceOptions
    {
        public static PMGModel GetSelectedPMG()
        {
            if (System.Web.HttpContext.Current.Session["SelectPMG"] == null)
                return new PMGModel();

            var model = System.Web.HttpContext.Current.Session["SelectPMG"] as PMGModel;
            return model;

        }


    }
}