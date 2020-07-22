using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Application.Common
{
    public class SendMail
    {

        #region send mail
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

                SmtpClient smtp = new SmtpClient();
                NetworkCredential nc = new NetworkCredential();
                nc.UserName = username;
                nc.Password = password;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = nc;
                smtp.EnableSsl = true;
                smtp.Port = port;
                smtp.Host = server;
                smtp.Send(mail);
                errorMsg = "success";
                return true;
            }
            catch (Exception exp)
            {
                errorMsg = exp.Message;
                return false;
            }
        }
        #endregion


        #region mail
        public static bool Send(string from, string to, string subject, string body, out string errorMsg)
        {
            errorMsg = "";
            List<string> listCC = new List<string>() { };
            string smtpServer = ConfigHelper.GetConfigValue("SmtpServer");
            int smtpPort = ConfigHelper.GetConfigValue("SmtpPort").ToInt(25);
            string smtpEmail = ConfigHelper.GetConfigValue("SmtpEmail");
            string smtpPwd = ConfigHelper.GetConfigValue("SmtpPwd");

            //bool bo = Send(smtpEmail, from, to, listCC, subject, body, smtpEmail, smtpPwd, smtpServer, smtpPort, out errorMsg);
            bool bo = SendMessageTo(from, to, subject, body, out errorMsg);
            return bo;
        }

        public static bool SendMessageTo(string from, string to, string subject, string body, out string errorMsg)
        {
            try
            {
                string smtpHost = "smtp.stalkerradar.com";
                smtpHost = "stalkerradar-com.mail.protection.outlook.com";

                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress(from);
                mailMsg.To.Add(new MailAddress(to));
                mailMsg.Subject = subject;
                mailMsg.Body = body;
                mailMsg.BodyEncoding = Encoding.ASCII;
                SmtpClient smtpClient = new SmtpClient(smtpHost);
                smtpClient.Credentials = new NetworkCredential("coptraxsvc@a-concepts.com", "Coptrax456");

                smtpClient.Send(mailMsg);

                errorMsg = "success";

                return true;
            }
            catch (Exception e)
            {
                errorMsg = "Exception=" + e.Message;
                return false;
            }

        }
        #endregion

    }
}
