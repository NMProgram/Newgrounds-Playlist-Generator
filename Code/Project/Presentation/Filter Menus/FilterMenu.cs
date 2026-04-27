using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;

[ExcludeFromCodeCoverage]
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
    void SetupPrint<T>(ref T[] objs, TaskRunner runner)
    {
        object[] values = runner.RunTasks();
        if (objs.Length < 2 || values[^1] is not long) { PrintDetails(objs, (_) => ""); return; }
        PrintDetails(objs[(long)values[^1] - 1]);
    }
    object GetOption<T>(T[] objs, Func<T, string> getter)
        => GetBetweenValues("\nEnter the number next to the entry you wish to check out: ", 1, objs, getter);
    protected void FilterData<TEntity, TResult>(string msg, string type, Func<string, TResult> funcType,
    Func<TResult, IEnumerable<TEntity>> filter, Func<TEntity, string> getter, Func<object, string>? formatter = null)
    {
        TResult value = default!; TEntity[] objs = []; TaskRunner runner = new();
        object GetResults()
        {
            value = funcType(msg);
            objs = [.. filter(value).DistinctBy(x => getter(x))];
            if (objs.Length > 1)
            { runner.Add(new("Search Result", () => GetOption(objs, getter))); }
            return value!;
        }
        runner.Add(type, GetResults, formatter);
        SetupPrint(ref objs, runner);
    }
    protected void FilterData<TEntity, TResult>(string startMsg, string endMsg, string type, Func<string, TResult> funcType,
    Func<TResult, TResult, IEnumerable<TEntity>> filter, Func<TEntity, string> getter, Func<object, string>? formatter = null)
    {
        TResult first = default!; TResult last = default!; TEntity[] objs = []; TaskRunner runner = new();
        object GetLast()
        {
            last = funcType(endMsg);
            objs = [.. filter(first, last).DistinctBy(x => getter(x))];
            if (objs.Length > 1) { runner.Add(new("Search Result", () => GetOption(objs, getter))); }
            return last!;
        }
        runner.Add("First " + type, () => { first = funcType(startMsg); return first!; }, formatter);
        runner.Add("Last " + type, GetLast, formatter);
        SetupPrint(ref objs, runner);
    }
}