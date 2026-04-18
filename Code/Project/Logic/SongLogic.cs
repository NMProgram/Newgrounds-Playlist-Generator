public class SongLogic : AccessLogic
{
    public SongLogic(IConnection con) : base(con)
    {}
    public void Add(Song song)
    {
        _sAccess.Insert(song);
        // foreach (var comp in song.Composers)
        // {
        //     _cAccess.Insert(comp);
        //     _scAccess.Insert(new(song.ID, comp.ID));
        // }
    }
    public void Update(Song oldSong, Song newSong)
    {
        Delete(oldSong);
        Add(newSong);
    }
    public void Delete(Song song)
    {
        _sAccess.Delete(song);
        long[] compIDs = _scAccess.Delete(song.ID);
        foreach (var id in compIDs)
        { _cAccess.Delete(id); }
    }
    public Song? GetByID(long id) => _sAccess.GetByID(id);
    public Song? GetClosestMatch(long id) => _sAccess.GetByClosestID(id);
    public IEnumerable<Song> GetSongMatches(string search) 
        => _sAccess.GetMatchResults(search);
    public IEnumerable<Song> GetBetweenLevelIDs(long low, long high)
        => _sAccess.GetBetweenLevelIDs(low, high);
    public IEnumerable<Song> GetBetweenSongData(long low, long high)
        => _sAccess.GetBetweenData(low, high);
    public IEnumerable<Song> GetBetweenSongData(string first, string last)
        => _sAccess.GetBetweenData(first, last);
    public IEnumerable<Song> GetBetweenSongData(DateTime first, DateTime last)
        => _sAccess.GetBetweenData(first, last);
    public IEnumerable<Song> GetByGenre(Genre genre) => _sAccess.GetByGenre(genre);
    public IEnumerable<Song> GetSongsFromComposer(string name) => _sAccess.GetByComposer(name);
    public IEnumerable<Song> GetUnavailableSongs() => _sAccess.GetUnavailable();
    public (bool InDatabase, long, string?) IsInDatabase(long id)
        => _sAccess.GetByID(id) is not null ? (true, id, null) : 
        (false, -1, $"{id} was not found in the database.");
    public (bool, long, string?) IsNotInDatabase(long id)
        => !IsInDatabase(id).InDatabase ? (true, id, null) : 
        (false, -1, $"{id} already exists in the database.");
    public (bool, long, string?) IsUniqueLevelID(long id)
        => _sAccess.GetByLevelID(id) is null ? (true, id, null) : 
        (false, -1, $"Level ID {id} has already been used.");
}