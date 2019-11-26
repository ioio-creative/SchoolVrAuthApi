using SchoolVrAuthApi.Models;
using System;
using System.Runtime.Serialization;

namespace SchoolVrAuthApi.Exceptions
{
    public class AuthenticateLicenseFailException : Exception
    {
        public readonly AuthResponseBody AuthResponse;


        public AuthenticateLicenseFailException() : base() { }
        public AuthenticateLicenseFailException(string message) : base(message) { }
        public AuthenticateLicenseFailException(string message, Exception inner) : base(message, inner) { }

        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/exceptions/creating-and-throwing-exceptions
        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected AuthenticateLicenseFailException(SerializationInfo info,
            StreamingContext context) : base(info, context) { }


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
