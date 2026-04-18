public class FilterMenu : Menu
{
    protected override string MenuStr => @"
    [1] Filter Songs
    [2] Filter Composers
    [Q] Return to Main Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => new FilterSongMenu().Start,
        '2' => new FilterCompMenu().Start,
        _ => () => _active = false
    };
}