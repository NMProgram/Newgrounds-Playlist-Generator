using System.ComponentModel;
using System.Security.Cryptography;

namespace ObjectTests;

[TestClass]
public sealed class SongTests
{
    [TestMethod]
    [DataRow(311087, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1)]
    [DataRow(386900, "Silent Hill (Dubstep)", "2010-12-28 00:00:00", 8, 99719, 1)]
    [DataRow(586990, "Aztech + Lockyn: Soulwind", "2014-09-07 00:00:00", 8, 148508, 1)]
    public void ToString_Song_ReturnsStringMatch(long id, string name, string releaseDate, long genre, long levelID, long available)
    {
        // Arrange
        Song s1 = new(id, name, releaseDate, genre, levelID, available, []);
        Composer c1 = new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1);
        Composer c2 = new(5, "KDrew", "2010-07-21 00:00:00", 1989, "KDrew music and videos", 0);
        string dir = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
        string path = Path.Combine(dir, "8762_newgrounds_brent_.mp3");
        // Act
        s1.SetAudio(File.ReadAllBytes(path));
        string exp = $"Song ID: {id}\nSong Name: {name}\nRelease Date: {DateTime.Parse(releaseDate).ToString("MMM dd, yyyy")}\nGenre: {ConversionLogic.GetGenreName(((Genre)genre).ToString())}\nFirst GD Level ID: {levelID}\nAvailable on Newgrounds: {available == 1}\nComposers: N/A";
        bool res1 = exp.Bold() == s1.ToString();

        s1.AddComposer(c1);
        exp = exp.Replace("Composers: N/A", "Composer: \'Fantomenk\'");
        bool res2 = exp.Bold() == s1.ToString();

        s1.AddComposer(c2);
        exp = exp.Replace("Composer: \'Fantomenk\'", "Composers: \'Fantomenk\', \'KDrew\'");
        bool res3 = exp.Bold() == s1.ToString();

        // Assert
        Assert.IsTrue(res1);
        Assert.IsTrue(res2);
        Assert.IsTrue(res3);
    }
    [TestMethod]
    public void CompareTo_Song_ReturnsOrdered()
    {
        // Arrange
        Song[] songs = [
            new(386900, "Silent Hill (Dubstep)", "2010-12-28 00:00:00", 8, 99719, 1, []),
            new(586990, "Aztech + Lockyn: Soulwind", "2014-09-07 00:00:00", 8, 148508, 1, []),
            new(311087, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []),
            new(206261, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []),
        ];
        Song[] songsOrdered = [
            new(586990, "Aztech + Lockyn: Soulwind", "2014-09-07 00:00:00", 8, 148508, 1, []),
            new(386900, "Silent Hill (Dubstep)", "2010-12-28 00:00:00", 8, 99719, 1, []),
            new(206261, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []),
            new(311087, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []),
        ];
        // Act
        songs.Sort();
        // Assert
        for (int i = 0; i < songs.Length; i++)
        { Assert.AreEqual(songs[i], songsOrdered[i]); }
    }
    [TestMethod]
    public void Equals_Song_ReturnsBool()
    {
        // Arrange
        Song[] songs1 = [
            new(386900, "Silent Hill (Dubstep)", "2010-12-28 00:00:00", 8, 99719, 1, []),
            new(586990, "Aztech + Lockyn: Soulwind", "2014-09-07 00:00:00", 8, 148508, 1, []),
            new(311087, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []),
        ];
        Song[] songs2 = [
            new(386900, "Silent Hill (Dubstep)", "2010-12-28 00:00:00", 8, 99719, 1, []),
            new(586990, "Aztech + Lockyn: Soulwind", "2014-09-07 00:00:00", 8, 148508, 1, []),
            new(311087, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []),
        ];
        Composer c1 = new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1);
        // Act
        bool notEqual = songs1[0].Equals(c1);
        // Assert
        for (int i = 0; i < songs1.Length; i++)
        { Assert.AreEqual(songs1[i], songs2[i]); }
        Assert.AreNotEqual(songs1[0], songs2[2]);
        Assert.IsFalse(notEqual);
    }
    [TestMethod]
    public void Clone_Song_ReturnsNewRef()
    {
        // Arrange
        Song s = new(311087, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []);
        Composer c1 = new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1);
        Composer c2 = new(5, "KDrew", "2010-07-21 00:00:00", 1989, "KDrew music and videos", 0);
        s.AddComposer(c1);
        s.AddComposer(c2);
        // Act
        Song clone = (Song)s.Clone();
        // Assert
        Assert.AreNotSame(s, clone);
        for (int i = 0; i < 2; i++)
        { Assert.AreNotSame(s.Composers[i], clone.Composers[i]); }

    }
    [TestMethod]
    public void GetHashCode_Song_ReturnsEqualityHash()
    {
        // Arrange
        Song s1 = new(311087, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []);
        Song s2 = new(586990, "Aztech + Lockyn: Soulwind", "2014-09-07 00:00:00", 8, 148508, 1, []);
        Song s3 = new(586990, "Aztech + Lockyn: Soulwind", "2014-09-07 00:00:00", 8, 148508, 1, []);
        Composer c1 = new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1);
        Composer c2 = new(5, "KDrew", "2010-07-21 00:00:00", 1989, "KDrew music and videos", 0);
        s1.AddComposer(c1);
        s1.AddComposer(c2);
        s2.AddComposer(c1);
        s3.AddComposer(c1);
        // Act
        int hash1 = s2.GetHashCode();
        int hash2 = s3.GetHashCode();
        int hash3 = s1.GetHashCode();
        // Assert
        Assert.AreEqual(hash1, hash2);
        Assert.AreNotEqual(hash1, hash3);
    }
    [TestMethod]
    [DataRow("Noob", "2012-02-12 00:00:00", 10, 305051, 0)]
    public void SetMethods_Song_SetsNewData(string name, string releaseDate, long genre, long levelID, long available)
    {
        // Arrange
        Song s1 = new(311087, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []);
        // Act
        s1.SetName(name);
        s1.SetReleaseDate(releaseDate);
        s1.SetGenre(genre);
        s1.SetLevelID(levelID);
        s1.SetAvailable(available);
        // Assert
        Assert.AreEqual(name, s1.Name);
        Assert.AreEqual(releaseDate, s1.ReleaseDate.ToString("yyyy-MM-dd HH:mm:ss"));
        Assert.AreEqual((Genre)genre, s1.Genre);
        Assert.AreEqual(levelID, s1.LevelID);
        Assert.AreEqual(available, s1.Available);
    }
    [TestMethod]
    [DataRow(-1, "", "2015-15-15 99:99:99", 999, -15, 500)]
    public void SetMethods_Song_KeepsOldData(long id, string name, string releaseDate, long genre, long levelID, long available)
    {
        // Arrange
        Song s1 = new(311087, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []);
        Song clone = (Song)s1.Clone();
        var vals = Enum.GetValues<Genre>().Select(x => (int)x);
        int minGenre = vals.Min();
        int maxGenre = vals.Max();
        // Act
        s1.SetID(id);
        s1.SetName(name);
        s1.SetReleaseDate(releaseDate);
        s1.SetGenre(genre);
        s1.SetLevelID(levelID);
        s1.SetAvailable(available);
        // Assert
        Assert.IsGreaterThan(0, s1.ID);
        Assert.AreEqual(clone.Name, s1.Name);
        Assert.AreEqual(clone.ReleaseDate, s1.ReleaseDate);
        Assert.IsInRange(minGenre, maxGenre, (int)s1.Genre);
        Assert.IsGreaterThan(0, s1.LevelID);
        Assert.IsInRange(0, 1, s1.Available);
    }
}