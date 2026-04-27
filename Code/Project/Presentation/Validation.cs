using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public static class Validation
{
    const string _return = "huyT^&Y*9ni_J98y7z6r76tr78798h)U9u009-Fdr5r6u(*0i97y8t6Zr56)"; 
    // just here to prevent you from seeing the same prompt without explanation
    public static char InputKey(string msg)
    {
        Console.Write(msg);
        return Console.ReadKey().KeyChar;
    }
    public static string Input(string msg)
    {
        string inp = ""; ConsoleKey key;
        Console.Write(msg);
        while (true)
        {
            var info = Console.ReadKey(true);
            key = info.Key;
            switch (key)
            {
                case ConsoleKey.Backspace: 
                    if (inp.Length == 0) { break; }; Console.Write("\b \b"); inp = inp[..^1]; break;
                case ConsoleKey.Escape: throw new ReturnedException("Successfully returned to the latest Menu!");
                case ConsoleKey.LeftArrow: throw new TracebackException("Undo last action.");
                case ConsoleKey.Enter: Console.WriteLine(); return inp;
                case not ConsoleKey.Tab: 
                    Console.Write(info.KeyChar); inp += !char.IsControl(info.KeyChar) ? info.KeyChar : ""; break;
                default: break;
            }
        }
    }
    public static T Validate<T>(string msg, Func<string, (bool, T, string?)> func, Action? action = null)
    {
        while (true)
        {
            if (action is not null) { action(); }
            string inp = Input(msg);
            if (inp == _return) { continue; }
            var (res, val, err) = func(inp);
            if (!res && err is not null) 
            { Console.WriteLine($"Error: {err}".Bold()); Menu.AskEnter(); Console.Clear(); throw new ReturnedException(""); }
            else { Console.Clear(); return val; }
        }
    }
}