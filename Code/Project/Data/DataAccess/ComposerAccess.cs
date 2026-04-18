using Dapper;

public class ComposerAccess : Accessor
{
    const string CompSQL = @"
    SELECT c.* FROM Composer AS c
    JOIN SongComposer AS sc ON sc.composerID = c.id
    JOIN Song AS s ON sc.songID = s.id ";
    public ComposerAccess(IConnection con) : base("Composer", con)
    {
        ExecuteSQL(@"CREATE TABLE IF NOT EXISTS Composer (
        id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        joinDate TEXT NOT NULL,
        birthYear INTEGER NOT NULL,
        description TEXT NOT NULL,
        onNewgrounds INTEGER NOT NULL
        );");
    }
    public IEnumerable<Composer>? GetComposers(string filter, object? DP = null)
    {
        string sql = @"
        SELECT s.* FROM Composer AS c
        JOIN SongComposer AS sc ON sc.composerID = c.id
        JOIN Song AS s ON sc.songID = s.id
        WHERE c.name = @Name
        ";
        IEnumerable<Composer> composers = QueryAll<Composer>(CompSQL + filter, DP);
        foreach (var comp in composers)
        {
            IEnumerable<Song> songs = QueryAll<Song>(sql, comp);
            comp.AddSong(songs);
        }
        return composers;
    }
    public Composer? GetFirst(string filter, object? DP = null) => GetComposers(filter, DP)?.FirstOrDefault();
    public void Insert(Composer composer)
    {
        long id = QueryScalar<long>($"SELECT MAX(id) FROM {Table}");
        composer.SetID(id + 1);
        string sql = @$"INSERT INTO {Table}
        VALUES (@ID, @Name, @JoinDate, @BirthYear, @Description, @OnNewgrounds)";
        QueryScalar<long>(sql, composer);
    }
    public void Update(Composer composer, string oldName)
    {
        string sql = @$"UPDATE {Table} SET name = @Name, joinDate = @JoinDate, 
        birthYear = @BirthYear, description = @Description, onNewgrounds = @OnNewgrounds
        WHERE name = @OldName";
        DynamicParameters DP = new(composer);
        DP.Add("OldName", oldName);
        ExecuteSQL(sql, DP);
    }
    public void Delete(Composer composer)
    {
        string sql = $"DELETE FROM {Table} WHERE id = @ID";
        string reset = $"UPDATE sqlite_sequence SET seq = (SELECT MAX(id) FROM {Table}) WHERE name = \'{Table}\'";
        ExecuteSQL(sql, composer);
        ExecuteSQL(reset);
    }
    public void Delete(long id) => Delete(new Composer(id, "", "2000-10-10 10:00:00", 0, "", 1));
    public Composer? GetByName(string name) => GetFirst("WHERE c.name = @Name", new { Name = name });
}