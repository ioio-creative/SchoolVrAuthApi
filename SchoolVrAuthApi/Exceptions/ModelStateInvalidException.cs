using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace SchoolVrAuthApi.Exceptions
{
    public class ModelStateInvalidException : Exception
    {
        public readonly ModelStateDictionary ModelState;


        public ModelStateInvalidException() : base() { }
        public ModelStateInvalidException(string message) : base(message) { }
        public ModelStateInvalidException(string message, Exception inner) : base(message, inner) { }

        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/exceptions/creating-and-throwing-exceptions
        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected ModelStateInvalidException(SerializationInfo info,
            StreamingContext context) : base(info, context) { }


        public ModelStateInvalidException(object modelSubmitted, ModelStateDictionary modelState) : base(ToMessage(modelSubmitted))
        {
            ModelState = modelState;
        }

        private static string ToMessage(object modelSubmitted)
        {
            return "Invalid model submitted:" + Environment.NewLine +
                    JsonConvert.SerializeObject(modelSubmitted);
        }        
    }
}
