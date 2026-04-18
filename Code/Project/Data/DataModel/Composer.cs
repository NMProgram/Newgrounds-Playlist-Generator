using System.Globalization;
public class Composer : IEquatable<Composer>, IComparable<Composer>, ICloneable, INamed
{
    private string _name;
    public long ID { get; private set; }
    public string Name { get => _name; set => _name = string.IsNullOrEmpty(value) ? _name : value; }
    public DateTime JoinDate { get; private set; }
    public long BirthYear { get; private set; }
    public string Description { get; private set; }
    public long OnNewgrounds { get; private set; }
    public List<Song> Songs { get; } = [];
    public Composer(long id, string name, string joinDate, long birthYear, string description, long onNewgrounds)
    {
        ID = id;
        _name = name;
        JoinDate = DateTime.ParseExact(joinDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        BirthYear = birthYear;
        Description = description;
        OnNewgrounds = onNewgrounds;
    }
    public Composer(Composer comp) : this(comp.ID, comp.Name, comp.JoinDate.ToString("yyyy-MM-dd HH:mm:ss"), 
    comp.BirthYear, comp.Description, comp.OnNewgrounds)
    {
        List<Song> songs = [];
        foreach (var song in comp.Songs)
        { songs.Add(song); }
        Songs = songs;
    }
    public object Clone()
    {
        Composer clone = new(ID, Name, JoinDate.ToString("yyyy-MM-dd HH:mm:ss"), BirthYear, Description, OnNewgrounds);
        foreach (var song in Songs) { clone.AddSong(new Song(song)); }
        return clone;
    }  
    public async Task Play()
    {
        if (Songs.Count > 0)
        { await Songs[Random.Shared.Next(Songs.Count - 1)].PlayAsync(); }
    }
    public int CompareTo(Composer? other)
    {
        if (other is null) { return 1; }
        int id = ID.CompareTo(other.ID);
        if (id != 0) { return id; }
        int name = Name.CompareTo(other.Name);
        if (name != 0) { return name; }
        return Description.CompareTo(other.Description);
    }
    public bool Equals(Composer? other)
    {
        if (other is null) { return false; }
        return ID == other.ID && Name == other.Name && JoinDate == other.JoinDate && 
        BirthYear == other.BirthYear && Description == other.Description;
    }
    public override bool Equals(object? obj)
        => Equals(obj as Composer);
    public override int GetHashCode()
        => HashCode.Combine(ID, Name, JoinDate, BirthYear, Description);
    public void AddSong(Song? song)
    {
        if (song is not null && !Songs.Contains(song))
        { Songs.Add(song); }
        Songs.Sort();
    }
    public void AddSong(IEnumerable<Song> songs)
    {
        foreach (var song in songs) { AddSong(song); }
    }
    public int UpdateSong(long oldID, Song newSong)
    {
        for (int i = 0; i < Songs.Count; i++)
        {
            if (oldID == Songs[i].ID) { Songs[i] = newSong; return i; }
        }
        return -1;
    }
    public long GetAge() => DateTime.Today.Year - BirthYear;
    public void SetID(long id) => ID = Math.Max(id, 0);
    public void SetName(string name) => Name = name;
    public void SetJoinDate(DateTime date) => JoinDate = date;
    public void SetAge(long age) => BirthYear = DateTime.Today.Year - age;
    public void SetDescription(string desc) => Description = desc;
    public void SetAvailability(int value) => OnNewgrounds = Math.Clamp(value, 0, 1);
    string GetName() => $"Name: {Name}";
    string GetJoinDate() => $"Join Date: " + JoinDate.ToString("MMM dd, yyyy");
    string GetAgeStr() => $"Age: {GetAge()}";
    string GetDescription() => $"Description: {Description}";
    string GetAvailable() => $"On Newgrounds: {OnNewgrounds == 1}";
    string GetSongs() => $"Songs: {string.Join(", ", Songs.Select(x => $"\'{x.Name}\'"))}";
    public override string ToString()
    {
        _ = Play();
        return string.Join('\n', GetName(), GetJoinDate(), 
        GetAgeStr(), GetDescription(), GetAvailable(), GetSongs()).Bold();
    }
}