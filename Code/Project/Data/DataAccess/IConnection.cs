public interface IConnection
{
    public string? GetConnection(string? fileName = null);
}

public class InFile : IConnection
{
    string dir = "Data/DataSource";
    public string GetConnection(string? fileName = null)
    {
        fileName ??= "database.db";
        return "Data Source=" + Path.Combine(dir, fileName);
    }
}

public class InMemory : IConnection
{
    public string GetConnection(string? fileName = null)
    {
        return $"Data Source=file:memorydb?mode=memory&cache=shared;";
    }
}