using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

[ExcludeFromCodeCoverage]
public class UpdateCompMenu : AlterCompMenu
{
    protected override string MenuStr => @"
    [1] Add Song
    [2] Update Song
    [3] Remove Song
    [4] Update Name
    [5] Update Join Date
    [6] Update Age
    [7] Update Description
    [8] Update Availability
    [Q] Return to Alter Composer Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => () => CheckActivity(AddSong),
        '2' => () => CheckActivity(UpdateSong),
        '3' => () => CheckActivity(RemoveSong),
        '4' => () => CheckActivity(Name),
        '5' => () => CheckActivity(Date),
        '6' => () => CheckActivity(Age),
        '7' => () => CheckActivity(Description),
        '8' => () => CheckActivity(Available),
        _ => () => _active = false
    };
    void Updater<T>(TaskRunner runner, Action<Composer, T> setter, Func<Composer, T, string> message)
    {
        Updater(runner, setter, message, _cLogic.Update);
    }
    object GetOldName(ref Composer comp)
    {
        _name = GetOldName(_prompts[0]); comp = _cLogic.GetByID(_name)!; return comp;
    }
    void AddSong()
    {
        Song song = null!; Composer comp = null!; TaskRunner runner = new();
        runner.Add("Old Name", () => GetOldName(ref comp), c => ((Composer)c).FormatCompName())
        .Add("New Name", () => _sLogic.GetByID(GetNewSongID("Enter the ID of the Song to add: ", comp))!)
        .RunTasks()
        .Deconstruct(out comp, out song);
        _cLogic.AddSong(comp, song);
        comp.AddSong(song);
        Console.WriteLine($"Successfully added the following Song Details under the name \'{comp.Name}\':\n\n{song}");
        AskEnter();
    }
    void UpdateSong()
    {
        Composer comp = null!; TaskRunner runner = new();
        runner.Add("Old Name", () => GetOldName(ref comp), c => ((Composer)c).FormatCompName())
        .Add("Old Song ID", () => _sLogic.GetByID(GetOldSongID("Enter the ID of the Song to replace: ", comp))!, s => ((Song)s).FormatSongID())
        .Add("New Song ID", () => _sLogic.GetByID(GetNewSongID(NamedPrompt(5), comp))!, s => ((Song)s).FormatSongID())
        .RunTasks()
        .Deconstruct(out comp, out Song oldSong, out Song newSong);
        _cLogic.UpdateSong(comp, oldSong, newSong);
        comp.UpdateSong(oldSong.ID, newSong);
        Console.WriteLine(UpdateMsg($"Song with Song ID {oldSong.ID}", comp, oldSong.Name, newSong.Name));
        Console.WriteLine($"\nNew Composer:\n{comp}");
        AskEnter();
    }
    void RemoveSong()
    {
        Song song = null!; Composer comp = null!; TaskRunner runner = new();
        runner.Add("Old Name", () => GetOldName(ref comp), c => ((Composer)c).FormatCompName())
        .Add("Old Song ID", () => { song = _sLogic.GetByID(GetOldSongID("Enter the ID of the Song to remove: ", comp))!; return song; }, s => ((Song)s).FormatSongID())
        .Add("Confirm", () => GetOption($"Are you sure you want to delete the Song \'{song.Name.Bold()}\' from the Composer \'{comp.Name.Bold()}\'?\nEnter your choice here: "))
        .RunTasks()
        .Deconstruct(out comp, out song, out bool delete);
        if (!delete) { Console.WriteLine($"Cancelled deletion of \'{song.Name.Bold()}\' from \'{comp.Name.Bold()}\''s collection."); return; }
        _cLogic.RemoveSong(comp, song);
        comp.RemoveSong(song);
        Console.WriteLine($"Successfully removed the following Song Details under the name {comp.Name.Bold()}:\n\n{song}");
        AskEnter();
    }
    void Name()
    {
        TaskRunner runner = new();
        runner.Add("Old Name", () => { _name = GetOldName(_prompts[0]); return _cLogic.GetByID(_name)!; }, c => ((Composer)c).FormatCompName())
        .Add("New Name", () => GetNewName(_prompts[0]));
        Updater<string>(runner, (c, val) => c.SetName(val),
        (c, val) => UpdateMsg("Name", c) + $" to {val.Bold()}!");
    }
    public void Date()
    {
        TaskRunner runner = new();
        Composer comp = null!;
        runner.Add("Old Name", () => GetOldName(ref comp), c => ((Composer)c).FormatCompName())
        .Add("New Join Date", () => GetDate(NamedPrompt(1), comp.BirthYear), d => ((DateTime)d).FormatDate());
        Updater<DateTime>(runner, (c, val) => c.SetJoinDate(val), 
        (c, val) => UpdateMsg("Join Date", c, c.JoinDate.ToString("MMM dd, yyyy"), val.ToString("MMM dd, yyyy")));
    }
    void Age()
    {
        TaskRunner runner = new();
        Composer comp = null!;
        runner.Add("Old Name", () => GetOldName(ref comp), c => ((Composer)c).FormatCompName())
        .Add("New Birth Year", () => GetAge(NamedPrompt(2), comp.JoinDate.Year), a => ((long)a).AgeStr());
        Updater<long>(runner, (c, val) => c.SetAge(val), 
        (c, val) => UpdateMsg("Birth Year", c, c.GetAge().AgeStr(), val.AgeStr()));
    }
    void Description()
    {
        TaskRunner runner = new();
        runner.Add("Old Name", () => { _name = GetOldName(_prompts[0]); return _cLogic.GetByID(_name)!; }, c => ((Composer)c).FormatCompName())
        .Add("New Description", () => Default(NamedPrompt(3)).Replace("\\n", "\n"), d => ((string)d).DescPrinter());
        Updater<string>(runner, (c, val) => c.SetDescription(val), 
        (c, val) => UpdateMsg("Description", c, c.Description.DescPrinter().Bold(), val.DescPrinter().Bold()));
    }
    void Available()
    {
        TaskRunner runner = new();
        runner.Add("Old Name", () => { _name = GetOldName(_prompts[0]); return _cLogic.GetByID(_name)!; }, c => ((Composer)c).FormatCompName())
        .Add("New Availability on NG", () => GetAvailable(NamedPrompt(4)), sb => ((sbyte)sb).FormatSbyte());
        Updater<sbyte>(runner, (c, val) => c.SetAvailability(val), 
        (c, val) => $"{UpdateMsg("Availability", c)} on Newgrounds {UpdateMsg(c.OnNewgrounds > 1, val > 1)}");
    }
}