using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.Common
{
    public class SendMail
    {

        #region 发送邮件
        public static bool Send(string from, string fromname, string to, List<string> ccList, string subject,
            string body, string username, string password, string server, int port, out string errorMsg)
        {
            try
            {
                errorMsg = "";
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, fromname);
                mail.To.Add(to);
                if (ccList != null && ccList.Count > 0)
                {
                    foreach (var item in ccList)
                        mail.CC.Add(new MailAddress(item));
                }
                mail.Subject = subject;
                mail.BodyEncoding = Encoding.Default;
                mail.Priority = MailPriority.Normal;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(server, port);
                smtp.UseDefaultCredentials = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(username, password);
                smtp.Timeout = 10000;
                smtp.Send(mail);
                return true;
            }
            catch (Exception exp)
            {
                errorMsg = exp.Message;
                return false;
            }
        }
        #endregion


        #region 发送邮件
        public static bool Send(string from, string to, string subject, string body, out string errorMsg)
        {
            errorMsg = "";
            List<string> listCC = new List<string>() { };
            string smtpServer = ConfigHelper.GetConfigValue("SmtpServer");
            int smtpPort = ConfigHelper.GetConfigValue("SmtpPort").ToInt(25);
            string smtpEmail = ConfigHelper.GetConfigValue("SmtpEmail");
            string smtpPwd = ConfigHelper.GetConfigValue("SmtpPwd");

            bool bo = Send(smtpEmail, from, to, listCC, subject, body, smtpEmail, smtpPwd, smtpServer, smtpPort, out errorMsg);
            return bo;
        }
        #endregion

    }
}
