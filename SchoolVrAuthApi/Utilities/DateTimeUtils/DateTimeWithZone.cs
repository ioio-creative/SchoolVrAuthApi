using System;

namespace SchoolVrAuthApi.Utilities.DateTimeUtils
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.createcustomtimezone?view=netframework-4.8
    public class DateTimeWithZone
    {
        // Hong Kong time zone
        private const string displayName = "(GMT+08:00) Hong Kong Time";
        private const string standardName = "Hong Kong Time";
        private static readonly TimeSpan offset = new TimeSpan(08, 00, 00);
        private static readonly TimeZoneInfo hongKong = TimeZoneInfo.CreateCustomTimeZone(standardName, offset, displayName, standardName);

        public static DateTime Now
        {
            get
            {
                return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, hongKong);
            }
        }
    }
}
