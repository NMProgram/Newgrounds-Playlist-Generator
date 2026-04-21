using System.ComponentModel.Design.Serialization;
using System.Globalization;
using NAudio.MediaFoundation;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
public class Song : IEquatable<Song>, IComparable<Song>, ICloneable, INamed
{
    private string _name;
    static CancellationTokenSource _loadCTS = new();
    static CancellationTokenSource _runCTS = new();
    static WaveOutEvent? _output = null;
    static WaveChannel32? _provider = null;
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
        {
            Composers.Add(composer);
            composer.AddSong(this);
        }
    }
    public void AddComposer(IEnumerable<Composer> composers)
    {
        foreach (var comp in composers.Distinct()) { AddComposer(comp); }
    }
    public async Task PlayAsync()
    {
        var loadToken = ResetCTS(ref _loadCTS);
        WaveFileReader? wavReader = null;
        try
        {
            wavReader = await new AudioPlayer(Audio).CreateWavAsync(-12);
            if (loadToken.IsCancellationRequested) { return; }
            TaskCompletionSource<bool> tcs = new(false);
            var runToken = ResetCTS(ref _runCTS);
            await PlaybackAsync(wavReader, runToken);
        }
        catch (OperationCanceledException) 
        { wavReader?.Dispose(); }
    }
    async Task FadeOutAsync()
    {
        if (_output is null || _provider is null) { return; }
        const float STEPS = 50;
        const int DELAY = 10;
        for (int i = 0; i < STEPS; i++)
        {
            _provider.Volume = 1f - (i / STEPS);
            await Task.Delay(DELAY);
        }
        _output.Stop(); _output.Dispose();
        _output = null; _provider = null;
    }
    static CancellationToken ResetCTS(ref CancellationTokenSource cts)
    {
        cts.Cancel();
        cts = new CancellationTokenSource();
        return cts.Token;
    }
    private async Task PlaybackAsync(WaveFileReader wavReader, CancellationToken token)
    {
        await FadeOutAsync();
        WaveChannel32 provider = new(wavReader) { Volume = 1.0f };
        WaveOutEvent output = new();
        TaskCompletionSource<bool> tcs = new();
        output.PlaybackStopped += (s, e) => tcs.TrySetResult(true);
        output.Init(provider);
        output.Play();
        _output = output;
        _provider = provider;
        try
        {
            var cancelTask = Task.Delay(Timeout.Infinite, token);
            var completed = await Task.WhenAny(tcs.Task, cancelTask);
            await tcs.Task;
        }
        finally
        { wavReader.Dispose(); }
    }
    public int CompareTo(Song? other)
    {
        if (other is null) { return 1; }
        int name = Name.CompareTo(other.Name);
        if (name != 0) { return name; }
        return ID.CompareTo(other.ID);
    }
    public bool Equals(Song? other)
    {
        if (other is null) { return false; }
        return Audio.SequenceEqual(other.Audio) && ID == other.ID;
    }
    public override bool Equals(object? obj) => Equals(obj as Song);
    public override int GetHashCode() => HashCode.Combine(Audio, ID);
    public void SetID(long id) => ID = Math.Max(id, 1);
    public void SetName(string name) => Name = name;
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
    string GetComposers()
    {
        return Composers.Count switch
        {
            0 => "Composers: N/A",
            1 => $"Composer: \'{Composers[0].Name}\'",
            _ => $"Composers: {string.Join(", ", Composers.Select(x => $"\'{x.Name}\'"))}",
        };
    }
    public override string ToString()
    {
        _ = PlayAsync();
        return string.Join('\n', GetID(), GetName(), GetReleaseDate(),
        GetGenre(), GetLevelID(), GetAvailable(), GetComposers()).Bold();
    }
}