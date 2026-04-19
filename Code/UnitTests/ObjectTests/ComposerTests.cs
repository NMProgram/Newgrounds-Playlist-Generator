namespace ObjectTests;

[TestClass]
public sealed class ComposerTests
{
    [TestMethod]
    [DataRow(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1)]
    [DataRow(2, "Aydin-Jewelz123", "2007-04-20 00:00:00", 1992, "Back in the flow of things!", 1)]
    [DataRow(5, "KDrew", "2010-07-21 00:00:00", 1989, "KDrew music and videos", 0)]
    public void ToString_Composer_ReturnsStringMatch(long id, string name, string joinDate, long birthYear, string desc, long available)
    {
        // Arrange
        Composer c1 = new(id, name, joinDate, birthYear, desc, available);
        Song s1 = new(478433, "-Pixelspace- SIK2 OST", "2012-03-28 00:00:00", 15, 660967, 1, []);
        Song s2 = new(479319, "-Electroman adventures-", "2012-04-04 00:00:00", 15, 731493, 1, []);
        // Act
        string exp = $"Name: {name}\nJoin Date: {DateTime.Parse(joinDate).ToString("MMM dd, yyyy")}\nAge: {DateTime.Today.Year - birthYear}\nDescription: {desc}\nOn Newgrounds: {available == 1}\nSongs: N/A";
        bool res1 = exp.Bold() == c1.ToString();

        c1.AddSong(s1);
        exp = exp.Replace("Songs: N/A", "Song: \'-Pixelspace- SIK2 OST\'");
        bool res2 = exp.Bold() == c1.ToString();

        c1.AddSong(s2);
        exp = exp.Replace("Song: \'-Pixelspace- SIK2 OST\'", "Songs: \'-Electroman adventures-\', \'-Pixelspace- SIK2 OST\'");
        bool res3 = exp.Bold() == c1.ToString();

        // Assert
        Assert.IsTrue(res1);
        Assert.IsTrue(res2);
        Assert.IsTrue(res3);
    }
    [TestMethod]
    public void CompareTo_Composer_ReturnsOrdered()
    {
        // Arrange
        Song s1 = new(478433, "-Pixelspace- SIK2 OST", "2012-03-28 00:00:00", 15, 660967, 1, []);
        Song s2 = new(479319, "-Electroman adventures-", "2012-04-04 00:00:00", 15, 731493, 1, []);
        Composer[] comps = [
            new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1),
            new(2, "Aydin-Jewelz123", "2007-04-20 00:00:00", 1992, "Back in the flow of things!", 1),
            new(3, "Aydin-Jewelz123", "2007-04-20 00:00:00", 1992, "Back in the flow of things!", 1),
            new(5, "KDrew", "2010-07-21 00:00:00", 1989, "KDrew music and videos", 0)
        ];
        Composer[] ordered = [
            new(3, "Aydin-Jewelz123", "2007-04-20 00:00:00", 1992, "Back in the flow of things!", 1),
            new(2, "Aydin-Jewelz123", "2007-04-20 00:00:00", 1992, "Back in the flow of things!", 1),
            new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1),
            new(5, "KDrew", "2010-07-21 00:00:00", 1989, "KDrew music and videos", 0)
        ];
        for (int i = 0; i < comps.Length; i++)
        { comps[i].AddSong(s1); }
        comps[2].AddSong(s2);
        // Act
        comps.Sort();
        // Assert
        for (int i = 0; i < comps.Length; i++)
        { Assert.AreEqual(comps[i], ordered[i]); }
    }
    [TestMethod]
    public void Equals_Composer_ReturnsBool()
    {
        // Arrange
        Composer[] comps1 = [
            new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1),
            new(2, "Aydin-Jewelz123", "2007-04-20 00:00:00", 1992, "Back in the flow of things!", 1),
            new(5, "KDrew", "2010-07-21 00:00:00", 1989, "KDrew music and videos", 0)
        ];
        Composer[] comps2 = [
            new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1),
            new(2, "Aydin-Jewelz123", "2007-04-20 00:00:00", 1992, "Back in the flow of things!", 1),
            new(5, "KDrew", "2010-07-21 00:00:00", 1989, "KDrew music and videos", 0)
        ];
        Song s1 = new(478433, "-Pixelspace- SIK2 OST", "2012-03-28 00:00:00", 15, 660967, 1, []);
        // Act
        bool notEqual = comps1[0].Equals(s1);
        // Assert
        for (int i = 0; i < comps1.Length; i++)
        { Assert.AreEqual(comps1[i], comps2[i]); }
        Assert.AreNotEqual(comps1[0], comps2[2]);
        Assert.IsFalse(notEqual);
    }
    [TestMethod]
    public void Clone_Composer_ReturnsNewRef()
    {
        // Arrange
        Composer c = new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1);
        Song s1 = new(478433, "-Pixelspace- SIK2 OST", "2012-03-28 00:00:00", 15, 660967, 1, []);
        Song s2 = new(479319, "-Electroman adventures-", "2012-04-04 00:00:00", 15, 731493, 1, []);
        c.AddSong(s1);
        c.AddSong(s2);
        // Act
        Composer clone = (Composer)c.Clone();
        // Assert
        Assert.AreNotSame(c, clone);
        for (int i = 0; i < 2; i++)
        { Assert.AreNotSame(c.Songs[i], clone.Songs[i]); }

    }
    [TestMethod]
    public void GetHashCode_Composer_ReturnsEqualityHash()
    {
        // Arrange
        Composer c1 = new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1);
        Composer c2 = new(2, "Aydin-Jewelz123", "2007-04-20 00:00:00", 1992, "Back in the flow of things!", 1);
        Composer c3 = new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1);
        Song s1 = new(478433, "-Pixelspace- SIK2 OST", "2012-03-28 00:00:00", 15, 660967, 1, []);
        Song s2 = new(479319, "-Electroman adventures-", "2012-04-04 00:00:00", 15, 731493, 1, []);
        c1.AddSong(s1);
        c1.AddSong(s2);
        c2.AddSong(s1);
        c3.AddSong(s1);
        // Act
        int hash1 = c1.GetHashCode();
        int hash2 = c2.GetHashCode();
        int hash3 = c3.GetHashCode();
        // Assert
        Assert.AreEqual(hash1, hash3);
        Assert.AreNotEqual(hash1, hash2);
    }
    [TestMethod]
    public void UpdateSong_Composer_SetsNewSongInSongs()
    {
        // Arrange
        Composer c1 = new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1);
        Song s1 = new(478433, "-Pixelspace- SIK2 OST", "2012-03-28 00:00:00", 15, 660967, 1, []);
        Song s2 = new(479319, "-Electroman adventures-", "2012-04-04 00:00:00", 15, 731493, 1, []);
        Song s3 = new(643474, "EnV - Ginseng", "2015-09-10 00:00:00", 6, 212295, 1, []);
        Song s4 = new(598682, "circles kdrew", "2014-12-24 00:00:00", 8, 188711, 0, []);
        c1.AddSong(s1);
        c1.AddSong(s2);
        // Act
        c1.UpdateSong(s1.ID, s3);
        c1.UpdateSong(s2.ID, s4);
        c1.UpdateSong(9040493, s1);
        //
        Assert.DoesNotContain(s1, c1.Songs);
        Assert.DoesNotContain(s2, c1.Songs);
        Assert.Contains(s3, c1.Songs);
        Assert.Contains(s4, c1.Songs);

    }
    [TestMethod]
    [DataRow(17, "Noob", "2012-02-13 00:00:00", 45, "Hello!", 0)]
    public void SetMethods_Composer_SetsNewData(long id, string name, string joinDate, int birthYear, string desc, int available)
    {
        // Arrange
        Composer c1 = new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1);
        int age = DateTime.Today.Year - birthYear;
        // Act
        c1.SetID(id);
        c1.SetName(name);
        c1.SetJoinDate(DateTime.Parse(joinDate));
        c1.SetAge(age);
        c1.SetDescription(desc);
        c1.SetAvailability(available);
        // Assert
        Assert.AreEqual(id, c1.ID);
        Assert.AreEqual(name, c1.Name);
        Assert.AreEqual(joinDate, c1.JoinDate.ToString("yyyy-MM-dd HH:mm:ss"));
        Assert.AreEqual(age, c1.GetAge());
        Assert.AreEqual(desc, c1.Description);
        Assert.AreEqual(available, c1.OnNewgrounds);
    }
    [TestMethod]
    [DataRow(-1, "", 500)]
    public void SetMethods_Composer_KeepsOldData(long id, string name, int available)
    {
        // Arrange
        Composer c1 = new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1);
        Composer clone = (Composer)c1.Clone();
        // Act
        c1.SetID(id);
        c1.SetName(name);
        c1.SetAvailability(available);
        // Assert
        Assert.IsGreaterThanOrEqualTo(0, c1.ID);
        Assert.AreEqual(clone.Name, c1.Name);
        Assert.IsInRange(0, 1, c1.OnNewgrounds);
    }
}