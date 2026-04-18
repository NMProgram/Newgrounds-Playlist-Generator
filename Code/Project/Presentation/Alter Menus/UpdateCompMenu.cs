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
        '1' => AddSong,
        '2' => UpdateSong,
        '3' => Name,
        '4' => Date,
        '5' => Age,
        '6' => Description,
        '7' => Available,
        _ => () => _active = false
    };
    (bool, long, string?) CheckSongIDs(string inp, string name, Func<string, long, (bool, long, string?)> func)
    {
        var tuple = CheckID(inp, _access.IsInDatabase);
        if (!tuple.Item1) { return tuple; }
        return func(name, tuple.Item2);
    }
    int ReplaceSong(Composer comp)
    {
        long oldID = Validate("Enter the old ID of the Song to replace: ", x => CheckSongIDs(x, comp.Name, _access.IsNotNewSong));
        Song song = GetSong();
        int i = comp.UpdateSong(oldID, song);
        return i;
    }
    void UpdateData<T>(string type, Action<Composer> action, Func<Composer, T> getter)
    {
        UpdateData(type, _prompts[0], InputLogic.IsNotEmpty, action, getter);
    }
    void AddSong()
    {
        string name = Validate(_prompts[0], InputLogic.IsNotEmpty, _access.IsInDatabase);
        long id = Validate("Enter the ID of the Song to add: ", x => CheckSongIDs(x, name, _access.IsNewSong));
        Song song = _access.GetByID(id)!;
        Composer o = _access.GetByID(name)!; Composer n = (o.Clone() as Composer)!;
        n.AddSong(song); _access.Update(o, n);
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