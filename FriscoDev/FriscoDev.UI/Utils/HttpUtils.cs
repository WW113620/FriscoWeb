using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Utils
{
    public class HttpUtils
    {
        public static string WebDomainPath()
        {
            //http://localhost:6392
            return string.Format(@"{0}://{1}:{2}", System.Web.HttpContext.Current.Request.Url.Scheme, System.Web.HttpContext.Current.Request.Url.Host, System.Web.HttpContext.Current.Request.Url.Port);
        }

    }
}