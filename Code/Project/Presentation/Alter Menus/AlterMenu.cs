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
    
    
    protected (TValue, TValue) SetUpdate<TKey, TValue>(TKey key, AccessLogic<TKey, TValue> getter, Action<TValue> action) where TValue : ICloneable
    {
        TValue o = getter.GetByID(key)!;
        TValue n = (TValue)o.Clone();
        action(n);
        getter.Update(o, n);
        return (o, n);
    }
}