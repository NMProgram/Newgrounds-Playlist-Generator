using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

public class AccessLogic
{
    readonly protected SongAccess _sAccess;
    readonly protected ComposerAccess _cAccess;
    readonly protected SongComposerAccess _scAccess;
    public AccessLogic(IConnection con)
    {
        _sAccess = new(con);
        _cAccess = new(con);
        _scAccess = new(con);
    }
    [ExcludeFromCodeCoverage]
    public void Update<T>(T value1, T value2)
    {
        switch((value1, value2))
        {
            case (Song s1, Song s2): Update(s1, s2); break;
            case (Composer c1, Composer c2): Update(c1, c2); break;
        }
    }
    [ExcludeFromCodeCoverage]
    public object? GetByID<TKey>(TKey value) => value switch
    {
        long s => GetByID(s),
        string c => GetByID(c),
        _ => throw new ArgumentException($"Invalid ID type \'{typeof(TKey)}\'.", nameof(value))
    };
}