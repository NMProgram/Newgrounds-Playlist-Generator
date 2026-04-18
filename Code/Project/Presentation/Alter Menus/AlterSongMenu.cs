using NAudio.Wave;
using SQLitePCL;

public class AlterSongMenu : AlterMenu
{
    protected readonly string[] _prompts = [
        "Enter the Song ID: ", "Enter the Song Name: ", "Enter the Release Date of the Song: ", 
        "Enter the Genre of the Song: ", "Enter the first Level ID associated with the Song: ", 
        "Enter the availability of the Song on Newgrounds: ", 
        "Enter the path to the audio file associated with this Song: "
        ];
    protected override string MenuStr => @"
    [1] Add Song
    [2] Update Song
    [3] Delete Song
    [Q] Return to Alter Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => () => CheckActivity(Add),
        '2' => new UpdateSongMenu().Start,
        '3' => () => CheckActivity(Delete),
        _ => () => _active = false
    };
    
    protected long GetID(Func<long, (bool, long, string?)> func, bool n = false) 
        => Validate(n ? _prompts[0].Insert(9, " new") : _prompts[0], x => CheckID(x, func));
    protected string GetName() => Validate(_prompts[1], InputLogic.IsNotEmpty);
    protected string GetReleaseDate() 
        => Validate(_prompts[2], x => ValidString(x, InputLogic.IsValidDate)).ToString("yyyy-MM-dd HH:mm:ss");
    protected Genre GetGenre() => Validate(_prompts[3], x => ValidString(x, InputLogic.IsValidGenre));
    protected long GetLevelID() => Validate(_prompts[4], x => CheckID(x, _sLogic.IsUniqueLevelID));
    protected byte GetAvailability() 
        => (byte) Validate(_prompts[5], x => ValidString(x, InputLogic.IsValidAvailability));
    protected byte[] GetAudioFile() => Validate(_prompts[6], x => ValidString(x, InputLogic.IsValidMP3)) ?? [];
    public void Add()
    {
        Song song = new(
            GetID(_sLogic.IsNotInDatabase), GetName(), GetReleaseDate(), (int)GetGenre(), 
            GetLevelID(), GetAvailability(), GetAudioFile()
        );
        _sLogic.Add(song);
        Console.WriteLine($"Successfully added the following Song Details:\n\n{song}");
        AskEnter();
    }
    void Delete()
    {
        long oldID = Validate(_prompts[0].Insert(_prompts[0].Length - 2, " to delete"), 
        x => CheckID(x, _sLogic.IsInDatabase));
        Song song = _sLogic.GetByID(oldID)!;
        string inp = Input($"Are you sure you want to delete the song \'{song.Name}\'?\nEnter your choice here: ");
        if (!inp.ToLower().StartsWith('y')) { Console.WriteLine($"Cancelled deletion of \'{song.Name}\'."); return; }
        Console.WriteLine($"Successfully deleted the following Song Details:\n\n{song}");
        AskEnter();
    }
}