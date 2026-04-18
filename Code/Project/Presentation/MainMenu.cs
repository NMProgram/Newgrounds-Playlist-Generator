public class MainMenu : Menu
{
    protected override string MenuStr => @"
    [1] Alter Data
    [2] Search Song Data
    [3] Filter Song Data
    [Q] Quit
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => new AlterMenu().Start,
        '2' => new SearchMenu().Start,
        '3' => new FilterMenu().Start,
        'Q' => () => Environment.Exit(0),
        _ => () => { Console.WriteLine("\nInvalid Key!"); Console.ReadKey(); }
    };
}