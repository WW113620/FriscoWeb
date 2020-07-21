using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FriscoDev.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string to = "WW113620@163.com";
                string smtpHost = "smtp.stalkerradar.com";
                smtpHost = "stalkerradar-com.mail.protection.outlook.com";

                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress("noreply@acicovidteam.com");
                mailMsg.To.Add(new MailAddress(to));
                mailMsg.Subject = "COPTRAX TEST MESSAGE RESPONSE";
                mailMsg.Body = "This email is an automated response to your test request from CopTrax";
                mailMsg.BodyEncoding = Encoding.ASCII;
                SmtpClient smtpClient = new SmtpClient(smtpHost);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("coptraxsvc@a-concepts.com", "Coptrax456");
                smtpClient.Send(mailMsg);
               // smtpClient.SendCompleted += SmtpClient_SendCompleted;
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }


        }

        private static void SmtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            System.Console.WriteLine(e.UserState);
            System.Console.WriteLine(e.UserState.ToString());
        }

    }
}
