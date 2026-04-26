public static class IntegerUtils
{
    public static string FormatSbyte(this object b) => (sbyte)b > 0 ? "Yes" : "No";
    public static string AgeStr(this object a) => (long)a > -1 ? ((long)a).ToYear().ToString() : "Unknown";
    public static long ToYear(this long age) => age == -1 ? age : DateTime.Today.Year - age;
}