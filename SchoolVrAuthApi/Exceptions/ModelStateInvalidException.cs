using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;

namespace SchoolVrAuthApi.Exceptions
{
    public class ModelStateInvalidException : Exception
    {
        public readonly ModelStateDictionary ModelState;


        public ModelStateInvalidException(string message) : base(message) { }        


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
