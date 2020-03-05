using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriscoDev.UI.Common
{
    public static class IMSICookie
    {
        ///
        /// 写cookie值
        ///
        /// 名称
        /// 值
        /// 过期时间(分钟)
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            cookie.Expires = DateTime.Now.AddMonths(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        ///
        /// 读cookie值
        ///
        /// 名称
        /// cookie值
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies[strName] != null)
                return HttpContext.Current.Request.Cookies[strName].Value;
            return "";
        }
    }
}