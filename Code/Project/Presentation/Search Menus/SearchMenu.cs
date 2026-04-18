public class SearchMenu : Menu
{
    protected override string MenuStr => @"
    [1] Search Song
    [2] Search Composer
    [Q] Return to Main Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => new SearchSongMenu().Start,
        '2' => new SearchCompMenu().Start,
        _ => () => _active = false
    };
}