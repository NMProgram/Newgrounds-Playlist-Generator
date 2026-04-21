using Microsoft.Data.Sqlite;

public interface IConnection
{
    public SqliteConnection Connect(string? fileName = null);
}

public class InFile : IConnection
{
    string dir = "Data/DataSource";
    public SqliteConnection Connect(string? fileName = null)
    {
        fileName ??= "database.db";
        return new("Data Source=" + Path.Combine(dir, fileName));
    }
}

public class InMemory : IConnection
{
    public SqliteConnection Connect(string? fileName = null)
    {
        return new($"Data Source=file:memorydb?mode=memory&cache=shared;");
    }
}