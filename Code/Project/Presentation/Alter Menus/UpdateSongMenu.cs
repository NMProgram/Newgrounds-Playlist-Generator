using TagLib;
using System.Text;
using Microsoft.VisualBasic;

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
        '1' => ID,
        '2' => Name,
        '3' => Date,
        '4' => Genre,
        '5' => LevelID,
        '6' => Available,
        '7' => AudioFile,
        _ => () => _active = false
    };
    void UpdateData<T>(string type, Action<Song> action, Func<Song, T> getter)
    {
        UpdateData(type, _prompts[0], x => CheckID(x, _access.IsInDatabase), action, getter);
        AskEnter();
    }
    void ID()
    {
        UpdateData("ID", newSong => newSong.SetID(GetID(_access.IsNotInDatabase, true)), any => any.ID);
    }
    void Name()
    {
        UpdateData("Name", newSong => newSong.SetName(GetName()), any => any.Name);
    }
    void Date()
    {
        UpdateData("Release Date", newSong => newSong.SetReleaseDate(GetReleaseDate()),
        any => any.ReleaseDate);
    }
    void Genre()
    {
        UpdateData("Genre", newSong => newSong.SetGenre((int)GetGenre()),
        any => ConversionLogic.GetGenreName(any.Genre.ToString()));
    }
    void LevelID()
    {
        UpdateData("Level ID", newSong => newSong.SetLevelID(GetLevelID()), any => any.LevelID);
    }
    void Available()
    {
        UpdateData("Availability on Newgrounds", 
        newSong => newSong.SetAvailable(GetAvailability()), any => any.Available);
    }
    void AudioFile()
    {
        UpdateData("Audio File", newSong => newSong.SetAudio(GetAudioFile()),
        any => GetTitle(any.Audio));
    }
    string GetTitle(byte[] bytes)
    {
        using MemoryStream ms = new(bytes);
        var file = TagLib.File.Create(new PathAbstraction("in-memory.mp3", ms));
        return file.Tag.Title;
    }
}