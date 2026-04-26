public static class EnumUtils
{
    public static string FormatGenre(this object g) => ((Genre)g).GetGenreName();
}