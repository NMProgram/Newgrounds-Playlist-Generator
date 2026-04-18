public class AlterMenu : Menu
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
    public (bool, long, string?) ValidID(string inp, Func<long, (bool, long, string?)> func)
    {
        (bool res, long val, string? err) = InputLogic.IsValidInteger(inp);
        return res ? func(val) : (res, default, err);
    }
    protected (bool, T?, string?) ValidString<T>(string inp, Func<string, (bool, T, string?)> func)
    {
        (bool res, _, string? err) = InputLogic.IsNotEmpty(inp);
        return res ? func(inp) : (res, default(T), err);
    }
    public (bool, long, string?) CheckID(string inp, Func<long, (bool, long, string?)> func)
    {
        return ValidString(inp, x => ValidID(x, func));
    }
    protected void UpdateData<TKey, TEntity, TValue>(
        string type, string prompt, 
        Func<string, (bool, TKey, string?)> validator, Action<TEntity> action, Func<TEntity, TValue> getter
    ) 
    where TEntity : ICloneable, INamed
    {
        TKey oldValue = Validate(prompt.Insert(9, " old"), validator);
        _name = oldValue!.ToString()!;
        TEntity o = (TEntity)_access.GetByID(oldValue)!;
        TEntity n = (TEntity)o.Clone();
        action(n);
        _access.Update(o, n);
        string msg = $"Successfully changed the {type} of the {typeof(TEntity)} {o.Name.Bold()} from \'{getter(o)}\' to \'{getter(n)}\'!";
        Console.WriteLine(msg);
    }

}