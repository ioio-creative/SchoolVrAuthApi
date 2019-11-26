using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
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
        private const bool SmtpIsEnableSsl = false;

        private const string ErrMsgFromName = "SchoolVrAuthApi";
        private const string ErrMsgFromAddr = "smtpclient2.ioio @gmail.com";

    
        private static void AddMultipleRecipientsToMailMessage(MimeMessage mimeMessage, IEnumerable<string> receiverAddrs)
        {
            foreach (var receiverAddr in receiverAddrs)
            {
                mimeMessage.To.Add(new MailboxAddress(receiverAddr));
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
                // https://github.com/jstedfast/MailKit
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(ErrMsgFromName, ErrMsgFromAddr));
                AddMultipleRecipientsToMailMessage(message, receiverAddrs);
                message.Subject = "SchoolVrAuthApi Exception";

                message.Body = new TextPart("plain")
                {
                    Text = messageBody
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(SmtpHost, SmtpPort, SmtpIsEnableSsl);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(SmtpUserName, SmtpPassword);

                    await client.SendAsync(message);
                    
                    await client.DisconnectAsync(true);
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
