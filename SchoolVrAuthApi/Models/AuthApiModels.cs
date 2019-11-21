namespace SchoolVrAuthApi.Models
{
    public class AuthRequestBody
    {
        public string ApiAuthCode { get; set; }
        public string LicenseKey { get; set; }
        public string MacAddress { get; set; }
    }

    public class AuthResponseBody
    {
        public bool IsAuthenticated { get; set; }
    }
}
