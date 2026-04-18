using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization.Metadata;

public class FilterSongMenu : FilterMenu
{
    protected override string MenuStr => @"
    [1] Search Song with Closest ID
    [2] Search Song By Match
    [3] Songs Between IDs
    [4] Songs Between Names
    [5] Songs Between Dates
    [6] Songs With Specific Genre
    [7] Songs From Composer
    [Q] Return to Filter Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => ClosestID,
        '2' => Match,
        '3' => BetweenIDs,
        '4' => BetweenNames,
        '5' => BetweenDates,
        '6' => WithGenre,
        '7' => FromComposer,
        _ => () => _active = false
    };
    
    void PrintDetails(IEnumerable<Song> songs)
    {
        PrintDetails([..songs], song => $"\'{song.Name}\' (ID: {song.ID})");
    }
    void ClosestID()
    {
        long id = Validate("Enter an ID: ", InputLogic.IsValidInteger);
        Song? song = _access.GetClosestMatch(id);
        PrintDetails(song);
    }
    void Match()
    {
        string search = Input("Enter a search term: ");
        IEnumerable<Song> songs = _access.GetMatches(search);
        PrintDetails(songs);
    }
    void BetweenIDs()
    {
        long lowID = Validate("Enter the first ID: ", x => ValidString(x, InputLogic.IsValidInteger));
        long highID = Validate("Enter the last ID: ", x => ValidString(x, InputLogic.IsValidInteger));
        IEnumerable<Song> songs = _access.GetBetweenSongData(lowID, highID);
        PrintDetails(songs);
    }
    void BetweenNames()
    {
        string first = Validate("Enter the first name: ", InputLogic.IsNotEmpty);
        string last = Validate("Enter the last name: ", InputLogic.IsNotEmpty);
        IEnumerable<Song> songs = _access.GetBetweenSongData(first, last);
        PrintDetails(songs);
    }
    void BetweenDates()
    {
        DateTime first = Validate("Enter the starting date: ", x => ValidString(x, InputLogic.IsValidDate));
        DateTime last = Validate("Enter the ending date: ", x => ValidString(x, InputLogic.IsValidDate));
        IEnumerable<Song> songs = _access.GetBetweenSongData(first, last);
        PrintDetails(songs);
    }
    void WithGenre()
    {
        Genre genre = Validate("Enter a Genre to filter by: ", x => ValidString(x, InputLogic.IsValidGenre));
        IEnumerable<Song> songs = _access.GetByGenre(genre);
        PrintDetails(songs);
    }
    void FromComposer()
    {
        string name = Validate("Enter the name of a Composer: ", x => ValidString(x, _access.IsInDatabase))!;
        Composer? comp = _access.GetByID(name);
        if (comp is null) { PrintDetails<Song>(null); return; }
        PrintDetails(comp.Songs);
    }
}