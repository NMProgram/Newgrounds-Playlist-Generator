public class SongLogic : AccessLogic<long, Song>
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
    public override void Update(Song oldSong, Song newSong)
    {
        _sAccess.Update(newSong, oldSong.ID);
        _scAccess.Update(newSong.ID, oldSong.ID);
        // Delete(oldSong);
        // Add(newSong);
    }
    public void Delete(Song song)
    {
        _sAccess.Delete(song);
        long[] compIDs = _scAccess.Delete(song.ID);
        foreach (var id in compIDs)
        { _cAccess.Delete(id); }
    }
    public override Song? GetByID(long id) => _sAccess.GetByID(id);
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
    public IEnumerable<Song> GetSongsFromComposer(string name) 
        => _sAccess.GetByComposer(name);
    public IEnumerable<Song> GetUnavailableSongs() => _sAccess.GetUnavailable();
    public (bool InDatabase, long, string?) IsInDatabase(string id)
    {
        var (res, val, err) = InputLogic.IsValidInteger(id);
        if (!res) { return (false, -1, err); }
        return _sAccess.GetByID(val) is not null ? (true, val, null) : 
        (false, -1, $"{val} was not found in the database.");
    }
    public (bool, long, string?) IsNotInDatabase(string id)
    {
        var (res, val, err) = InputLogic.IsValidInteger(id);
        if (!res) { return (false, -1, err); }
        return _sAccess.GetByID(val) is null ? 
        (true, val, null) : (false, -1, $"{id} already exists in the database.");
    }
    public (bool, long, string?) IsUniqueLevelID(string id)
    {
        var (res, val, err) = InputLogic.IsValidInteger(id);
        if (!res) { return (false, -1, err); }
        return _sAccess.GetByLevelID(val) is null ? (true, val, null) : 
        (false, -1, $"Level ID {val} has already been used.");
    }
}