using System;
using System.Runtime.Serialization;

namespace SchoolVrAuthApi.Exceptions
{
    public class AuthCodeFailException : Exception
    {
        public AuthCodeFailException() : base() { }
        public AuthCodeFailException(string message) : base(message) { }
        public AuthCodeFailException(string message, Exception inner) : base(message, inner) { }

        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/exceptions/creating-and-throwing-exceptions
        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected AuthCodeFailException(SerializationInfo info,
            StreamingContext context) : base(info, context) { }


        public static AuthCodeFailException AuthCodeFailExceptionFactory(string authCodeSubmitted)
        {
            return new AuthCodeFailException(ToMessage(authCodeSubmitted));            
        }

        private static string ToMessage(string authCodeSubmitted)
        {
            return "Invalid auth code submitted:" + Environment.NewLine +
                authCodeSubmitted;
        }
    }
}
