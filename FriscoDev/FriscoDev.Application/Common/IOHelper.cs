using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class IOHelper
    {
        private static string _LogRootSavePath = System.Configuration.ConfigurationManager.AppSettings["LogRootSavePath"];

        #region Write in log
        public static void WriteLog(string content, System.Text.Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(content))
                return;
            if (string.IsNullOrEmpty(_LogRootSavePath))
                _LogRootSavePath = @"E:\WriteLog\";
            try
            {
                string strPath = _LogRootSavePath;
                if (string.IsNullOrEmpty(strPath))
                {
                    if (System.Web.HttpContext.Current == null)
                    {
                        strPath = System.Environment.CurrentDirectory.TrimEnd('\\');
                    }
                    else
                    {
                        strPath = System.Web.HttpContext.Current.Server.MapPath("~");
                    }
                }
                DateTime dtNow = DateTime.Now;
                strPath = string.Format("{0}\\Logs\\{1}.txt", strPath.TrimEnd('\\'), dtNow.ToString("yyyyMMdd"));
                string writeContent = string.Format(" Operate Time: {0} ,Operate Content: {1} ", dtNow.ToString("yyy-MM-dd HH:mm:ss"), content);
                WriteFile(strPath, writeContent, true, encoding);
            }
            catch (Exception ex)
            { }
        }
        public static void WriteFile(string path, string content, bool isAppend, System.Text.Encoding encoding = null)
        {
            string temp = @"\";
            string strOutFileDirectory = path.Substring(0, path.LastIndexOf(temp));
            if (!System.IO.Directory.Exists(strOutFileDirectory))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(strOutFileDirectory);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            try
            {
                if (isAppend)
                {
                    if (!File.Exists(path))
                        CreateFile(path, content);
                    else
                    {
                        encoding = encoding == null ? System.Text.Encoding.UTF8 : encoding;
                        using (StreamWriter sw = new StreamWriter(path, true, encoding))
                        {
                            sw.WriteLine(content);
                            sw.Flush();
                            sw.Close();
                        }
                    }
                }
                else
                    CreateFile(path, content);
            }
            catch (Exception ex)
            {
            }
        }
        public static void CreateFile(string path, string content, System.Text.Encoding encoding = null)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                encoding = encoding == null ? System.Text.Encoding.UTF8 : encoding;
                using (StreamWriter sw = new StreamWriter(path, false, encoding))
                {
                    sw.WriteLine(content);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            { }
        }

        #endregion
    }
}
