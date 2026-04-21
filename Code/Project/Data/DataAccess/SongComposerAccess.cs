using System.Diagnostics.CodeAnalysis;
using Dapper;
public class SongComposerAccess : Accessor
{
    public SongComposerAccess(IConnection con) : base("SongComposer", con)
    {
        ExecuteSQL(@"CREATE TABLE IF NOT EXISTS SongComposer (
        songID INTEGER NOT NULL REFERENCES Song(id),
        composerID INTEGER NOT NULL REFERENCES Composer(id),
        PRIMARY KEY(songID, composerID)
        );");
    }
    public void Insert(SongComposer sc)
    {
        string sql = $"INSERT INTO {Table} VALUES (@SongID, @ComposerID)";
        ExecuteSQL(sql, sc);
    }
    public void Update(long songID, long oldID)
    {
        string sql = @$"UPDATE {Table} 
        SET songID = @SongID
        WHERE songID = @OldID";
        ExecuteSQL(sql, new { SongID = songID, OldID = oldID });
    }
    public long[] Delete(long songID)
    {
        long[] compIDs = [.. GetCompIDs(songID) ?? []];
        string sql = $"DELETE FROM {Table} WHERE songID = @SongID";
        ExecuteSQL(sql, new { SongID = songID });
        return compIDs;
    }
    public void Delete(long songID, long compID)
    {
        string sql = $"DELETE FROM {Table} WHERE songID = @SongID AND composerID = @CompID";
        ExecuteSQL(sql, new { SongID = songID, CompID = compID });
    }
    public void Delete(SongComposer sc)
    {
        ExecuteSQL($"DELETE FROM {Table} WHERE composerID = @ComposerID", sc);
    }
    public IEnumerable<long> GetCompIDs(long songID)
    {
        return QueryAll<long>($"SELECT composerID FROM {Table} WHERE songID = @SongID", new { SongID = songID });
    }
}