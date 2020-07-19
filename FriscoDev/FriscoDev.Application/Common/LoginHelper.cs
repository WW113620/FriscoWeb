using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FriscoDev.Application.ViewModels;

namespace FriscoDev.Application.Common
{
    public class LoginHelper
    {
        public static string LoginPrefix = "FD";
        public static string LoginCookieUID { get { return $"{LoginPrefix}_Login_Cookies_UserId"; } }
        public static string LoginCookieUserName { get { return $"{LoginPrefix}_Login_Cookies_UserName"; } }
        public static string LoginCookieRealName { get { return $"{LoginPrefix}_Login_Cookies_RealName"; } }
        public static string LoginCookieUserType { get { return $"{LoginPrefix}_Login_Cookies_UserType"; } }
        public static string LoginCookieCS_ID { get { return $"{LoginPrefix}_Login_Cookies_CS_ID"; } }
        public static string LoginCookieTIME_ZONE { get { return $"{LoginPrefix}_Login_Cookies_TIME_ZONE"; } }
        public static string LoginCookieZoomLevel { get { return $"{LoginPrefix}_Login_Cookies_ZoomLevel"; } }

        public static string UserID
        {
            get { return CookieHelper.GetCookie(LoginHelper.LoginCookieUID); }
        }
        public static string UserName
        {
            get { return CookieHelper.GetCookie(LoginHelper.LoginCookieUserName); }
        }

        public static string RealName
        {
            get { return CookieHelper.GetCookie(LoginHelper.LoginCookieRealName); }
        }

        public static int UserType
        {
            get { return CookieHelper.GetCookie(LoginHelper.LoginCookieUserType).ToInt(0); }
        }
        public static string CS_ID
        {
            get { return CookieHelper.GetCookie(LoginHelper.LoginCookieCS_ID); }
        }
        public static string TIME_ZONE
        {
            get { return CookieHelper.GetCookie(LoginHelper.LoginCookieTIME_ZONE); }
        }
        public static string ZoomLevel
        {
            get { return CookieHelper.GetCookie(LoginHelper.LoginCookieZoomLevel); }
        }


        public static bool IsOnline()
        {
            if (!string.IsNullOrEmpty(LoginHelper.UserID) && !string.IsNullOrEmpty(LoginHelper.UserName))
                return true;
            return false;
        }


        public static void SetLoginCookie(Models.Account user,SiteModel site, int minute = 30)
        {
            string expireTimeSpan = minute.ToString("");
            CookieHelper.SetCookie(LoginHelper.LoginCookieUID, user.UR_ID, CookieHelper.TimeUtil.mi, expireTimeSpan);
            CookieHelper.SetCookie(LoginHelper.LoginCookieUserName, user.UR_NAME, CookieHelper.TimeUtil.mi, expireTimeSpan);
            CookieHelper.SetCookie(LoginHelper.LoginCookieRealName, user.UR_NAME, CookieHelper.TimeUtil.mi, expireTimeSpan);
            CookieHelper.SetCookie(LoginHelper.LoginCookieUserType, user.UR_TYPE_ID.ToString(), CookieHelper.TimeUtil.mi, expireTimeSpan);
            CookieHelper.SetCookie(LoginHelper.LoginCookieCS_ID, user.CS_ID, CookieHelper.TimeUtil.mi, expireTimeSpan);
            CookieHelper.SetCookie(LoginHelper.LoginCookieTIME_ZONE, string.IsNullOrEmpty(site.TimeZone) ? "Pacific Standard Time" : site.TimeZone, CookieHelper.TimeUtil.mi, expireTimeSpan);
            CookieHelper.SetCookie(LoginHelper.LoginCookieZoomLevel, user.ZoomLevel, CookieHelper.TimeUtil.mi, expireTimeSpan);

        }

        public static void Logout()
        {

            CookieHelper.DelCookie(LoginHelper.LoginCookieUID);
            CookieHelper.DelCookie(LoginHelper.LoginCookieUserName);
            CookieHelper.DelCookie(LoginHelper.LoginCookieRealName);
            CookieHelper.DelCookie(LoginHelper.LoginCookieUserType);

            if (System.Web.HttpContext.Current.Session != null)
            {
                System.Web.HttpContext.Current.Session.Clear();
            }
        }


    }
}
