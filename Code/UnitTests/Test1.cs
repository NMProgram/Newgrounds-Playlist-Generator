using System.Data;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SQLitePCL;

namespace DatabaseTests;

[TestClass]
public sealed class SongAccessTests
{
    public static readonly IConnection Factory = new InMemory();
    // static SongAccessTests()
    // {
    //     SongAccess sAccess = new(Factory);
    //     ComposerAccess cAccess = new(Factory);
    //     SongComposerAccess scAccess = new(Factory);

    //     Song song1 = new(1, "Chilled 1", "2020-10-10 00:00:00", "Dubstep");
    //     Song song2 = new(2, "At the Speed of Light", "2012-10-10 00:00:00", "VideoGame");
    //     Composer composer1 = new(1, "Sean", "2020-10-05 00:00:00", "2010-10-05 00:00:00", "Hello");
    //     Composer composer2 = new(1, "Sean", "2020-10-05 00:00:00", "2010-10-05 00:00:00", "Hello");
    //     sAccess.Insert(song1);
    //     sAccess.Insert(song2);
    //     long val = cAccess.Insert(composer1);
    //     long val2 = cAccess.Insert(composer2);
    //     scAccess.Insert(new(song1.ID, val));
    //     scAccess.Insert(new(song1.ID, val2));
    //     scAccess.Insert(new(song2.ID, val));
    // }
    // [TestMethod]
    // public void TestMethod1()
    // {
    //     SongAccess sAccess = new(Factory);
    //     Song? song = sAccess.GetByID(1);
    //     Song? song2 = sAccess.GetByID(2);
    //     Assert.IsNotNull(song);
    //     Assert.HasCount(2, song.Composers);
    //     Assert.IsNotNull(song2);
    //     Assert.HasCount(1, song2.Composers);
    // }
    [TestMethod]
    [DataRow(Genre.VideoGame, "Video Game")]
    [DataRow(Genre.RNB, "R&B")]
    [DataRow(Genre.HipHopOlskool, "hIp hOp -OlskOOl")]
    public void IsValidGenre_InputLogic_ReturnsGenre(Genre exp, string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidGenre(str);
        // Assert
        Assert.IsTrue(res);
        Assert.AreEqual(exp, val);
        Assert.IsNull(err);
    }
}
