using SchoolVrAuthApi.Models;
using System;

namespace SchoolVrAuthApi.Exceptions
{
    public class AuthenticateLicenseFailException : Exception
    {
        public readonly AuthResponseBody AuthResponse;

       
        public AuthenticateLicenseFailException(string message) : base(message) { }


        public AuthenticateLicenseFailException(AuthResponseBody authResponseBody) : base(ToMessage(authResponseBody))
        {
            AuthResponse = authResponseBody;
        }

        private static string ToMessage(AuthResponseBody authResponseBody)
        {
            return authResponseBody.IsAuthenticated ?
                "Authenticate license succeeded." : "Authenticate license failed.";
        }
    }
}
