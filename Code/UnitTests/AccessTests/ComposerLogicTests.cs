using System.Data;

namespace DatabaseTests;

[TestClass]
public sealed class ComposerLogicTests : TestStartup
{
    [TestMethod]
    public void AddUpdateDelete_ComposerLogic_ReturnsComposerOrNull()
    {
        // Arrange
        Composer comp = new(0, "-", "2020-10-10 00:00:00", 2000, "", 1);
        // Act
        _cAccess.Add(comp);
        Composer? foundComp = _cAccess.GetByID("-");
        // Assert
        Assert.AreEqual(foundComp, comp);
        // Act
        foundComp?.SetName("*");
        _cAccess.Update(comp, foundComp!);
        Composer? newComp = _cAccess.GetByID("*");
        Composer? notAnymore = _cAccess.GetByID("-");
        // Assert
        Assert.AreEqual(newComp, foundComp);
        Assert.IsNull(notAnymore);
        // Act
        _cAccess.Delete(newComp!.ID);
        Composer? notHere = _cAccess.GetByID("*");
        // Assert
        Assert.IsNull(notHere);
    }
    [TestMethod]
    [DataRow("Aydin-Jewelz123")]
    [DataRow("Fantomenk")]
    [DataRow("KDrew")]
    public void GetByID_ComposerLogic_ReturnsComposerWithOneSong(string name)
    {
        // ^^^ Arrange ^^^
        // Act
        Composer? comp = _cAccess.GetByID(name);
        // Assert
        Assert.IsNotNull(comp);
        Assert.HasCount(1, comp.Songs);
    }
    [TestMethod]
    [DataRow("Waterflame")]
    public void GetByID_ComposerLogic_ReturnsComposerWithMultipleSongs(string name)
    {
        // ^^^ Arrange ^^^
        // Act
        Composer? comp = _cAccess.GetByID(name);
        // Assert
        Assert.IsNotNull(comp);
        Assert.HasCount(2, comp.Songs);
    }
    [TestMethod]
    [DataRow("")]
    [DataRow("Nothing")]
    public void GetByID_ComposerLogic_ReturnsNull(string name)
    {
        // ^^^ Arrange ^^^
        // Act
        Composer? comp = _cAccess.GetByID(name);
        // Assert
        Assert.IsNull(comp);
    }
    [TestMethod]
    [DataRow("Fantomenk")]
    [DataRow("Wat")]
    [DataRow("asus")]
    public void GetComposerMatches_ComposerLogic_ReturnsCompOrComps(string inp)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Composer> comps = _cAccess.GetComposerMatches(inp);
        // Assert
        Assert.IsNotEmpty(comps);
        CollectionAssert.AllItemsAreNotNull(comps.ToArray());
        Assert.IsNotEmpty(comps.First().Songs);
    }
    [TestMethod]
    [DataRow("a", "z")]
    [DataRow("Albert", "Sean")]
    [DataRow("helpegasus", "HelpegasuS")]
    public void GetBetweenCompData_Names_ReturnsCompsOrComp(string low, string high)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Composer> comps = _cAccess.GetBetweenCompData(low, high);
        // Assert
        Assert.IsNotEmpty(comps);
        CollectionAssert.AllItemsAreNotNull(comps.ToArray());
        Assert.IsNotEmpty(comps.First().Songs);
    }
    [TestMethod]
    [DataRow("zzzzzz", "zzzzzz")]
    [DataRow("\\", "\\")]
    public void GetBetweenCompData_Names_ReturnsEmpty(string low, string high)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Composer> comps = _cAccess.GetBetweenCompData(low, high);
        // Assert
        Assert.IsEmpty(comps);
    }
    [TestMethod]
    [DataRow("2000-12-20", "2015-04-04")]
    [DataRow("2010-02-02", "2011-04-13")]
    [DataRow("2008-10-26", "2008-10-26")]
    public void GetBetweenCompData_Dates_ReturnsCompsOrComp(string low, string high)
    {
        // ^^^ Arrange ^^^
        DateTime start = DateTime.Parse(low);
        DateTime end = DateTime.Parse(high);
        // Act
        IEnumerable<Composer> comps = _cAccess.GetBetweenCompData(start, end);
        // Assert
        Assert.IsNotEmpty(comps);
        CollectionAssert.AllItemsAreNotNull(comps.ToArray());
        Assert.IsNotEmpty(comps.First().Songs);
    }
    [TestMethod]
    [DataRow("1980-10-13", "1981-03-16")]
    [DataRow("2008-10-25", "2008-10-24")]
    public void GetBetweenCompData_Dates_ReturnsEmpty(string low, string high)
    {
        // ^^^ Arrange ^^^
        DateTime start = DateTime.Parse(low);
        DateTime end = DateTime.Parse(high);
        // Act
        IEnumerable<Composer> comps = _cAccess.GetBetweenCompData(start, end);
        // Assert
        Assert.IsEmpty(comps);
    }
    [TestMethod]
    [DataRow(18, 35)]
    [DataRow(30, 40)]
    [DataRow(37, 37)]
    public void GetBetweenCompData_Age_ReturnsCompsOrComp(long low, long high)
    {
        // ^^^ Arrange ^^^

        // Act
        IEnumerable<Composer> comps = _cAccess.GetBetweenCompData(low, high);
        // Assert
        Assert.IsNotEmpty(comps);
        CollectionAssert.AllItemsAreNotNull(comps.ToArray());
        Assert.IsNotEmpty(comps.First().Songs);
    }
    [TestMethod]
    [DataRow(1, 1)]
    [DataRow(-15, 2)]
    public void GetBetweenCompData_Age_ReturnsEmpty(long low, long high)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Composer> comps = _cAccess.GetBetweenCompData(low, high);
        // Assert
        Assert.IsEmpty(comps);
    }
    [TestMethod]
    public void GetUnavailableComposers_ComposerLogic_ReturnsComp()
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Composer> comps = _cAccess.GetUnavailableComposers();
        // Assert
        Assert.IsNotEmpty(comps);
        foreach (var comp in comps) 
        { Assert.AreEqual(0, comp.OnNewgrounds); }
        CollectionAssert.AllItemsAreNotNull(comps.ToArray());
        Assert.HasCount(1, comps);
    }
    [TestMethod]
    [DataRow(2, 598682)]
    [DataRow(1, 311087)]
    public void GetBySongID_ComposerLogic_ReturnsCompsOrComp(int count, long id)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Composer> comps = _cAccess.GetBySongID(id);
        // Assert
        Assert.IsNotEmpty(comps);
        foreach (var comp in comps) 
        { Assert.IsTrue(comp.Songs.All(x => x.ID == id)); }
        CollectionAssert.AllItemsAreNotNull(comps.ToArray());
        Assert.HasCount(count, comps);
    }
    [TestMethod]
    [DataRow("circles")]
    [DataRow("Electroman")]
    [DataRow("Tin")]
    public void GetBySongName_ComposerLogic_ReturnsCompsOrComp(string name)
    {
        // ^^^ Arrange ^^^
        // Act
        IEnumerable<Composer> comps = _cAccess.GetBySongName(name);
        // Assert
        Assert.IsNotEmpty(comps);
        foreach (var comp in comps) 
        { Assert.IsNotNull(comp.Songs.FirstOrDefault(x => x.Name.Contains(name))); }
        CollectionAssert.AllItemsAreNotNull(comps.ToArray());
        Assert.IsNotEmpty(comps.First().Songs);
    }
    [TestMethod]
    [DataRow(true, "Waterflame")]
    [DataRow(true, "Fantomenk")]
    [DataRow(true, "KDrew")]
    [DataRow(false, "Invalid")]
    public void IsInDatabase_ComposerLogic_ReturnsTuple(bool exp, string name)
    {
        // ^^^ Arrange ^^^
        
        // Act
        var (res, val, err) = _cAccess.IsInDatabase(name);
        // Assert
        Assert.AreEqual(exp, res);
        Assert.AreEqual(exp, !string.IsNullOrEmpty(val));
        Assert.AreEqual(exp, err is null);
    }
    [TestMethod]
    [DataRow(false, "Waterflame")]
    [DataRow(false, "Fantomenk")]
    [DataRow(false, "KDrew")]
    [DataRow(true, "Invalid")]
    public void IsNotInDatabase_ComposerLogic_ReturnsTuple(bool exp, string name)
    {
        // ^^^ Arrange ^^^
        
        // Act
        var (res, val, err) = _cAccess.IsNotInDatabase(name);
        // Assert
        Assert.AreEqual(exp, res);
        Assert.AreEqual(exp, !string.IsNullOrEmpty(val));
        Assert.AreEqual(exp, err is null);
    }
    [TestMethod]
    [DataRow(true, "Waterflame", 643474)]
    [DataRow(true, "Fantomenk", 386900)]
    [DataRow(true, "KDrew", 311087)]
    [DataRow(false, "Helpegasus", 598682)]
    public void IsNewSong_ComposerLogic_ReturnsTuple(bool exp, string name, long id)
    {
        // ^^^ Arrange ^^^
        
        // Act
        var (res, val, err) = _cAccess.IsNewSong(name, id);
        // Assert
        Assert.AreEqual(exp, res);
        Assert.AreEqual(exp, val != -1);
        Assert.AreEqual(exp, err is null);
    }
    [TestMethod]
    [DataRow(false, "Waterflame", 643474)]
    [DataRow(false, "Fantomenk", 386900)]
    [DataRow(false, "KDrew", 311087)]
    [DataRow(true, "Helpegasus", 598682)]
    public void IsNotNewSong_ComposerLogic_ReturnsTuple(bool exp, string name, long id)
    {
        // ^^^ Arrange ^^^
        
        // Act
        var (res, val, err) = _cAccess.IsNotNewSong(name, id);
        // Assert
        Assert.AreEqual(exp, res);
        Assert.AreEqual(exp, val != -1);
        Assert.AreEqual(exp, err is null);
    }
}
