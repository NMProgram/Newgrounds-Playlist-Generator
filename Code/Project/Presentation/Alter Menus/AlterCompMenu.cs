using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using NAudio.Wave;
using SQLitePCL;
[ExcludeFromCodeCoverage]
public class AlterCompMenu : AlterMenu
{
    protected readonly string[] _prompts = [
        "Enter the Composer's Name: ", "Enter the Join Date of the Composer: ",
        "Enter the Composer's Age: ", "Enter the Composer's Description: ",
        "Enter if the Composer is on Newgrounds: ",
        "Enter the ID of a Song from this Composer: "
        ];
    protected override string MenuStr => @"
    [1] Add Composer
    [2] Update Composer
    [3] Delete Composer
    [Q] Return to Alter Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => () => CheckActivity(Add),
        '2' => new UpdateCompMenu().Start,
        '3' => () => CheckActivity(Delete),
        _ => () => _active = false
    };
    protected string NamedPrompt(int index)
    {
        return _prompts[index].Replace("the Composer", _name);
    }
    
    
    Composer GetCompDetails()
    {
        DateTime joinDate = DateTime.MinValue; TaskRunner runner = new();
        runner.Add("Name", () => { _name = GetNewName(_prompts[0]); return _name; })
        .Add("Join Date", () => { joinDate = GetDate(NamedPrompt(1)); return joinDate; }, d => ((DateTime)d).FormatDate())
        .Add("Birth Year", () => GetAge(NamedPrompt(2), joinDate.Year), a => ((long)a).AgeStr())
        .Add("Description", () => Default(NamedPrompt(3)).Replace("\\n", "\n"), d => ((string)d).DescPrinter())
        .Add("Available on Newgrounds", () => GetAvailable(NamedPrompt(4)), sb => ((sbyte)sb).FormatSbyte())
        .Add("Song ID", () => _sLogic.GetByID(GetOldID(NamedPrompt(5)))!)
        .RunTasks()
        .Deconstruct(out string name, out joinDate, out long age, out string desc, out sbyte available, out Song song);
        Composer comp = new(1, name, joinDate.ToString("yyyy-MM-dd HH:mm:ss"), age.ToYear(), desc, available);
        comp.AddSong(song);
        return comp;
    }
    void Add()
    {
        Composer comp = GetCompDetails();
        _cLogic.Add(comp);
        Console.WriteLine($"Successfully added the following Composer Details:\n\n{comp}");
        AskEnter();
    }
    void Delete()
    {
        Composer comp = null!; TaskRunner runner = new();
        object GetComp()
        {
            string name = GetOldName(_prompts[0]);
            comp = _cLogic.GetByID(name)!;
            return comp;
        }
        runner.Add("Old Name", GetComp, c => ((Composer)c).FormatCompName())
        .Add("Confirm", () => Delete(comp), c => ((Composer)c).Name)
        .RunTasks()
        .Deconstruct(out comp, out bool delete);
        if (!delete) { Console.WriteLine($"Cancelled deletion of \'{comp.Name}\'."); }
        else
        {
            _cLogic.Delete(comp);
            Console.WriteLine($"Successfully deleted the following Composer Details:\n\n{comp}");
        }
        AskEnter();
    }
}