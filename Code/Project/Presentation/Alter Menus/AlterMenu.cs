using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;

[ExcludeFromCodeCoverage]
public class AlterMenu : MainMenu
{
    protected string _name = null!;
    protected override string MenuStr => @"
    [1] Alter Song
    [2] Alter Composer
    [Q] Return to Main Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => new AlterSongMenu().Start,
        '2' => new AlterCompMenu().Start,
        _ => () => _active = false
    };
    
    protected void Updater<TKey, TValue>(TaskRunner runner, Action<TValue, TKey> setter, 
    Func<TValue, TKey, string> message, Action<TValue, TValue> updater) where TValue : ICloneable
    {
        runner.RunTasks().Deconstruct(out TValue obj, out TKey val);
        var copy = (TValue)obj.Clone();
        setter(copy, val);
        updater(obj, copy);
        Console.WriteLine(message(obj, val));
        Console.WriteLine($"\nNew {typeof(TValue)}:\n{copy}");
        AskEnter();
    }
    
    protected object Delete<T>(T obj) where T : INamed 
        => GetOption($"Are you sure you want to delete the {typeof(T)} {obj?.Name.Bold()}?\nEnter your choice here: ");
    protected string UpdateMsg<T>(string type, T obj) where T : INamed 
        => $"Successfully changed the {type} of the {typeof(T)} {obj.Name.Bold()}";
    protected string UpdateMsg<T>(T old, T val) => $"from \'{old?.ToString()?.Bold()}\' to \'{val?.ToString()?.Bold()}\'!";
    protected string UpdateMsg<TKey, TValue>(string type, TValue obj, TKey old, TKey val) where TValue : INamed
        => UpdateMsg(type, obj) + ' ' + UpdateMsg(old, val);
}