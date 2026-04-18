public class FilterMenu : MainMenu
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
    protected void PrintDetails<T>(T? obj)
    {
        string msg = obj is null ? "No results found." : obj.ToString()!;
        Console.WriteLine(msg);
        AskEnter();
    }
    protected void PrintDetails<T>(T[] objs, Func<T, string> getter)
    {
        if (objs.Length == 0) { Console.WriteLine("No results found."); return; }
        Console.WriteLine($"Found {objs.Length} results:\n");
        for (int i = 0; i < objs.Length; i++)
        {
            T? obj = objs[i];
            Console.WriteLine($"#{i + 1}: {getter(obj)}");
        }
        long[] list = ConversionLogic.CreateNumArr<long>(1, objs.Length, 1);
        long option = Validate("Enter the number next to the entry you wish to check out: ", 
        x => ValidID(x, y => InputLogic.IsInOptions(y, list)));
        PrintDetails(objs[option - 1]);
    }
}