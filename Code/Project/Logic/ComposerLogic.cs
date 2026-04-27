public class ComposerLogic : AccessLogic<string, Composer>
{
    public ComposerLogic(IConnection con) : base(con)
    {}
    public void Add(Composer comp)
    {
        _cAccess.Insert(comp);
        foreach (var song in comp.Songs)
        { AddSong(comp, song); }
    }
    public override void Update(Composer oldComp, Composer newComp)
    {
        _cAccess.Update(newComp, oldComp.Name);
    }
    public void AddSong(Composer comp, Song song)
    {
        _scAccess.Insert(new(song.ID, comp.ID));
    }
    public void UpdateSong(Composer comp, Song oldSong, Song newSong)
    {
        RemoveSong(comp, oldSong);
        AddSong(comp, newSong);
    }
    public void RemoveSong(Composer comp, Song song)
    {
        _scAccess.Delete(song.ID, comp.ID);
    }
    public void Delete(Composer comp)
    {
        Delete(comp.ID);
    }
    public void Delete(long id)
    {
        _scAccess.Delete(new SongComposer(-1, id));
        _cAccess.Delete(id);
    }
    public override Composer? GetByID(string name) => _cAccess.GetByName(name);
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
    public (bool InDatabase, string, string?) IsInDatabase(string name)
    {
        var (res, _, err) = InputLogic.IsNotEmpty(name);
        if (!res) { return (false, "", err); }
        return _cAccess.GetByName(name) is not null ? (true, name, null) : 
        (false, "", $"{name} was not found in the database.");
    }
    public (bool, string, string?) IsNotInDatabase(string name)
    {
        var (res, val, err) = InputLogic.IsNotEmpty(name);
        if (!res) { return (res, val, err); }
        return _cAccess.GetByName(name) is null ? (true, name, null) : (false, "", $"{name} already exists in the database.");
    }
    public (bool, long, string?) IsNewSong(string name, string id)
    {
        var (res, _, err) = IsInDatabase(name);
        if (!res) { return (false, -1, err); }
        var (res2, val2, err2) = InputLogic.IsValidInteger(id);
        if (!res2) { return (res2, -1, err2); }
        return TestNewSong(_cAccess.GetByName(name)!, val2);
    }
    (bool, long, string?) TestNewSong(Composer comp, long id)
    {
        IEnumerable<Song> songs = comp.Songs;
        Song? found = songs.FirstOrDefault(x => x.ID == id);
        return found is null ? (true, id, null) : 
        (false, -1, $"\'{comp.Name}\' already has the Song \'{found.Name}\' (ID \'{id}\').");
    }
    public (bool, long, string?) IsNotNewSong(string name, string id)
    {
        var (res, val, err) = IsInDatabase(name);
        if (!res) { return (res, -1, err); }
        var (res2, val2, err2) = InputLogic.IsValidInteger(id);
        if (!res2) { return (res2, -1, err2); }
        return _cAccess.GetByName(name)!.Songs.Any(x => x.ID == val2) ? (true, val2, null) : 
        (false, -1, $"\'{name}\' doesn't have a Song with ID \'{id}\'.");
    }
}