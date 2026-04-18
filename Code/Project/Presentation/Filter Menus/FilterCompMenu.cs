public class FilterCompMenu : Menu
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
        '1' => BetweenNames,
        '2' => BetweenDates,
        '3' => BetweenAges,
        '4' => OnNewgrounds,
        _ => () => _active = false
    };
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