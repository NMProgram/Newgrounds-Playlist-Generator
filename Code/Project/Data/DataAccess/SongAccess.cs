using Dapper;

public class SongAccess : Accessor
{
    const string SongSQL = @"
    SELECT s.* FROM Song AS s
    LEFT JOIN SongComposer AS sc ON sc.songID = s.id
    LEFT JOIN Composer AS c ON sc.composerID = c.id "; 
    public SongAccess(IConnection con) : base("Song", con)
    {
        ExecuteSQL(@"CREATE TABLE IF NOT EXISTS Song (
        id INTEGER NOT NULL PRIMARY KEY,
        name TEXT NOT NULL,
        releaseDate TEXT NOT NULL,
        genre INTEGER NOT NULL,
        levelID INTEGER NOT NULL,
        available INTEGER NOT NULL,
        audio BLOB NOT NULL
        );");
    }
    public IEnumerable<Song>? GetSongs(string filter, object? DP = null)
    {
        string sql = @"
        SELECT c.* FROM Song AS s 
        LEFT JOIN SongComposer AS sc ON sc.songID = s.id
        LEFT JOIN Composer AS c ON sc.composerID = c.id
        WHERE sc.songID = @ID
        ";
        IEnumerable<Song> songs = QueryAll<Song>(SongSQL + filter, DP);
        foreach (var song in songs)
        {
            IEnumerable<Composer> comps = QueryAll<Composer>(sql, song);
            song.AddComposer(comps);
        }
        return songs;
    }
    public Song? GetFirst(string filter, object? DP = null)
    {
        return GetSongs(filter, DP).FirstOrDefault();
    }
    public void Insert(Song song)
    {
        ExecuteSQL("INSERT INTO Song VALUES (@ID, @Name, @ReleaseDate, @Genre, @LevelID, @Available, @Audio)", song);
    }
    public void Update(Song song, long oldID)
    {
        string sql = @"UPDATE Song 
        SET id = @ID, name = @Name, releaseDate = @ReleaseDate, 
        genre = @Genre, levelID = @LevelID, available = @Available
        WHERE id = @OldID";
        DynamicParameters DP = new(song);
        DP.Add("OldID", oldID);
        ExecuteSQL(sql, DP);
    }
    public void Delete(Song song)
    {
        string sql = @"DELETE FROM Song WHERE id = @ID";
        ExecuteSQL(sql, song);
    }
    public Song? GetByID(long id)
    {
        return GetFirst("WHERE s.id = @ID", new { ID = id });
    }
    public Song? GetByLevelID(long levelID)
    {
        return GetFirst("WHERE s.levelID = @LevelID", new { LevelID = levelID });
    }
}