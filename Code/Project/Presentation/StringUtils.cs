public static class StringUtils
{
    public static string Bold(this string str) => $"\u001b[1m{str}\u001b[0m";
}