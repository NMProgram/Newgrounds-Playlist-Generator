using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using NAudio.Wave;
using SQLitePCL;
using TagLib.IFD.Entries;

[ExcludeFromCodeCoverage]
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
        '3' => new PlaylistMenu().Start,
        'Q' => () => Environment.Exit(0),
        _ => () => { Console.WriteLine("\nInvalid Key!"); Console.ReadKey(); }
    };
    protected void PrintDetails<T>(T? obj)
    {
        Console.WriteLine("Found result:\n");
        string msg = obj is null ? "No results found." : obj.ToString()!;
        Console.WriteLine(msg);
        AskEnter();
    }
    protected void PrintDetails<T>(T[] objs, Func<T, string> getter)
    {
        switch (objs.Length)
        {
            case 0: Console.WriteLine("No results found."); AskEnter(); return;
            case 1: PrintDetails(objs[0]); return;
        }
        Console.WriteLine($"Found {objs.Length} results:\n");
        for (int i = 0; i < objs.Length; i++)
        {
            T? obj = objs[i];
            Console.WriteLine($"#{i + 1}: {getter(obj)}");
        }
    }
    protected string Default(string msg) => Validate(msg);
    protected long GetOldID(string msg) => Validate(msg, _sLogic.IsInDatabase);
    protected long GetNewID(string msg) => Validate(msg, _sLogic.IsNotInDatabase);
    protected long GetNewLevelID(string msg) => Validate(msg, _sLogic.IsUniqueLevelID);
    protected string GetOldName(string msg) => Validate(msg, _cLogic.IsInDatabase);
    protected string GetNewName(string msg) => Validate(msg, _cLogic.IsNotInDatabase);
    protected string GetString(string msg) => Validate(msg, InputLogic.IsNotEmpty);
    protected long GetInteger(string msg) => Validate(msg, InputLogic.IsValidInteger);
    protected DateTime GetDate(string msg) => Validate(msg, InputLogic.IsValidDate);
    protected DateTime GetDate(string msg, long birthYear) => Validate(msg, x => InputLogic.IsValidDate(x, birthYear));
    protected Genre GetGenre(string msg) => Validate(msg, InputLogic.IsValidGenre);
    protected sbyte GetAvailable(string msg) => Validate(msg, InputLogic.IsValidAvailability);
    protected long GetAge(string msg, int year) => Validate(msg, x => InputLogic.IsValidAge(x, year));
    protected long GetNewSongID(string msg, Composer comp) => Validate(msg, x => _cLogic.IsNewSong(comp.Name, x));
    protected long GetOldSongID(string msg, Composer comp) => Validate(msg, x => _cLogic.IsNotNewSong(comp.Name, x));
    protected bool GetOption(string msg) => Validate(msg, InputLogic.IsBoolean);
    protected byte[] GetAudio(string msg) => Validate(msg, InputLogic.IsValidMP3);
    protected long GetBetweenValues<T>(string msg, long min, T[] max, Func<T, string> getter) 
        => Validate(msg, x => InputLogic.IsBetween(x, min, max.Length), () => PrintDetails(max, getter));
    
}