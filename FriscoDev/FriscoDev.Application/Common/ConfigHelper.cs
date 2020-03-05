using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Application.Common
{
    public class ConfigHelper
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public static readonly string LogRootSavePath = GetConfigValue("LogRootSavePath");
        /// <summary>
        /// read appSettings
        /// </summary>
        /// <param name="sKey">appSettings key</param>
        /// <returns>value</returns>
        public static string GetConfigValue(string sKey)
        {          
            string sValue = null;
            if ((sValue = System.Configuration.ConfigurationManager.AppSettings[sKey]) == null)
            {
                sValue = "";
            }
            return sValue;
        }

    }

}