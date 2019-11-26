using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace SchoolVrAuthApi.Utilities
{
    public class ExceptionUtils
    {
        private static readonly string[] ErrorNotificationEmailRecipients = new string[]
        {
            "christopher.wong@ioiocreative.com"
        };


        // Notify System Operators about an exception 
        public async static Task NotifySystemOps(Exception exc, IHttpContextAccessor httpContextAccessor)
        {
            await EmailUtils.SendInternalErrorNotificationAsync(
                ErrorNotificationEmailRecipients, exc, httpContextAccessor);
        }
    }
}
