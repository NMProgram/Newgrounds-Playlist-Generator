using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
public static class InputLogic
{
    static readonly string[] _formats = [
        "MM/dd/yyyy",
        "MM/d/yyyy",
        "M/dd/yyyy",
        "M/d/yyyy",
        "MM/dd/yy",
        "M/dd/yy",
        "MM/d/yy",
        "M/d/yy",
        "MMM d, yyyy",
        "MMM dd, yyyy"
    ];
    public static (bool, string, string?) IsNotEmpty(string str)
        => !string.IsNullOrEmpty(str) ? (true, str, null) : (false, "", "Please enter at least one character.");
    public static (bool, long, string?) IsBetween(string val, long low, long high)
    {
        var (res, intVal, intErr) = IsValidInteger(val);
        if (!res) { return (res, intVal, intErr); }
        string err = $"{intVal} is not between {low} and {high}.";
        return intVal >= low && intVal <= high ? (true, intVal, null) : (false, default, err);
    }
    public static (bool, bool, string?) IsBoolean(string str)
    {
        var (res, _, err) = IsNotEmpty(str);
        if (!res) { return (res, false, err); }
        str = str.ToLower();
        return str.StartsWith('y') || str.StartsWith('n') ? (true, str.StartsWith('y'), null) : 
        (false, false, $"Please enter an answer starting with {"y".Bold()} or {"n".Bold()}.");
    }
    public static (bool, long, string?) IsValidInteger(string val)
    {
        var (res, _, err) = IsNotEmpty(val);
        if (!res) { return (res, -1, err); }
        return long.TryParse(val, out long intRes) && intRes >= 0 ?
        (true, intRes, null) : (false, -1, $"{val} is not a valid number.");
    }
    public static (bool, DateTime, string?) IsValidDate(string val)
    {
        var (res, _, err) = IsNotEmpty(val);
        if (!res) { return (false, DateTime.MinValue, err); }
        return TestDate(val);
    }
    public static (bool, DateTime, string?) IsValidDate(string date, long birthYear)
    {
        var (res, val, err) = IsValidDate(date);
        if (!res) { return (false, DateTime.MinValue, err); }
        string ageErr = $"The Join Date \'{date:MMM dd, yyyy}\' cannot be earlier than the birth year \'{birthYear}\'.";
        return val.Year >= birthYear ? (true, val, null) : (false, DateTime.MinValue, ageErr);
    }
    public static (bool, long, string?) IsValidAge(string age, int year)
    {
        var (res, val, err) = IsValidInteger(age);
        if (!res) { return (false, -1, err); }
        return TestAge(val, year);
    }
    public static (bool, Genre, string?) IsValidGenre(string genre)
    {
        var (res, _, err) = IsNotEmpty(genre);
        if (!res) { return (false, default, err); }
        return TestGenre(genre);
    }
    public static (bool, sbyte, string?) IsValidAvailability(string available)
    {
        var (res, _, err) = IsNotEmpty(available);
        if (!res) { return (false, -1, err); }
        return TestAvailability(available);
    }
    public static (bool, byte[], string?) IsValidMP3(string path)
    {
        var (res, _, err) = IsNotEmpty(path);
        if (!res) { return (false, [], err); }
        return TestMP3(path);
    }
    static (bool, DateTime, string?) TestDate(string val)
    {
        string err = $"{val} is not a valid date.\nPlease enter one of the following formats: {'\n' + string.Join(", ", _formats)}";
        return DateTime.TryParseExact(
            val, _formats, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateTime dt
        ) ? (true, dt, null) : (false, DateTime.MinValue, err);
    }
    static (bool, long, string?) TestAge(long age, int year)
    {
        int years = DateTime.Today.Year - year;
        string err = $"The Composer cannot be younger than the Join Date ({age} <= {years}).";
        return age >= years ? (true, age, null) : (false, -1, err);
    }
    static (bool, Genre, string?) TestGenre(string genre)
    {
        string[] names = [.. EnumUtils.GetGenreNames().Order()];
        string err = $"{genre} is not a valid Genre.\nPlease enter one of the following options:\n{string.Join(", ", names)}";
        string newGenre = new([.. genre.Where(x => x != ' ' && x != '-')]);
        if (newGenre.Equals("R&B", StringComparison.OrdinalIgnoreCase))
        { return (true, Genre.RNB, null); }
        return Enum.TryParse(newGenre, true, out Genre res) ? (true, res, null) : (false, default, err);
    }
    static (bool, sbyte, string?) TestAvailability(string available) 
    => available?.ToLower() switch
        {
            "yes" or "true" or "1" => (true, 1, null),
            "no" or "false" or "0" => (true, 0, null),
            _ => (false, -1, "Please enter a valid availability.")
        };
    static (bool, byte[], string?) TestMP3(string path)
    {
        try
        {
            path = path.Trim();
            path = path.Replace("\"", "").Replace("\'", "");
            byte[] bytes = File.ReadAllBytes(path);
            return path.VerifyMP3() ? (true, bytes, null) : (false, [], "The given file given cannot be used as an MP3.");
        }
        catch
        {
            int lastIndex = Math.Max(path.LastIndexOf('\\'), path.LastIndexOf('/'));
            if (lastIndex == -1) { return (false, [], $"The given file \'{path}\' was not found."); }
            string fileName = ".." + path[lastIndex..];
            return (false, [], $"The given file \'{fileName}\' was not found.");
        }
    }

}