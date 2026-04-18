using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization.Metadata;

public class FilterSongMenu : Menu
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
    void PrintSongDetails(Song? song)
    {
        string msg = song is null ? "No results found." : song.ToString();
        Console.WriteLine(msg);
    }
    void PrintSongDetails(Song[] songs)
    {
        Console.WriteLine($"Found {songs.Length} results:\n");
        for (int i = 0; i < songs.Length; i++)
        {
            Song? song = songs[i];
            Console.WriteLine($"#{i + 1}: \'{song.Name}\' (ID: {song.ID})");
        }
        int[] list = Enumerable.Sequence(1, songs.Length, 1).ToArray();
        int option = Validate("Enter the number next to the entry you wish to check out: ", 
        InputLogic.IsValidInteger, x => InputLogic.IsInOptions(int.Parse(x), list));
        PrintSongDetails(songs[option - 1]);
    }
    void ClosestID()
    {
        long id = Validate("Enter an ID: ", InputLogic.IsValidInteger);
        Song? song = _access.GetClosestMatch(id);
        PrintSongDetails(song);
    }
    void Match()
    {
        string search = Input("Enter a search term: ");
        IEnumerable<Song> songs = _access.GetMatches(search);
        PrintSongDetails([..songs]);
    }
    void BetweenIDs()
    {
        
    }
    void BetweenNames()
    {
        
    }
    void BetweenDates()
    {
        
    }
    void WithGenre()
    {
        
    }
    void FromComposer()
    {
        
    }
}