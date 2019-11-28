using System.ComponentModel.DataAnnotations;

namespace SchoolVrAuthApi.Models
{
    public class AuthRequestBody
    {
        [Required()]
        [StringLength(25)]  // depends on SchoolVrAuthApi.Controllers.AuthController.ApiAuthCode
        public string ApiAuthCode { get; set; }
        [Required()]
        [StringLength(Constants.DefaultIdStringMaxLength)]
        public string LicenseKey { get; set; }
        [Required()]
        [StringLength(Constants.DefaultIdStringMaxLength)]
        public string MacAddress { get; set; }
    }

    public class AuthResponseBody
    {
        public bool IsAuthenticated { get; set; }
        // for testing sending email at production server
        public string EmailNotificationErrMsg { get; set; }
    }
}
