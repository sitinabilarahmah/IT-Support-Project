using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Services
{
    public class RandomGenerator
    {
        private Random Rand = new Random();
        public int GenerateRandom()
        {
            return Rand.Next(1000, 9999);
        }
    }
    public class ServiceEmail
    {
        public void SendEmail(string sendEmail, string theCode)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.To.Add(new MailAddress(sendEmail));
            mail.From = new MailAddress("mcc50mcc@gmail.com", "NET CORE WEB API");
            mail.Subject = "VERIFICATION CODE: " + DateTime.Now.ToString();
            mail.Body = "Dear User, <br><br>Please sign up using this code to your application :<br><br><b>" + theCode + "</b><br><br> Thank you, <br> CJ Education";
            mail.IsBodyHtml = true;

            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential("mcc50mcc@gmail.com", "b00tcamp");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

        }
    }
}
