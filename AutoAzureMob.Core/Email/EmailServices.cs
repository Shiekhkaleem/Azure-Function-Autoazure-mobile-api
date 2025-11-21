using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Core.Email
{
    public class EmailServices
    {
        public static void SendEmail(IConfiguration config, string template, string toMail, string subject)
        {
            try
            {
                // Set up email message
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(config.GetSection("SMTPConfig:MailFrom").Value.ToString());
                msg.To.Add(toMail);
                msg.Subject = subject;
                msg.Body = template;
                msg.IsBodyHtml = true;
                //msg.Priority = MailPriority.High;
                using (SmtpClient client = new SmtpClient())
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(config.GetSection("SMTPConfig:SMTPUser").Value.ToString(), config.GetSection("SMTPConfig:SMTPPass").Value.ToString());
                    client.Host = "smtp.sendgrid.net";
                    client.Port = 587;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    client.Send(msg);
                }
            }
            catch (SmtpException ex)
            {
                throw ex;
            }

        }
    }
}
