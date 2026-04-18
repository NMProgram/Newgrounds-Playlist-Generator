public class SearchCompMenu : Menu
{
    protected override string MenuStr => @"
    [1] Search Composer By Matching Name
    [2] Generate Random Composer
    [Q] Return to Search Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => ClosestID,
        '2' => Match,
        'R' => RandomSong,
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