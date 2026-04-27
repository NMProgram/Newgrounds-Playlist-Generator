using TagLib;
using System.Text;
using Microsoft.VisualBasic;
using System.Diagnostics.CodeAnalysis;
[ExcludeFromCodeCoverage]
public class UpdateSongMenu : AlterSongMenu
{
    protected override string MenuStr => @"
    [1] Update ID
    [2] Update Name
    [3] Update Release Date
    [4] Update Genre
    [5] Update Level ID
    [6] Update Availability
    [7] Update Audio File
    [Q] Return to Alter Song Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => () => CheckActivity(ID),
        '2' => () => CheckActivity(Name),
        '3' => () => CheckActivity(Date),
        '4' => () => CheckActivity(Genre),
        '5' => () => CheckActivity(LevelID),
        '6' => () => CheckActivity(Available),
        '7' => () => CheckActivity(AudioFile),
        _ => () => _active = false
    };
    void Updater<T>(TaskRunner runner, Action<Song, T> setter, Func<Song, T, string> message)
    {
        Updater(runner, setter, message, _sLogic.Update);
    }
    
    object GetOldID() => _sLogic.GetByID(GetOldID(_prompts[0].Insert(9, " old")))!;
    void ID()
    {
        TaskRunner runner = new();
        runner.Add("Old ID", GetOldID, s => ((Song)s).FormatSongID())
        .Add("New ID", () => GetNewID(_prompts[0].Insert(9, " new")));
        Updater<long>(runner, (s, val) => s.SetID(val), (s, val) => UpdateMsg("ID", s, s.ID, val));
    }
    void Name()
    {
        TaskRunner runner = new();
        runner.Add("Old ID", GetOldID, s => ((Song)s).FormatSongID())
        .Add("New Name", () => GetString(_prompts[1].Insert(9, " new")));
        Updater<string>(runner, (s, val) => s.SetName(val), (s, val) => UpdateMsg("Name", s) + $" to {val.Bold()}!");
    }
    void Date()
    {
        TaskRunner runner = new();
        runner.Add("Old ID", GetOldID, s => ((Song)s).FormatSongID())
        .Add("New Release Date", () => GetDate(_prompts[2].Insert(9, " new")), d => ((DateTime)d).FormatDate());
        Updater<DateTime>(runner, (s, val) => s.SetReleaseDate(val), 
        (s, val) => UpdateMsg("Release Date", s, s.ReleaseDate.ToString("MMM dd, yyyy"), val.ToString("MMM dd, yyyy")));
    }
    void Genre()
    {
        // static string Converter(object g) => ConversionLogic.GetGenreName(((Genre)g).ToString());
        TaskRunner runner = new();
        runner.Add("Old ID", GetOldID, s => ((Song)s).FormatSongID())
        .Add("New Genre", () => GetGenre(_prompts[3].Insert(9, " new")), g => ((Genre)g).GetGenreName());
        Updater<Genre>(runner, (s, val) => s.SetGenre((int)val), 
        (s, val) => UpdateMsg("Genre", s, s.Genre.GetGenreName(), val.GetGenreName()));
    }
    void LevelID()
    {
        TaskRunner runner = new();
        runner.Add("Old ID", GetOldID, s => ((Song)s).FormatSongID())
        .Add("New Level ID", () => GetNewLevelID(_prompts[4].Insert(9, " new")));
        Updater<long>(runner, (s, val) => s.SetLevelID(val), (s, val) => UpdateMsg("Level ID", s, s.LevelID, val));
    }
    void Available()
    {
        TaskRunner runner = new();
        runner.Add("Old ID", GetOldID, s => ((Song)s).FormatSongID())
        .Add("New Availability on NG", () => GetAvailable(_prompts[5].Insert(9, " new")), sb => ((sbyte)sb).FormatSbyte());
        Updater<sbyte>(runner, (s, val) => s.SetAvailable(val), 
        (s, val) => $"{UpdateMsg("Availability", s)} on Newgrounds {UpdateMsg(s.Available > 0, val > 0)}");
    }
    void AudioFile()
    {
        TaskRunner runner = new();
        runner.Add("Old ID", GetOldID, s => ((Song)s).FormatSongID())
        .Add("New Audio File", () => GetAudio(_prompts[6].Insert(9, " new")), a => ((byte[])a).FormatAudio());
        Updater<byte[]>(runner, (s, val) => s.SetAudio(val), 
        (s, val) => UpdateMsg("Audio", s, s.GetAudioTitle(), val.Title));
    }
    
}