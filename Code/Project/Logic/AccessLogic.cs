using System.Security.Cryptography.X509Certificates;

public class AccessLogic
{
    readonly SongAccess _sAccess;
    readonly ComposerAccess _cAccess;
    readonly SongComposerAccess _scAccess;
    public AccessLogic(IConnection con)
    {
        _sAccess = new(con);
        _cAccess = new(con);
        _scAccess = new(con);
    }
    public void AddSong(Song song)
    {
        _sAccess.Insert(song);
        foreach (var comp in song.Composers)
        {
            _cAccess.Insert(comp);
            _scAccess.Insert(new(song.ID, comp.ID));
        }
    }
    public void AddComposer(Composer comp)
    {
        _cAccess.Insert(comp);
        foreach (var song in comp.Songs)
        { _scAccess.Insert(new(song.ID, comp.ID)); }
    }
    public void Update(Song oldSong, Song newSong)
    {
        DeleteSong(oldSong);
        AddSong(newSong);
    }
    public void Update(Composer oldComp, Composer newComp)
    {
        DeleteComposer(oldComp);
        AddComposer(newComp);
    }
    public void Update<T>(T value1, T value2)
    {
        switch((value1, value2))
        {
            case (Song s1, Song s2): Update(s1, s2); break;
            case (Composer c1, Composer c2): Update(c1, c2); break;
        }
    }
    public void DeleteSong(Song song)
    {
        _sAccess.Delete(song);
        long[] compIDs = _scAccess.Delete(song.ID);
        foreach (var id in compIDs)
        { _cAccess.Delete(id); }
    }
    public void DeleteComposer(Composer comp)
    {
        _cAccess.Delete(comp);
        _scAccess.Delete(new SongComposer(-1, comp.ID));
    }
    public Song? GetByID(long id) => _sAccess.GetByID(id);
    public Composer? GetByID(string name) => _cAccess.GetByName(name);
    public object? GetByID<TKey>(TKey value) => value switch
    {
        long s => GetByID(s),
        string c => GetByID(c),
        _ => throw new ArgumentException($"Invalid ID type \'{typeof(TKey)}\'.", nameof(value))
    };
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
    public IEnumerable<Song>? GetSongsFromComposer(string name) => GetByID(name)?.Songs;
    public IEnumerable<Song> GetUnavailableSongs() => _sAccess.GetUnavailable();
    public IEnumerable<Composer> GetComposerMatches(string search) 
        => _cAccess.GetMatchResults($"%{search}%");
    public IEnumerable<Composer> GetBetweenCompData(string first, string last)
        => _cAccess.GetBetweenData(first, last);
    public IEnumerable<Composer> GetBetweenCompData(DateTime first, DateTime last)
        => _cAccess.GetBetweenData(first, last);
    public IEnumerable<Composer> GetBetweenCompData(long first, long last)
        => _cAccess.GetBetweenData(first, last);
    public IEnumerable<Composer> GetUnavailableComposers() 
        => _cAccess.GetUnavailable();
    public IEnumerable<Composer> GetBySongID(long id) 
        => _cAccess.GetComposersWithSong(id);
    public IEnumerable<Composer> GetBySongName(string name) 
        => _cAccess.GetComposersWithSong($"%{name}%");
    public (bool InDatabase, long, string?) IsInDatabase(long id)
    {
        return _sAccess.GetByID(id) is not null ? (true, id, null) : (false, -1, $"{id} was not found in the database.");
    }
    public (bool, long, string?) IsNotInDatabase(long id)
    {
        return !IsInDatabase(id).InDatabase ? (true, id, null) : (false, -1, $"{id} already exists in the database.");
    }
    public (bool InDatabase, string, string?) IsInDatabase(string name)
    {
        return _cAccess.GetByName(name) is not null ? (true, name, null) : (false, "", $"{name} was not found in the database.");
    }
    public (bool, string, string?) IsNotInDatabase(string name)
    {
        return !IsInDatabase(name).InDatabase ? (true, name, null) : (false, "", $"{name} already exists in the database.");
    }
    public (bool, long, string?) IsUniqueLevelID(long id)
    {
        return _sAccess.GetByLevelID(id) is null ? (true, id, null) : (false, -1, $"Level ID {id} has already been used.");
    }
    public (bool, long, string?) IsNewSong(string name, long id)
    {
        return IsNewSong(_cAccess.GetByName(name)!, id);
    }
    public (bool, long, string?) IsNewSong(Composer comp, long id)
    {
        IEnumerable<Song> songs = comp.Songs;
        Song? found = songs.FirstOrDefault(x => x.ID == id);
        return found is null ? (true, id, null) : (false, -1, $"\'{comp.Name}\' already has the Song \'{found.Name}\' (ID \'{id}\').");
    }
    public (bool, long, string?) IsNotNewSong(string name, long id)
    {
        return !IsNewSong(name, id).Item1 ? 
        (true, id, null) : (false, -1, $"\'{name}\' doesn't have a Song with ID \'{id}\'.");
    }
}