using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

public abstract class AccessLogic<TKey, TValue>
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
    public abstract TValue? GetByID(TKey key);
    public abstract void Update(TValue o, TValue n);
}