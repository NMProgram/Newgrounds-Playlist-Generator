using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection;
using System.Threading.Channels;
using NAudio.CoreAudioApi;
using SQLitePCL;
[ExcludeFromCodeCoverage]
public abstract class Menu
{
    protected bool _active = true;
    protected SongLogic _sLogic = new(new InFile());
    protected ComposerLogic _cLogic = new(new InFile());
    protected abstract string MenuStr { get; }
    protected abstract Action GetAction(char inp);
    static void Exit(string? msg)
    {
        Console.Clear();
        if (string.IsNullOrEmpty(msg)) { return; }
        Console.WriteLine(msg);
        AskEnter();
        Console.Clear();
    }
    protected void CheckActivity(Action action)
    {
        try { action(); }
        catch (ReturnedException ex) { Exit(ex.Message); }
        catch (TracebackException) { Exit(null); }
    }
    static string Clean(string str) => str.Replace("\r\n", "").Trim().Replace("    ", "\n");
    protected T Validate<T>(string msg, Func<string, (bool, T, string?)> func, Action? action = null) 
        => Validation.Validate(msg, func, action);
    protected string Validate(string msg)
        => Validate<string>(msg, str => (true, str, null));
    public static void AskEnter()
    {
        Validation.InputKey("\nPress any key to continue: ");
        Console.Clear();
    }
    public void Start()
    {
        char chr;
        do
        {
            Console.WriteLine(Clean(MenuStr));
            chr = Validation.InputKey("\nEnter your choice here: ");
            Action action = GetAction(char.ToUpper(chr));
            Console.Clear();
            action();
        }
        while (!chr.Equals('q') && _active);
    }
}