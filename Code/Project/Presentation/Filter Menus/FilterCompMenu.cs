public class FilterCompMenu : FilterMenu
{
    protected override string MenuStr => @"
    [1] Composers Between Names
    [2] Composers Between Dates
    [3] Composers Between Ages
    [4] Composers On Newgrounds
    [Q] Return to Search Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => () => CheckActivity(BetweenNames),
        '2' => () => CheckActivity(BetweenDates),
        '3' => () => CheckActivity(BetweenAges),
        '4' => () => CheckActivity(OnNewgrounds),
        _ => () => _active = false
    };
    void PrintDetails(IEnumerable<Composer> comps)
    {
        PrintDetails([..comps], comp => $"\'{comp.Name}\' (Total Songs: {comp.Songs.Count})");
    }
    void BetweenNames()
    {
        
    }
    void BetweenDates()
    {
        
    }
    void BetweenAges()
    {
        
    }
    void OnNewgrounds()
    {
        
    }
}