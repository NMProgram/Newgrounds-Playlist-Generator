using System.Diagnostics.CodeAnalysis;

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
    (bool, long, string?) CheckSongIDs(string inp, string name, Func<string, long, (bool, long, string?)> func)
    {
        var tuple = CheckID(inp, _sLogic.IsInDatabase);
        if (!tuple.Item1) { return tuple; }
        return func(name, tuple.Item2);
    }
    int ReplaceSong(Composer comp)
    {
        long oldID = Validate("Enter the old ID of the Song to replace: ", 
        x => CheckSongIDs(x, comp.Name, _cLogic.IsNotNewSong));
        Song song = GetSong();
        int i = comp.UpdateSong(oldID, song);
        _cLogic.UpdateSong(comp, _sLogic.GetByID(oldID)!, song);
        return i;
    }
    protected void UpdateData<T>(string type, Action<Composer> action, Func<Composer, T> getter, Func<T, T, string>? message = null)
    {
        string oldName = Validate(_prompts[0], x => ValidString(x, _cLogic.IsInDatabase));
        var (o, n) = SetUpdate(oldName, _cLogic, action);
        string msg = message is not null ? message(getter(o), getter(n)) : 
        $"Successfully changed the {type} of the Composer {o.Name.Bold()} from \'{getter(o)}\' to \'{getter(n)}\'!";
        Console.WriteLine(msg);
        AskEnter();
    }
    void UpdateData<T>(Action<Composer> action, Func<Composer, T> getter, Func<T, T, string>? message)
    {
        UpdateData("", action, getter, message);
    }
    void AddSong()
    {
        Song song = null!;
        void Action(Composer comp)
        {
            long id = Validate("Enter the ID of the Song to add: ", 
            x => CheckSongIDs(x, comp.Name, _cLogic.IsNewSong));
            song = _sLogic.GetByID(id)!;
            comp.AddSong(song);
            _cLogic.AddSong(comp, song);
        }
        UpdateData(Action, c => c.Name, (name, _) => 
        $"Successfully added the following Song Details under the name \'{name}\':\n\n{song}");
    }
    void UpdateSong()
    {
        int index = 0;
        UpdateData("Song", newComp => { index = ReplaceSong(newComp); }, 
        any => any.Songs[index].Name);
    }
    public void RemoveSong()
    {
        Song song = null!;
        void Action(Composer comp)
        {
            long id = Validate("Enter the ID of the Song to remove: ",
            x => CheckSongIDs(x, comp.Name, _cLogic.IsNotNewSong));
            song = _sLogic.GetByID(id)!;
            comp.Songs.Remove(song);
            _cLogic.RemoveSong(comp, song);
        }
        UpdateData(Action, c => c.Name, (name, _) => 
        $"Successfully removed the following Song details under the name \'{name}\':\n\n{song}");
    }
    void Name()
    {
        UpdateData("Name", newComp => newComp.SetName(GetName()), any => any.Name);
    }
    public void Date()
    {
        UpdateData("Join Date", newComp => {
            DateTime date = Validate(NamedPrompt(1), x => ValidString(x, y => InputLogic.IsValidDate(y, newComp.BirthYear)));
            newComp.SetJoinDate(date);
        },
        any => any.JoinDate.ToString("MMM dd, yyyy"));
    }
    void Age()
    {
        UpdateData("Age", newComp => newComp.SetAge(GetAge(newComp.JoinDate.Year)),
        any => any.GetAge() > -1 ? any.GetAge().ToString() : "Unknown");
    }
    void Description()
    {
        Func<Composer, string> getter = any => any.Description.Replace("\n", " ").Bold();
        UpdateData("Description", newComp => newComp.SetDescription(GetDescription()), getter);
    }
    void Available()
    {
        UpdateData("Availability on Newgrounds", newSong => newSong.SetAvailability(GetAvailability()),
        any => any.OnNewgrounds == 1);
    }
}