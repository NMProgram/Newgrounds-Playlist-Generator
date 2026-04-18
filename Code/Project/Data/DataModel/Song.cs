using System.Globalization;
using NAudio.Wave;
public class Song : IEquatable<Song>, IComparable<Song>, ICloneable, INamed
{
    private string _name;
    static CancellationTokenSource _cts = new();
    public long ID { get; private set; }
    public string Name { get => _name; set => _name = string.IsNullOrEmpty(value) ? _name : value; }
    public DateTime ReleaseDate { get; private set; }
    public Genre Genre { get; private set; }
    public long LevelID { get; private set; }
    public long Available { get; private set; }
    public byte[] Audio { get; private set; }
    public List<Composer> Composers { get; } = [];
    public Song(long id, string name, string releaseDate, long genre, 
    long levelID, long available, byte[] audio)
    {
        ID = id;
        _name = name;
        ReleaseDate = DateTime.ParseExact(releaseDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        Genre = (Genre)genre;
        LevelID = levelID;
        Available = available;
        Audio = audio;
    }
    public Song(Song song) : this(song.ID, song.Name, song.ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss"), 
    (int)song.Genre, song.LevelID, song.Available, song.Audio)
    {
        List<Composer> comps = [];
        foreach (var comp in song.Composers)
        { comps.Add(comp); }
        Composers = comps;
    }
    public object Clone()
    {
        Song clone = new(ID, Name, ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss"), (int)Genre, LevelID, Available, Audio);
        foreach (var comp in Composers) { clone.AddComposer(new Composer(comp)); }
        return clone;
    }    
    public void AddComposer(Composer? composer)
    {
        if (composer is not null && !Composers.Contains(composer))
        { Composers.Add(composer); }
    }
    public void AddComposer(IEnumerable<Composer> composers)
    {
        foreach (var comp in composers.Distinct()) { AddComposer(comp); }
    }
    public async Task PlayAsync() 
    {
        var token = ResetCTS();
        using Mp3FileReader reader = new(new MemoryStream(Audio));
        using WaveOutEvent output = new();
        var tcs = RunSong(reader, output);
        var completed = await Task.WhenAny(tcs.Task, Task.Run(token.WaitHandle.WaitOne));
        if (completed != tcs.Task) { output.Stop(); }
    }
    static CancellationToken ResetCTS()
    {
        _cts.Cancel();
        _cts = new();
        return _cts.Token;
    }
    static TaskCompletionSource<bool> RunSong(Mp3FileReader reader, WaveOutEvent output)
    {
        TaskCompletionSource<bool> tcs = new(false);
        output.PlaybackStopped += (s, args) => tcs.TrySetResult(true);
        output.Init(reader);
        output.Play();
        return tcs;
    }
    public int CompareTo(Song? other)
    {
        if (other is null) { return 1; }
        int id = ID.CompareTo(other.ID);
        if (id != 0) { return id; }
        int name = Name.CompareTo(other.Name);
        if (name != 0) { return name; }
        return ReleaseDate.CompareTo(other.ReleaseDate);
    }
    public bool Equals(Song? other)
    {
        if (other is null) { return false; }
        return Audio.SequenceEqual(other.Audio);
    }
    public override bool Equals(object? obj) => Equals(obj as Song);
    public override int GetHashCode()
    {
        return HashCode.Combine(Audio);
    }
    public void SetID(long id) => ID = Math.Max(id, 1);
    public void SetName(string name) => Name = !string.IsNullOrEmpty(name) ? name : Name;
    public void SetReleaseDate(string releaseDate)
    {
        bool succ = DateTime.TryParseExact(releaseDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime res);
        if (succ) { ReleaseDate = res; }
    }
    public void SetGenre(long genre)
    {
        var vals = Enum.GetValues<Genre>().Select(x => (int)x);
        Genre = (Genre)Math.Clamp(genre, vals.Min(), vals.Max());
    }
    public void SetLevelID(long id) => LevelID = Math.Max(id, 1);
    public void SetAvailable(long val) => Available = Math.Clamp(val, 0, 1);
    public void SetAudio(byte[] audio) => Audio = audio;
    string GetID() => $"Song ID: {ID}";
    string GetName() => $"Song Name: {Name}";
    string GetReleaseDate() => $"Release Date: " + ReleaseDate.ToString("MMM dd, yyyy");
    string GetGenre() => $"Genre: {ConversionLogic.GetGenreName(Genre.ToString())}";
    string GetLevelID() => $"First GD Level ID: {LevelID}";
    string GetAvailable() => $"Available on Newgrounds: {Available == 1}";
    string GetComposers() => $"Composers: {string.Join(", ", Composers.Select(x => $"\'{x.Name}\'"))}";
    public override string ToString()
    {
        _ = PlayAsync();
        return string.Join('\n', GetID(), GetName(), GetReleaseDate(), 
        GetGenre(), GetLevelID(), GetAvailable(), GetComposers()).Bold();
    }
}