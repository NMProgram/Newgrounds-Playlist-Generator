public static class IntegerUtils
{
    public static string FormatSbyte(this sbyte b) => b > 0 ? "Yes" : "No";
    public static string AgeStr(this long a) => a > -1 ? a.ToYear().ToString() : "Unknown";
    public static long ToYear(this long age) => age == -1 ? age : DateTime.Today.Year - age;
}