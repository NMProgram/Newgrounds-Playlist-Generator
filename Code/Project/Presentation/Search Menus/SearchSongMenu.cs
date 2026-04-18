public class SearchSongMenu : Menu
{
    protected override string MenuStr => @"
    [1] Search Song with Closest ID
    [2] Search Song By Match
    [3] Generate Random Song
    [Q] Return to Search Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => ClosestID,
        '2' => Match,
        '3' => RandomSong,
        _ => () => _active = false
    };
    void ClosestID()
    {
        
    }
    void Match()
    {
        
    }
    void RandomSong()
    {
        
    }
}