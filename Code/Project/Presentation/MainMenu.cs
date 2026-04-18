public class MainMenu : Menu
{
    protected override string MenuStr => @"
    [1] Alter Data
    [2] Filter Data
    [3] Run Playlist
    [Q] Quit
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => new AlterMenu().Start,
        '2' => new FilterMenu().Start,
        'Q' => () => Environment.Exit(0),
        _ => () => { Console.WriteLine("\nInvalid Key!"); Console.ReadKey(); }
    };
    protected (bool, long, string?) ValidID(string inp, Func<long, (bool, long, string?)> func)
    {
        (bool res, long val, string? err) = InputLogic.IsValidInteger(inp);
        return res ? func(val) : (res, default, err);
    }
    protected (bool, T?, string?) ValidString<T>(string inp, Func<string, (bool, T, string?)> func)
    {
        (bool res, _, string? err) = InputLogic.IsNotEmpty(inp);
        return res ? func(inp) : (res, default(T), err);
    }
    public (bool, long, string?) CheckID(string inp, Func<long, (bool, long, string?)> func)
    {
        return ValidString(inp, x => ValidID(x, func));
    }
}