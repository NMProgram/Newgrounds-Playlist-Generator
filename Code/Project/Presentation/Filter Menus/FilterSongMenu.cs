using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization.Metadata;
[ExcludeFromCodeCoverage]
public class FilterSongMenu : FilterMenu
{
    protected override string MenuStr => @"
    [1] Search Song with Closest ID
    [2] Search Song By Match
    [3] Songs Between IDs
    [4] Songs Between Level IDs
    [5] Songs Between Names
    [6] Songs Between Dates
    [7] Songs With Specific Genre
    [8] View Unavailable Songs
    [9] Songs From Composer
    [Q] Return to Filter Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => () => CheckActivity(ClosestID),
        '2' => () => CheckActivity(Match),
        '3' => () => CheckActivity(BetweenIDs),
        '4' => () => CheckActivity(BetweenLevelIDs),
        '5' => () => CheckActivity(BetweenNames),
        '6' => () => CheckActivity(BetweenDates),
        '7' => () => CheckActivity(WithGenre),
        '8' => () => CheckActivity(UnavailableSongs),
        '9' => () => CheckActivity(FromComposer),
        _ => () => _active = false
    };
    static string Getter(Song song) => $"\'{song.Name}\' (ID: {song.ID})";
    void ClosestID()
    {
        TaskRunner runner = new();
        object[] res = runner.Add("Song ID", () => GetInteger("Enter an ID: "))
        .RunTasks();
        Song? song = _sLogic.GetClosestMatch((long)res[0]);
        PrintDetails(song);
    }
    void Match()
    {
        FilterData("Enter a search term to filter by: ", "Search Term", GetString, _sLogic.GetSongMatches, Getter);
    }
    void BetweenIDs()
    {
        FilterData("Enter the first ID: ", $"Enter the last ID: ", "Song ID", GetInteger, _sLogic.GetBetweenSongData, Getter);
    }
    void BetweenLevelIDs()
    {
        FilterData("Enter the first Level ID: ", "Enter the last Level ID: ", "Level ID", GetInteger, _sLogic.GetBetweenLevelIDs, Getter);
    }
    void BetweenNames()
    {
        string start = "Enter the first name: ";
        string end = "Enter the last name: ";
        FilterData(start, end, "Name", GetString, _sLogic.GetBetweenSongData, Getter);
    }
    void BetweenDates()
    {
        string start = "Enter the starting date: ";
        string end = "Enter the ending date: ";
        FilterData(start, end, "Release Date", GetDate, _sLogic.GetBetweenSongData, Getter, d => ((DateTime)d).FormatDate());
    }
    public void WithGenre()
    {
        FilterData("Enter a Genre to filter by: ", "Genre", GetGenre, _sLogic.GetByGenre, Getter, g => ((Genre)g).GetGenreName());
    }
    void UnavailableSongs()
    {
        FilterData("", "", x => "", (_) => _sLogic.GetUnavailableSongs(), Getter);
    }
    void FromComposer()
    {
        FilterData("Enter the name of a Composer: ", "Composer Name", GetOldName, _sLogic.GetSongsFromComposer, Getter);
    }
}