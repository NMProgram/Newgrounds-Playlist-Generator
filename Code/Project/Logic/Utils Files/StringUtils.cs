using System.Reflection;
using SQLitePCL;

public static class StringUtils
{
    public static string Bold(this string str) => $"\u001b[1m{str}\u001b[0m";
    public static string DescPrinter(this string d) => d.Replace("\n", "; ");
    public static string GetGenreName(this string str)
    {
        if (str == "RNB") { return "R&B"; }
        string newStr = "";
        foreach (char chr in str)
        {
            bool isUpper = char.IsUpper(chr) && newStr.Count(x => x != ' ') > 0 && newStr.Count(x => x != ' ') <= str.Length - 1;
            newStr += isUpper ? " " : "";
            newStr += chr;
        }
        if (newStr.Contains("Hip Hop")) { newStr = newStr.Insert(7, " -"); }
        return newStr;
    }
    public static bool VerifyMP3(this string path)
    {
        byte[] header = new byte[3];
        using FileStream fStream = new(path, FileMode.Open, FileAccess.Read);
        if (fStream.Length < 3) { return false; }
        fStream.ReadExactly(header);
        bool checkHeader = header[0] == 'I' && header[1] == 'D' && header[2] == '3';
        fStream.Seek(0, SeekOrigin.Begin);
        int byte1 = fStream.ReadByte(); int byte2 = fStream.ReadByte();
        bool checkFrameSync = byte1 == 0xFF && (byte2 & 0xE0) == 0xE0;
        return checkHeader || checkFrameSync;
    }
}