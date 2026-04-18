using Microsoft.Data.Sqlite;
using Dapper;

public class Accessor
{
    protected string Table { get; }
    readonly IConnection _con; 
    public Accessor(string table, IConnection con)
    {
        Table = table;
        _con = con;
    }
    public Accessor(IConnection con) : this("", con)
    {}
    SqliteConnection Connect() => new(_con.GetConnection());
    protected void ExecuteSQL(string sql, object? DP = null)
    {
        using var con = Connect();
        con.Execute(sql, DP);
    }
    protected T? QueryScalar<T>(string sql, object? DP = null)
    {
        using var con = Connect();
        return con.ExecuteScalar<T>(sql, DP);
    }
    protected T? QuerySingle<T>(string sql, object? DP = null)
    {
        using var con = Connect();
        return con.QueryFirstOrDefault(sql, DP);
    }
    protected IEnumerable<T1> QueryAll<T1, T2>(string sql, Func<T1, T2, T1> mapper, object? DP = null)
    {
        using var con = Connect();
        return con.Query(sql, mapper, DP);
    }
    protected IEnumerable<T> QueryAll<T>(string sql, object? DP = null)
    {
        using var con = Connect();
        return con.Query<T>(sql, DP);
    }
    
}