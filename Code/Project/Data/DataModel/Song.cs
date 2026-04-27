using System.Globalization;
using NAudio.Wave;
public class Song : IEquatable<Song>, IComparable<Song>, ICloneable, INamed
{
    private string _name;
    public long ID { get; private set; }
    public string Name { get => _name; set => _name = string.IsNullOrEmpty(value) ? _name : value; }
    public DateTime ReleaseDate { get; private set; }
    public Genre Genre { get; private set; }
    public long LevelID { get; private set; }
    public long Available { get; private set; }
    public byte[] Audio { get; private set; }
    private Audio AudioSetup;
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
        AudioSetup = new(Audio);
        AudioSetup.SetupPlayback();
    }
    public Song(Song song) : this(song.ID, song.Name, song.ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss"),
    (int)song.Genre, song.LevelID, song.Available, song.AudioSetup.AudioBytes)
    {
        List<Composer> comps = [];
        foreach (var comp in song.Composers)
        { comps.Add(comp); }
        Composers = comps;
    }
    public object Clone()
    {
        Song clone = new(ID, Name, ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss"), (int)Genre, LevelID, Available, AudioSetup.AudioBytes);
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
    public void RemoveComposer(Composer comp)
    {
        for (int i = 0; i < Composers.Count; i++)
        {
            if (comp.Name == Composers[i].Name) { Composers.RemoveAt(i); comp.RemoveSong(this); }
        }
    }
    public async Task PlayAsync() => await AudioSetup.PlayAsync();
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
        return AudioSetup.AudioBytes.SequenceEqual(Audio) && ID == other.ID;
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
    public void SetReleaseDate(DateTime releaseDate) => ReleaseDate = releaseDate;
    public void SetGenre(long genre)
    {
        var vals = Enum.GetValues<Genre>().Select(x => (int)x);
        Genre = (Genre)Math.Clamp(genre, vals.Min(), vals.Max());
    }
    public void SetLevelID(long id) => LevelID = Math.Max(id, 1);
    public void SetAvailable(long val) => Available = Math.Clamp(val, 0, 1);
    public void SetAudio(byte[] audio) => AudioSetup = new(audio);
    public string GetAudioTitle() => Audio.Title;
    string GetID() => $"Song ID: {ID}";
    string GetName() => $"Song Name: {Name}";
    string GetReleaseDate() => $"Release Date: " + ReleaseDate.ToString("MMM dd, yyyy");
    string GetGenre() => $"Genre: {Genre.GetGenreName()}";
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