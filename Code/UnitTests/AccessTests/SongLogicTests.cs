using System.Data;

namespace DatabaseTests;

[TestClass]
public sealed class SongLogicTests : TestStartup
{
    [TestMethod]
    public void AddUpdateDelete_SongLogic_ReturnsSongOrNull()
    {
        // Arrange
        Song song = new(int.MaxValue, "Nonexistant", "2020-10-10 00:00:00", 15, -50, 1, []);
        // Act
        _sAccess.Add(song);
        Song? foundSong = _sAccess.GetByID(int.MaxValue);
        // Assert
        Assert.AreEqual(foundSong, song);
        // Act
        foundSong?.SetID(long.MaxValue);
        _sAccess.Update(song, foundSong!);
        Song? newSong = _sAccess.GetByID(long.MaxValue);
        Song? notAnymore = _sAccess.GetByID(int.MaxValue);
        // Assert
        Assert.AreEqual(newSong, foundSong);
        Assert.IsNull(notAnymore);
        // Act
        _sAccess.Delete(newSong!);
        Song? notHere = _sAccess.GetByID(long.MaxValue);
        // Assert
        Assert.IsNull(notHere);
    }
    [DoNotParallelize]
    [TestMethod]
    [DataRow(311087)]
    [DataRow(386900)]
    [DataRow(643474)]
    public void GetByID_SongLogic_ReturnsSongWithOneComp(long id)
    {
        // ^^^ Arrange ^^^
        SongLogic logic = new(Factory);
        // Act
        Song? song = _sAccess.GetByID(id);
        // Assert
        Assert.IsNotNull(song);
        Assert.HasCount(1, song.Composers);
    }
    [TestMethod]
    [DataRow(586990)]
    [DataRow(598682)]
    public void GetByID_SongLogic_ReturnsSongWithMultipleComps(long id)
    {
        // ^^^ Arrange ^^^
        // Act
        Song? song = _sAccess.GetByID(id);
        // Assert
        Assert.IsNotNull(song);
        Assert.HasCount(2, song.Composers);
    }
    [TestMethod]
    [DataRow(1)]
    [DataRow(-1)]
    public void GetByID_SongLogic_ReturnsNull(long id)
    {
        // ^^^ Arrange ^^^
        // Act
        Song? song = _sAccess.GetByID(id);
        // Assert
        Assert.IsNull(song);
    }
    [TestMethod]
    [DataRow(311087, -999999)]
    [DataRow(643474, 635000)]
    [DataRow(478433, 450000)]
    public void GetClosestMatch_SongLogic_ReturnsSong(long exp, long id)
    {
        // ^^^ Arrange ^^^
        // Act
        Song? song = _sAccess.GetClosestMatch(id);
        // Assert
        Assert.IsNotNull(song);
        Assert.AreEqual(exp, song.ID);
    }
    [TestMethod]
    [DataRow("Fantomenk")]
    [DataRow("Soulwind")]
    [DataRow("circ")]
    public void GetSongMatches_SongLogic_ReturnsSongOrSongs(string inp)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Song> songs = _sAccess.GetSongMatches(inp);
        // Assert
        Assert.IsNotEmpty(songs);
        CollectionAssert.AllItemsAreNotNull(songs.ToArray());
        Assert.IsNotEmpty(songs.First().Composers);
    }
    [TestMethod]
    [DataRow(1, 999999)]
    [DataRow(100000, 200000)]
    [DataRow(188711, 188711)]
    public void GetBetweenLevelIDs_SongLogic_ReturnsSongsOrSong(long low, long high)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Song> songs = _sAccess.GetBetweenLevelIDs(low, high);
        // Assert
        Assert.IsNotEmpty(songs);
        CollectionAssert.AllItemsAreNotNull(songs.ToArray());
        Assert.IsNotEmpty(songs.First().Composers);
    }
    [TestMethod]
    [DataRow(-int.MaxValue, -100000)]
    [DataRow(int.MaxValue, long.MaxValue)]
    public void GetBetweenLevelIDs_SongLogic_ReturnsEmpty(long low, long high)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Song> songs = _sAccess.GetBetweenLevelIDs(low, high);
        // Assert
        Assert.IsEmpty(songs);
    }
    [TestMethod]
    [DataRow(1, int.MaxValue)]
    [DataRow(503241, 725205)]
    [DataRow(598682, 598682)]
    public void GetBetweenSongData_IDs_ReturnsSongsOrSong(long low, long high)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Song> songs = _sAccess.GetBetweenSongData(low, high);
        // Assert
        Assert.IsNotEmpty(songs);
        CollectionAssert.AllItemsAreNotNull(songs.ToArray());
        Assert.IsNotEmpty(songs.First().Composers);
    }
    [TestMethod]
    [DataRow(-99999, 0)]
    [DataRow(4329843290, 5439053354)]
    public void GetBetweenSongData_IDs_ReturnsEmpty(long low, long high)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Song> songs = _sAccess.GetBetweenSongData(low, high);
        // Assert
        Assert.IsEmpty(songs);
    }
    [TestMethod]
    [DataRow("a", "z")]
    [DataRow("Albert", "Gottfried")]
    [DataRow("-", "/")]
    [DataRow("Tiny Tunes", "Tiny Tunes")]
    public void GetBetweenSongData_Names_ReturnsSongsOrSong(string low, string high)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Song> songs = _sAccess.GetBetweenSongData(low, high);
        // Assert
        Assert.IsNotEmpty(songs);
        CollectionAssert.AllItemsAreNotNull(songs.ToArray());
        Assert.IsNotEmpty(songs.First().Composers);
    }
    [TestMethod]
    [DataRow("zzzzzz", "zzzzzz")]
    [DataRow("\\", "\\")]
    public void GetBetweenSongData_Names_ReturnsEmpty(string low, string high)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Song> songs = _sAccess.GetBetweenSongData(low, high);
        // Assert
        Assert.IsEmpty(songs);
    }
    [TestMethod]
    [DataRow("2000-12-20", "2015-04-04")]
    [DataRow("2010-02-02", "2011-04-13")]
    [DataRow("2010-02-12", "2010-02-12")]
    public void GetBetweenSongData_Dates_ReturnsSongsOrSong(string low, string high)
    {
        // ^^^ Arrange ^^^
        DateTime start = DateTime.Parse(low);
        DateTime end = DateTime.Parse(high);
        // Act
        IEnumerable<Song> songs = _sAccess.GetBetweenSongData(start, end);
        // Assert
        Assert.IsNotEmpty(songs);
        CollectionAssert.AllItemsAreNotNull(songs.ToArray());
        Assert.IsNotEmpty(songs.First().Composers);
    }
    [TestMethod]
    [DataRow("1980-10-13", "1981-03-16")]
    [DataRow("2008-10-25", "2008-10-24")]
    public void GetBetweenSongData_Dates_ReturnsEmpty(string low, string high)
    {
        // ^^^ Arrange ^^^
        DateTime start = DateTime.Parse(low);
        DateTime end = DateTime.Parse(high);
        // Act
        IEnumerable<Song> songs = _sAccess.GetBetweenSongData(start, end);
        // Assert
        Assert.IsEmpty(songs);
    }
    [TestMethod]
    [DataRow("Video Game")]
    [DataRow("Dubstep")]
    [DataRow("Dance")]
    public void GetByGenre_SongLogic_ReturnsSongsOrSong(string genre)
    {
        // ^^^ Arrange ^^^
        var (_, val, _) = InputLogic.IsValidGenre(genre);
        // Act
        IEnumerable<Song> songs = _sAccess.GetByGenre(val);
        // Assert
        Assert.IsNotEmpty(songs);
        foreach (var song in songs) { Assert.AreEqual(val, song.Genre); }
        CollectionAssert.AllItemsAreNotNull(songs.ToArray());
        Assert.IsNotEmpty(songs.First().Composers);
    }
    [TestMethod]
    [DataRow("Bluegrass")]
    public void GetByGenre_SongLogic_ReturnsEmpty(string genre)
    {
        // ^^^ Arrange ^^^
        var (_, val, _) = InputLogic.IsValidGenre(genre);
        // Act
        IEnumerable<Song> songs = _sAccess.GetByGenre(val);
        // Assert
        Assert.IsEmpty(songs);
    }
    [TestMethod]
    [DataRow("Fantomenk")]
    [DataRow("Waterflame")]
    [DataRow("Ay")]
    public void GetSongsFromComposer_SongLogic_ReturnsSongsOrSong(string comp)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Song> songs = _sAccess.GetSongsFromComposer(comp);
        // Assert
        Assert.IsNotEmpty(songs);
        foreach (var song in songs) 
        { Assert.IsNotNull(song.Composers.FirstOrDefault(x => x.Name.Contains(comp))); }
        CollectionAssert.AllItemsAreNotNull(songs.ToArray());
        Assert.IsNotEmpty(songs.First().Composers);
    }
    [TestMethod]
    [DataRow("NNWNFKWIJOFWIJOK")]
    public void GetBetweenSongData_IDs_ReturnsEmpty(string comp)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Song> songs = _sAccess.GetSongsFromComposer(comp);
        // Assert
        Assert.IsEmpty(songs);
    }
    [TestMethod]
    public void GetUnavailableSongs_SongLogic_ReturnsSongs()
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Song> songs = _sAccess.GetUnavailableSongs();
        // Assert
        Assert.IsNotEmpty(songs);
        foreach (var song in songs) 
        { Assert.AreEqual(0, song.Available); }
        CollectionAssert.AllItemsAreNotNull(songs.ToArray());
        Assert.HasCount(2, songs.First().Composers);
    }
    [TestMethod]
    [DataRow(true, "598682")]
    [DataRow(true, "311087")]
    [DataRow(true, "643474")]
    [DataRow(false, "993042")]
    [DataRow(false, "Hello")]
    [DataRow(false, "")]
    [DataRow(false, null)]
    public void IsInDatabase_SongLogic_ReturnsTuple(bool exp, string id)
    {
        // ^^^ Arrange ^^^
        string[] errs = [
            "Please enter at least one character.",
            $"{id} is not a valid number.",
            $"{id} was not found in the database."
        ];
        // Act
        var (res, val, err) = _sAccess.IsInDatabase(id);
        // Assert
        Assert.AreEqual(exp, res);
        Assert.AreEqual(exp, val != -1);
        Assert.AreEqual(exp, !errs.Contains(err));
    }
    [TestMethod]
    [DataRow(true, "1")]
    [DataRow(true, "0")]
    [DataRow(true, "49320429")]
    [DataRow(false, "598682")]
    [DataRow(false, "Hello")]
    [DataRow(false, "")]
    [DataRow(false, null)]
    public void IsNotInDatabase_SongLogic_ReturnsTuple(bool exp, string id)
    {
        // ^^^ Arrange ^^^
        string[] errs = [
            "Please enter at least one character.",
            $"{id} is not a valid number.",
            $"{id} already exists in the database."
        ];
        // Act
        var (res, val, err) = _sAccess.IsNotInDatabase(id);
        // Assert
        Assert.AreEqual(exp, res);
        Assert.AreEqual(exp, val != -1);
        Assert.AreEqual(exp, !errs.Contains(err));
    }
    [TestMethod]
    [DataRow(true, "1")]
    [DataRow(true, "1700")]
    [DataRow(true, "2105492")]
    [DataRow(false, "99719")]
    [DataRow(false, "Hello")]
    [DataRow(false, "")]
    [DataRow(false, null)]
    public void IsUniqueLevelID_SongLogic_ReturnsTuple(bool exp, string levelID)
    {
        // ^^^ Arrange ^^^
        string[] errs = [
            "Please enter at least one character.",
            $"{levelID} is not a valid number.",
            $"Level ID {levelID} has already been used."
        ];
        // Act
        var (res, val, err) = _sAccess.IsUniqueLevelID(levelID);
        // Assert
        Assert.AreEqual(exp, res);
        Assert.AreEqual(exp, val != -1);
        Assert.AreEqual(exp, !errs.Contains(err));
    }
}
