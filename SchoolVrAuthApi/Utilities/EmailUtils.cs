using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SchoolVrAuthApi.Utilities
{
    public class EmailUtils
    {
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SmtpUserName = "smtpclient2.ioio@gmail.com";
        private const string SmtpPassword = "EKWaVG54";
        private const bool SmtpIsEnableSsl = true;

        private const string ErrMsgFromAddr = @"SchoolVrAuthApi Error <smtpclient2.ioio @gmail.com>";

        private static SmtpClient GetNewSmtpClient()
        {
            NetworkCredential credential = new NetworkCredential()
            {
                UserName = SmtpUserName,
                Password = SmtpPassword
            };

            return new SmtpClient()
            {
                Credentials = credential,
                Host = SmtpHost,
                Port = SmtpPort,
                EnableSsl = SmtpIsEnableSsl

                // The following statement does not get credentials
                // from web.config
                // Hence, it's completely wrong
                //smtp.UseDefaultCredentials = true;
            };
        }

        private static void AddMultipleRecipientsToMailMessage(MailMessage mailMessage, IEnumerable<string> receiverAddrs)
        {
            foreach (string receiverAddr in receiverAddrs)
            {
                mailMessage.To.Add(receiverAddr);
            }
        }

        public async static Task SendInternalErrorNotificationAsync(string receiverAddr, Exception exc, IHttpContextAccessor httpContextAccessor)
        {
            await SendInternalErrorNotificationAsync(new string[] { receiverAddr }, exc, httpContextAccessor);
        }

        public async static Task SendInternalErrorNotificationAsync(IEnumerable<string> receiverAddrs, Exception exc, IHttpContextAccessor httpContextAccessor)
        {
            StringBuilder MessageBodyBuilder = new StringBuilder();
            MessageBodyBuilder.AppendFormat("********** {0} **********", DateTime.Now);
            MessageBodyBuilder.AppendLine("");
            MessageBodyBuilder.Append("Client IP Address: ");
            // https://stackoverflow.com/questions/51116403/how-to-get-client-ip-address-in-asp-net-core-2-1/51245326
            MessageBodyBuilder.AppendLine(httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString());
            MessageBodyBuilder.AppendLine("");
            if (exc.InnerException != null)
            {
                MessageBodyBuilder.Append("Inner Exception Type: ");
                MessageBodyBuilder.AppendLine(exc.InnerException.GetType().ToString());
                MessageBodyBuilder.Append("Inner Exception: ");
                MessageBodyBuilder.AppendLine(exc.InnerException.Message);
                MessageBodyBuilder.Append("Inner Source: ");
                MessageBodyBuilder.AppendLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    MessageBodyBuilder.AppendLine("Inner Stack Trace: ");
                    MessageBodyBuilder.AppendLine(exc.InnerException.StackTrace);
                }
            }
            MessageBodyBuilder.Append("Exception Type: ");
            MessageBodyBuilder.AppendLine(exc.GetType().ToString());
            MessageBodyBuilder.AppendLine("Exception: " + exc.Message);
            MessageBodyBuilder.AppendLine("Source: " + exc.Source);
            MessageBodyBuilder.AppendLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                MessageBodyBuilder.AppendLine(exc.StackTrace);
                MessageBodyBuilder.AppendLine();
            }
            string messageBody = MessageBodyBuilder.ToString();

            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(ErrMsgFromAddr);
                    AddMultipleRecipientsToMailMessage(message, receiverAddrs);
                    message.Subject = "SchoolVrAuthApi Exception";
                    message.Body = messageBody;
                    message.IsBodyHtml = false;

                    using (SmtpClient smtp = GetNewSmtpClient())
                    {
                        await smtp.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Silence exception when sending email?
                Console.WriteLine("Could not send internal error notification e-mail." + Environment.NewLine +
                    "Exception caught: " + ex);
            }
        }
    }
}
