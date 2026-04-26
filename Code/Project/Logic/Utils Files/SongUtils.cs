public static class SongUtils
{
    public static string FormatSongID(this object s) => ((Song)s).ID.ToString();
}