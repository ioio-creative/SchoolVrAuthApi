using System;
using System.Runtime.Serialization;

namespace SchoolVrAuthApi.Exceptions
{
    public class AuthCodeFailException : Exception
    {        
        public AuthCodeFailException(string message) : base(message) { }        


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
