public class FilterSongMenu : Menu
{
    protected override string MenuStr => @"
    [1] Songs Between IDs
    [2] Songs Between Names
    [3] Songs Between Dates
    [4] Songs With Specific Genre
    [5] Songs From Composer
    [Q] Return to Search Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => BetweenIDs,
        '2' => BetweenNames,
        '3' => BetweenDates,
        '4' => WithGenre,
        '5' => FromComposer,
        _ => () => _active = false
    };
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