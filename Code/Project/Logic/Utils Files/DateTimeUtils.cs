public static class DateTimeUtils
{
    public static string FormatDate(this object d) => ((DateTime)d).ToString("MMM dd, yyyy");
}
