using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Application.Common
{
    public class CommonHelper
    {


        #region Create Guid
        /// <summary>
        /// Create Guid
        /// </summary>
        public static string CreateGuid(string type)
        {
            string result = string.Empty;
            switch (type.ToLower())
            {
                default:
                case "user":
                    result = "U" + Guid.NewGuid().ToString("N");
                    break;
                case "ques":
                    result = "st" + Guid.NewGuid().ToString("N");
                    break;
                case "paper":
                    result = "sj" + Guid.NewGuid().ToString("N");
                    break;
                case "exam":
                    result = "dt" + Guid.NewGuid().ToString("N");
                    break;
                case "kaoshi":
                    result = "ks" + Guid.NewGuid().ToString("N");
                    break;
                case "qingjing":
                    result = "qj" + Guid.NewGuid().ToString("N");
                    break;
            }
            return result;
        } 
        #endregion

        #region Common      
        /// <summary>
        /// is Int
        /// </summary>
        /// <returns>result：true or：false</returns>
        public static bool isIntValue(string strValue)
        {
            try
            {
                Convert.ToInt64(strValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// is Float
        /// </summary>
        /// <returns>result：true or：false</returns>
        public static bool isFloatValue(string strValue)
        {
            try
            {
                Convert.ToSingle(strValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Remove Number
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveNumber(string key)
        {
            return Regex.Replace(key, @"\d", "");
        }

        /// <summary>
        /// Remove No Number
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveNotNumber(string key)
        {
            return Regex.Replace(key, @"[^\d]*", "");
        }
        /// <summary>
        ///Full - byte half - angle digital conversion
        /// </summary>
        public static string ChangeCharToInt(string strNum)
        {
            strNum = strNum.Replace("０", "0");
            strNum = strNum.Replace("１", "1");
            strNum = strNum.Replace("２", "2");
            strNum = strNum.Replace("３", "3");
            strNum = strNum.Replace("４", "4");

            strNum = strNum.Replace("５", "5");
            strNum = strNum.Replace("６", "6");
            strNum = strNum.Replace("７", "7");
            strNum = strNum.Replace("８", "8");
            strNum = strNum.Replace("９", "9");
            return strNum;
        }

        public static bool IsEmail(string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return false;
            Regex regex = new Regex(@"^[a-zA-Z0-9][a-zA-Z0-9_\-]*@[a-zA-Z0-9_\-]+(\.[a-zA-Z0-9_\-]+)+$", RegexOptions.IgnoreCase);
            return regex.IsMatch(strValue);
        }

        public static bool IsUrlFormat(string strValue)
        {
            Regex re = new Regex(@"(http://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return re.IsMatch(strValue);
        }
        #endregion

        #region PostValue
        public static string GetPostValue(string FormKey, int maxLength = 0, bool isClearHtml = false, bool isClearSql = false, bool isReservationSimpleTag = false, System.Web.Routing.RouteValueDictionary routeDataValue = null)
        {
            string RetValue = "";
            try
            {
                if (routeDataValue != null && routeDataValue.Keys.Contains(FormKey) && routeDataValue[FormKey].ToString("").Length > 0)
                {
                    RetValue = routeDataValue[FormKey].ToString("");
                }
                else
                {
                    if (HttpContext.Current.Request.Form[FormKey] != null)
                    {
                        RetValue = HttpContext.Current.Request.Form[FormKey].ToString();
                    }
                    else if (HttpContext.Current.Request.QueryString[FormKey] != null)
                    {
                        RetValue = HttpContext.Current.Request.QueryString[FormKey].ToString();
                    }
                }
                if (string.IsNullOrWhiteSpace(RetValue))
                    return string.Empty;

                RetValue = RetValue.Trim();
                if (isClearHtml)
                {
                    RetValue = StringNoHtml(RetValue, isReservationSimpleTag, maxLength);
                }
                if (isClearSql)
                {
                    RetValue = SqlFilterExt(RetValue);
                }
                if (maxLength > 0 && RetValue.Length > maxLength)
                {
                    RetValue = RetValue.Substring(0, maxLength);
                }
                RetValue = RetValue.Trim();
            }
            catch (Exception ex)
            {

            }
            if (RetValue == "*")
                RetValue = string.Empty;
            return RetValue.TrimEnd(';');
        }
        /// <summary>
        /// 去掉全部HTML的, 将 小于号=>  gt;,大于号=>  lt;,空格=>  nbsp 进行转换
        /// </summary>
        /// <param name="strHtml"></param>
        /// <param name="isReservationSimpleTag">是否保留大于小于号、制表和换行符</param>
        /// <param name="maxLength">截取的长度</param>
        /// <returns></returns>
        public static string StringNoHtml(string strHtml, bool isReservationSimpleTag = false, int maxLength = 0)
        {
            if (String.IsNullOrWhiteSpace(strHtml))
            {
                return string.Empty;
            }
            else
            {
                string[] aryReg ={
                @"<script[^>]*?>.*?</script>",
                @"<iframe[^>]*?>.*?</iframe>",
                @"<!--.*\n(-->)?",
                @"<(\/\s*)?(.|\n)*?(\/\s*)?>",
                @"<(\w|\s|""|'| |=|\\|\.|\/|#)*",
                @"([\r\n|\s])*",
                @"&(quot|#34);",
                @"&(amp|#38);",
                @"&(lt|#60);",
                @"&(gt|#62);",
                @"&(nbsp|#160);",
                @"&(iexcl|#161);",
                @"&(cent|#162);",
                @"&(pound|#163);",
                @"&(copy|#169);",
                @"%(27|32|3E|3C|3D|3F)",
                @"&#(\d+);"};

                string newReg = aryReg[0];
                string strOutput = strHtml.Replace("&nbsp;", " ");
                for (int i = 0; i < aryReg.Length; i++)
                {
                    Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                    strOutput = regex.Replace(strOutput, string.Empty);
                }
                if (!isReservationSimpleTag)
                {
                    strOutput = strOutput.Replace("<", "&gt;").Replace(">", "&lt;").Replace("\r", string.Empty).Replace("\n", string.Empty);
                }
                if (maxLength > 0 && strOutput.Length > maxLength)
                    strOutput = strOutput.Substring(0, maxLength);
                return strOutput.Replace(" ", "&nbsp;");
            }
        }
        /// <summary>
        /// 去除sql保留字
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string SqlFilterExt(string sql)
        {
            string sqlfilter = @"exec|insert|select|delete|update|chr|mid|master|or|and|truncate|char|declare|join|cmd|;|'|--";
            if (string.IsNullOrEmpty(sql))
                return string.Empty;
            foreach (string i in sqlfilter.Split('|'))
            {
                sql = sql.Replace(i + " ", string.Empty).Replace(" " + i, string.Empty).Replace(i + "%20", string.Empty).Replace("%20" + i, string.Empty);
            }
            sql = sql.Replace("'", "");
            return sql;
        }
        #endregion

        #region Temp
        public static string ConvertShowTakeTime(long sec)
        {
            var m = sec / 60;
            var s = sec % 60;
            string res = string.Format("{0}分{1}秒", m, s);
            return res;
        }
        public static string GetShowExamState(int isExam, int state)
        {
            string res = string.Empty;
            if (isExam == 0)
            {
                res = "---";
            }
            else if (isExam == 1)
            {
                if (state == 0)
                    res = "未批阅";
                else
                    res = "已批阅";
            }
            return res;
        }
        public static string GetShowExamSetState(int isExam, int state)
        {
            string res = string.Empty;
            if (isExam == 2)
            {
                res = "---";
            }
            else if (isExam == 3)
            {
                if (state == 0)
                    res = "未批阅";
                else
                    res = "已批阅";
            }
            return res;
        }
        public static string ShowTotalScore(int isExam, int state, double total)
        {
            string res = string.Empty;
            if (isExam == 0)
            {
                res = "---";
            }
            else if (isExam == 1)
            {
                if (state == 0)
                    res = "---";
                else
                    res = total.ToString();
            }
            return res;
        }
        public static string ShowUserState(int type)
        {
            string res = string.Empty;
            switch(type)
            {
                case 0:
                    res = "正常";
                    break;
                case 1:
                    res = "已锁定";
                    break;
                case 4:
                    res = "已删除";
                    break;
                default:
                    res = "正常";
                    break;
            }
            return res;
        }
        #endregion

        #region Empty
        public static string IsEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";
            else
                return value;
        }
        #endregion

    }

}