using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolVrAuthApi.Models
{
    // Entity Framework Core: Specifying data type length and precision
    // https://www.meziantou.net/entity-framework-core-specifying-data-type-length-and-precision.htm
    public class Constants
    {
        public const int DefaultIdStringMaxLength = 25;
        public const string DefaultIdStringType = "varchar(25)";

        public const int DefaultNameStringMaxLength = 50;
        public const string DefaultNameStringType = "nvarchar(50)";
        
        public const string DefaultDateTimeType = "smalldatetime";
    }


    // DataAnnotations: MaxLength vs StringLength
    // https://stackoverflow.com/questions/5717033/stringlength-vs-maxlength-attributes-asp-net-mvc-with-entity-framework-ef-code-f
    public class User
    {
        [Column(TypeName = Constants.DefaultIdStringType)]
        public string UserId { get; set; }
        [Column(TypeName = Constants.DefaultNameStringType)]
        public string Name { get; set; }

        public virtual List<LicenseKey> LicenseKeys { get; set; }
    }

    public class LicenseKey
    {
        [Column(TypeName = Constants.DefaultIdStringType)]
        public string LicenseKeyId { get; set; }
        public bool IsActive { get; set; }
        public int NumOfMacAddressesAllowed { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public virtual List<MacAddress> MacAddresses { get; set; }
    }

    public class MacAddress
    {
        [Column(TypeName = Constants.DefaultIdStringType)]
        public string MacAddressId { get; set; }
        [Column(TypeName = Constants.DefaultDateTimeType)]
        public DateTime? AssignedDt { get; set; }
        
        public string LicenseKeyId { get; set; }
        public virtual LicenseKey LicenseKey { get; set; }
    }
}
