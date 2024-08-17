public static class DateTimeExtensions
{
    public static DateTime ToManilaTime(this DateTime sendDate)
    {
        // Ensure the provided sendDate is in UTC
        DateTime utcTime = sendDate.ToUniversalTime();

        // Define the Singapore time zone
        TimeZoneInfo manilaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");

        // Return the converted the UTC time to Singapore time
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, manilaTimeZone);
    }
}