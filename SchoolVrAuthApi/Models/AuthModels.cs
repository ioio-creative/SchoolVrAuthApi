using System;
using System.Collections.Generic;

namespace SchoolVrAuthApi.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Name { get; set; }

        public virtual List<LicenseKey> LicenseKeys { get; set; }
    }

    public class LicenseKey
    {
        public string LicenseKeyId { get; set; }
        public bool IsActive { get; set; }
        public int NumOfMacAddressesAllowed { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual List<MacAddress> MacAddresses { get; set; }
    }

    public class MacAddress
    {
        public string MacAddressId { get; set; }
        public DateTime AssignedDt { get; set; }

        public string LicenseKeyId { get; set; }
        public virtual LicenseKey LicenseKey { get; set; }
    }
}
