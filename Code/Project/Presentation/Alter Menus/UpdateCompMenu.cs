public class UpdateCompMenu : AlterCompMenu
{
    protected override string MenuStr => @"
    [1] Add Song
    [2] Update Song
    [3] Update Name
    [4] Update Join Date
    [5] Update Age
    [6] Update Description
    [7] Update Availability
    [Q] Return to Alter Composer Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => () => CheckActivity(AddSong),
        '2' => () => CheckActivity(UpdateSong),
        '3' => () => CheckActivity(Name),
        '4' => () => CheckActivity(Date),
        '5' => () => CheckActivity(Age),
        '6' => () => CheckActivity(Description),
        '7' => () => CheckActivity(Available),
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
        return i;
    }
    void UpdateData<T>(string type, Action<Composer> action, Func<Composer, T> getter)
    {
        UpdateData(type, _prompts[0], InputLogic.IsNotEmpty, action, getter);
        AskEnter();
    }
    void AddSong()
    {
        string name = Validate(_prompts[0], InputLogic.IsNotEmpty, _cLogic.IsInDatabase);
        long id = Validate("Enter the ID of the Song to add: ", 
        x => CheckSongIDs(x, name, _cLogic.IsNewSong));
        Song song = _sLogic.GetByID(id)!;
        Composer o = _cLogic.GetByID(name)!; Composer n = (o.Clone() as Composer)!;
        n.AddSong(song); _cLogic.Update(o, n);
        Console.WriteLine($"Successfully added the following Song Details under the name \'{name}\':\n\n{song}");
    }
    void UpdateSong()
    {
        int index = 0;
        UpdateData("Song", newComp => { index = ReplaceSong(newComp); }, 
        any => any.Songs[index].Name);
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