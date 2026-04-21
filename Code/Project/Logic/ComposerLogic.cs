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
        => _cAccess.GetByName(name) is not null ? (true, name, null) : 
        (false, "", $"{name} was not found in the database.");
    public (bool, string, string?) IsNotInDatabase(string name)
        => !IsInDatabase(name).InDatabase ? (true, name, null) : 
        (false, "", $"{name} already exists in the database.");
    public (bool, long, string?) IsNewSong(string name, long id)
        => IsNewSong(_cAccess.GetByName(name)!, id);
    public (bool, long, string?) IsNewSong(Composer comp, long id)
    {
        IEnumerable<Song> songs = comp.Songs;
        Song? found = songs.FirstOrDefault(x => x.ID == id);
        return found is null ? (true, id, null) : 
        (false, -1, $"\'{comp.Name}\' already has the Song \'{found.Name}\' (ID \'{id}\').");
    }
    public (bool, long, string?) IsNotNewSong(string name, long id)
        => !IsNewSong(name, id).Item1 ? (true, id, null) : 
        (false, -1, $"\'{name}\' doesn't have a Song with ID \'{id}\'.");
}