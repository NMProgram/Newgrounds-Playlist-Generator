using System.Threading.Channels;

public abstract class Menu
{
    protected bool _active = true;
    protected AccessLogic _access = new(new InFile());
    protected abstract string MenuStr { get; }
    protected abstract Action GetAction(char inp);
    protected static char InputKey(string msg)
    {
        Console.Write(msg);
        return Console.ReadKey().KeyChar;
    }
    protected static string Input(string msg)
    {
        Console.Write(msg);
        return Console.ReadLine() ?? "";
    }
    static string Clean(string str) => str.Replace("\r\n", "").Trim().Replace("    ", "\n");
    protected T Validate<T>(string msg, params Func<string, (bool, T, string?)>[] funcs)
    {
        while (true)
        {
            string inp = Input(msg);
            for (int i = 0; i < funcs.Length; i++)
            {
                var (res, val, err) = funcs[i](inp);
                if (!res && err is not null) { Console.WriteLine($"Error: {err.Bold()}"); break; }
                else if (i == funcs.Length - 1) { return val; }
            }
        }
    }
    protected void AskEnter()
    {
        InputKey("\nPress any key to continue: ");
        Console.Clear();
    }
    public void Start()
    {
        
        char chr;
        do
        {
            Console.WriteLine(Clean(MenuStr));
            chr = InputKey("\nEnter your choice here: ");
            Action action = GetAction(char.ToUpper(chr));
            Console.Clear();
            action();
        }
        while (!chr.Equals('q') && _active);
    }
}