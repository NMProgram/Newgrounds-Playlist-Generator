using System.Globalization;
using System.Numerics;
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
    public static (bool, T, string?) IsInOptions<T>(T val, T[] options) where T : INumber<T>
    {
        string err = $"{val} is not between {options.Min()} and {options.Max()}.";
        return options.Contains(val) ? (true, val, null) : (false, default!, err);
    }
    public static (bool, int, string?) IsValidInteger(string val)
    {
        return int.TryParse(val, out int res) && res >= 0 ?
        (true, res, null) : (false, -1, $"{val} is not a valid number.");
    }
    public static (bool, DateTime, string?) IsValidDate(string val)
    {
        string err = $"{val} is not a valid date.\nPlease enter one of the following formats: {string.Join('\n', _formats)}";
        return DateTime.TryParseExact(
            val, _formats, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out DateTime res
        ) ? (true, res, null) : (false, DateTime.MinValue, err);
    }
    public static (bool, DateTime, string?) IsValidDate(string val, long birthYear)
    {
        var tuple = IsValidDate(val);
        if (!tuple.Item1) { return tuple; }
        string date = tuple.Item2.ToString("MMM dd, yyyy");
        string err = $"The Join Date \'{date}\' cannot be earlier than the birth year \'{birthYear}\'.";
        return tuple.Item2.Year >= birthYear ? (true, tuple.Item2, null) : (false, DateTime.MinValue, err);
    }
    public static (bool, long, string?) IsValidAge(long age, int year)
    {
        int years = DateTime.Today.Year - year;
        string err = $"The Composer cannot be younger than the Join Date ({age} < {years}).";
        return age >= years ? (true, age, null) : (false, -1, err);
    }
    public static (bool, Genre, string?) IsValidGenre(string genre)
    {
        string[] names = [.. ConversionLogic.GetGenreNames().Order()];
        string err = $"{genre} is not a valid Genre.\nPlease enter one of the following options:\n{string.Join(", ", names)}";
        string newGenre = new([.. genre.Where(x => x != ' ' && x != '-')]);
        if (newGenre.Equals("R&B", StringComparison.OrdinalIgnoreCase))
        { return (true, Genre.RNB, null); }
        return Enum.TryParse(newGenre, true, out Genre res) ? (true, res, null) : (false, default, err);
    }
    public static (bool, int, string?) IsValidAvailability(string? available) => available?.ToLower() switch
    {
        "yes" or "true" or "1" => (true, 1, null),
        "no" or "false" or "0" => (true, 0, null),
        _ => (false, -1, "Please enter a valid availability.")
    };
    public static (bool, byte[], string?) IsValidMP3(string path)
    {
        if (path is null) 
        { return (false, [], $"The given file \' \' was not found.");  }
        try
        {
            path = path.Trim();
            path = path.Replace("\"", "").Replace("\'", "");
            byte[] bytes = File.ReadAllBytes(path);
            return ConversionLogic.VerifyMP3(path) ? (true, bytes, null) : (false, [], "The file given cannot be used as an MP3.");
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