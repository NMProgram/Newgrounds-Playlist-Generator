using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using NAudio.Wave;
using SQLitePCL;
[ExcludeFromCodeCoverage]
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
    
    void Add()
    {
        TaskRunner runner = new();
        runner.Add("ID", () => GetNewID(_prompts[0]))
        .Add("Name", () => GetString(_prompts[1]))
        .Add("Release Date", () => GetDate(_prompts[2]), d => ((DateTime)d).FormatDate())
        .Add("Genre", () => GetGenre(_prompts[3]), g => ((Genre)g).GetGenreName())
        .Add("Level ID", () => GetNewLevelID(_prompts[4]))
        .Add("Available on Newgrounds", () => GetAvailable(_prompts[5]), sb => ((sbyte)sb).FormatSbyte())
        .Add("Audio File", () => GetAudio(_prompts[6]), a => ((byte[])a).FormatAudio())
        .RunTasks()
        .Deconstruct(out long id, out string name, out DateTime releaseDate, out Genre genre, out long levelID, out sbyte available, out byte[] audio);
        Song song = new(id, name, releaseDate.ToString("yyyy-MM-dd HH:mm:ss"), (int)genre, levelID, available, audio);
        _sLogic.Add(song);
        Console.WriteLine($"Successfully added the following Song Details:\n\n{song}");
        AskEnter();
    }
    void Delete()
    {
        Song song = null!; TaskRunner runner = new();
        object SetSong()
        {
            long oldID = GetOldID(_prompts[0].Insert(_prompts[0].Length - 2, " to delete"));
            song = _sLogic.GetByID(oldID)!;
            return song;
        }
        runner.Add("Old ID", SetSong, s => ((Song)s).FormatSongID())
        .Add("Confirm", () => Delete(song))
        .RunTasks()
        .Deconstruct(out song, out bool delete);
        if (!delete) { Console.WriteLine($"Cancelled deletion of \'{song.Name}\'."); }
        else
        {
            _sLogic.Delete(song);
            Console.WriteLine($"Successfully deleted the following Song Details:\n\n{song}");
        }
        AskEnter();
    }
}