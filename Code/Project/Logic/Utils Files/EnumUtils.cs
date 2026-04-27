public static class EnumUtils
{
    public static string GetGenreName(this Genre genre) => genre.ToString().GetGenreName();
    public static IEnumerable<string> GetGenreNames()
    {
        foreach (string str in Enum.GetNames<Genre>())
        {
            yield return str.GetGenreName();
        }
    }
}