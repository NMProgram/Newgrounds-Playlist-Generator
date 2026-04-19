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
    void IDChecker(Func<long, long, IEnumerable<Song>> selector, string prefix = "")
    {
        prefix = $" {prefix} ";
        long lowID = Validate($"Enter the first{prefix}ID: ", x => ValidString(x, InputLogic.IsValidInteger));
        long highID = Validate($"Enter the last{prefix}ID: ", x => ValidString(x, InputLogic.IsValidInteger));
        IEnumerable<Song> songs = selector(lowID, highID);
        PrintDetails(songs);
    }
    void PrintDetails(IEnumerable<Song> songs)
    {
        PrintDetails([..songs.DistinctBy(x => x.ID)], song => $"\'{song.Name}\' (ID: {song.ID})");
    }
    void ClosestID()
    {
        long id = Validate("Enter an ID: ", InputLogic.IsValidInteger);
        Song? song = _sLogic.GetClosestMatch(id);
        PrintDetails(song);
    }
    void Match()
    {
        string search = Input("Enter a search term to filter by: ");
        IEnumerable<Song> songs = _sLogic.GetSongMatches(search);
        PrintDetails(songs);
    }
    void BetweenIDs()
    {
        IDChecker(_sLogic.GetBetweenSongData);
    }
    void BetweenLevelIDs()
    {
        IDChecker(_sLogic.GetBetweenLevelIDs, "Level");
    }
    void BetweenNames()
    {
        string first = Validate("Enter the first name: ", InputLogic.IsNotEmpty);
        string last = Validate("Enter the last name: ", InputLogic.IsNotEmpty);
        IEnumerable<Song> songs = _sLogic.GetBetweenSongData(first, last);
        PrintDetails(songs);
    }
    void BetweenDates()
    {
        DateTime first = Validate("Enter the starting date: ", x => ValidString(x, InputLogic.IsValidDate));
        DateTime last = Validate("Enter the ending date: ", x => ValidString(x, InputLogic.IsValidDate));
        IEnumerable<Song> songs = _sLogic.GetBetweenSongData(first, last);
        PrintDetails(songs);
    }
    void WithGenre()
    {
        Genre genre = Validate("Enter a Genre to filter by: ", x => ValidString(x, InputLogic.IsValidGenre));
        IEnumerable<Song> songs = _sLogic.GetByGenre(genre);
        PrintDetails(songs);
    }
    void UnavailableSongs()
    {
        IEnumerable<Song> songs = _sLogic.GetUnavailableSongs();
        PrintDetails(songs);
    }
    void FromComposer()
    {
        string name = Validate("Enter the name of a Composer: ", x => ValidString(x, _cLogic.IsInDatabase))!;
        IEnumerable<Song> songs = _sLogic.GetSongsFromComposer(name)!;
        PrintDetails(songs);
    }
}