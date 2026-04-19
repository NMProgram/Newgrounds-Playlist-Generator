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
    (bool, long, string?) CheckAge(string inp, int year)
    {
        if (!InputLogic.IsNotEmpty(inp).Item1) { return (true, -1, null); }
        return ValidID(inp, x => InputLogic.IsValidAge(x, year));
    }
    // protected long GetID(Func<long, (bool, long, string?)> func, bool n = false) 
    //     => Validate(n ? _prompts[0].Insert(9, " new") : _prompts[0], x => CheckID(x, func));
    protected string GetName() => Validate(_prompts[0], InputLogic.IsNotEmpty);
    protected string GetName(Func<string, (bool, string, string?)> func) 
        => Validate(_prompts[0], InputLogic.IsNotEmpty, func);
    protected DateTime GetJoinDate() 
        => Validate(NamedPrompt(1), x => ValidString(x, InputLogic.IsValidDate));
    protected long GetAge(int year) => Validate(NamedPrompt(2), x => CheckAge(x, year));
    protected string GetDescription() => Input(NamedPrompt(3)).Replace("\\n", "\n");
    protected byte GetAvailability() 
        => (byte) Validate(NamedPrompt(4), x => ValidString(x, InputLogic.IsValidAvailability));
    protected Song GetSong() 
        => _sLogic.GetByID(
            Validate(
                NamedPrompt(5), 
                x => ValidString(
                    x, y => CheckID(y, _sLogic.IsInDatabase)
                )
            )
        )!;
    long GetBirthYear(long age) => age == -1 ? age : DateTime.Today.Year - age;
    Composer GetCompDetails()
    {
        string name = GetName(_cLogic.IsNotInDatabase);
        _name = name;
        DateTime joinDate = GetJoinDate();
        Composer comp = new(1, name, joinDate.ToString("yyyy-MM-dd HH:mm:ss"), 
        GetBirthYear(GetAge(joinDate.Year)), GetDescription(), GetAvailability());
        comp.AddSong(GetSong());
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
        string oldName = Validate(_prompts[0], InputLogic.IsNotEmpty, _cLogic.IsInDatabase);
        Composer comp = _cLogic.GetByID(oldName)!;
        string inp = Input($"Are you sure you want to delete the composer \'{comp.Name}\'?\nEnter your choice here: ");
        if (!inp.ToLower().StartsWith('y')) { Console.WriteLine($"Cancelled deletion of \'{comp.Name}\'."); return; }
        Console.WriteLine($"Successfully deleted the following Song Details:\n\n{comp}");
        AskEnter();
    }
}