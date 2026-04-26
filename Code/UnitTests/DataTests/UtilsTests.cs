using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace LogicTests;
[TestClass]
public sealed class UtilsTests
{
    [TestMethod]
    [DataRow("")]
    [DataRow("Hello!")]
    [DataRow(null)]
    public void Bold_StringUtils_ReturnsBoldString(string str)
    {
        // ^^^ Arrange ^^^
        // Act
        string bold = str.Bold();
        int count = bold.Count(x => x == '\u001b');
        // Assert
        Assert.AreEqual(2, count);
    }
    [TestMethod]
    [DataRow(2011, 15)]
    [DataRow(2025, 1)]
    [DataRow(1976, 50)]
    [DataRow(2050, -24)]
    public void ToYear_IntegerUtils_ReturnsBirthYear(long exp, long age)
    {
        // ^^^ Arrange ^^^
        // Act
        long years = age.ToYear();
        // Assert
        Assert.AreEqual(exp, years);
    }
    [TestMethod]
    [DataRow("Video Game", 15)]
    [DataRow("Hip Hop - Olskool", Genre.HipHopOlskool)]
    public void FormatGenre_EnumUtils_ReturnsGenreName(string exp, object genre)
    {
        // ^^^ Arrange ^^^
        // Act
        string format = genre.FormatGenre();
        // Assert
        Assert.AreEqual(exp, format);
    }
    [TestMethod]
    public void FormatAudio_ArrayUtils_ReturnsAudioTitle()
    {
        // Arrange
        string dir = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        string path = Path.Combine(dir, "8762_newgrounds_brent_.mp3");
        byte[] audio = File.ReadAllBytes(path);
        // Act
        string title = audio.FormatAudio();
        // Assert
        Assert.AreEqual("Elton (ID: 8762)", title);
    }
    [TestMethod]
    public void FormatSbyte_IntegerUtils_ReturnsSbyteBoolean()
    {
        // Arrange
        string[] results = ["Yes", "Yes", "No", "No"];
        sbyte[] sbytes = [30, 1, 0, -99];
        // Act
        var correct = results.Zip(sbytes).Where(x => x.First == x.Second.FormatSbyte()).ToArray();
        // Assert
        Assert.HasCount(4, correct);
    }
    [TestMethod]
    [DataRow("Hello; There!", "Hello\nThere!")]
    [DataRow("I'm pondering my existence; It's futile.", "I'm pondering my existence\nIt's futile.")]
    public void DescPrinter_StringUtils_ReturnsJoinedString(string exp, object desc)
    {
        // ^^^ Arrange ^^^
        // Act
        string res = desc.DescPrinter();
        // Assert
        Assert.AreEqual(exp, res);
    }
    [TestMethod]
    [DataRow("Jan 20, 2006", 20, 1, 2006)]
    [DataRow("Jul 09, 2021", 9, 7, 2021)]
    [DataRow("Feb 29, 1996", 29, 2, 1996)]
    public void FormatDate_DateTimeUtils_ReturnsMMM_dd_YYYY_Format(string exp, int day, int month, int year)
    {
        // ^^^ Arrange ^^^
        object obj = new DateTime(year, month, day); 
        // Act
        string date = obj.FormatDate();
        // Assert
        Assert.AreEqual(exp, date);
    }
    [TestMethod]
    [DataRow("Unknown", -1)]
    [DataRow("Unknown", -999)]
    [DataRow("20", 2006)]
    [DataRow("32", 1994)]
    public void AgeStr_IntegerUtils_ReturnsPrintableAge(string exp, long age)
    {
        // ^^^ Arrange ^^^
        // Act
        string ageVal = age.AgeStr();
        // Assert
        Assert.AreEqual(exp, ageVal);
    }
    [TestMethod]
    public void FormatSongID_SongUtils_ReturnsSongID()
    {
        // Arrange
        string[] exps = ["311087", "386900", "586990"];
        Song[] songs = [
            new(311087, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []),
            new(386900, "Silent Hill (Dubstep)", "2010-12-28 00:00:00", 8, 99719, 1, []),
            new(586990, "Aztech + Lockyn: Soulwind", "2014-09-07 00:00:00", 8, 148508, 1, []),
        ];
        // Act
        bool all = exps.Zip(songs).All(x => x.First == x.Second.FormatSongID());
        // Assert
        Assert.IsTrue(all);
    }
    [TestMethod]
    public void FormatCompName_ComposerUtils_ReturnsComposerName()
    {
        // Arrange
        string[] exps = ["Fantomenk", "Aydin-Jewelz123", "kiynaria"];
        Composer[] comps = [
            new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1),
            new(2, "Aydin-Jewelz123", "2007-04-20 00:00:00", 1992, "Back in the flow of things!", 1),
            new(3, "kiynaria", "2012-09-11 00:00:00", 1996, "I use FL Studio. Feel free to PM me about anything at all. All music posted here is free to use for NON-COMMERCIAL PURPOSES, just give me credit and we're all good. (Anything else must be discussed.) Thanks for stopping by!", 1)
        ];
        // Act
        bool all = exps.Zip(comps).All(x => x.First == x.Second.FormatCompName());
        // Assert
        Assert.IsTrue(all);
    }
    [TestMethod]
    public void Deconstruct_ArrayUtils_ReturnsExpectedOutParams()
    {
        // Arrange
        object[] objs = ["Oh no", 15D, new SongLogic(new InMemory()), 5E10, new Random(), new string[,] {{"String", "Yep"}, {"No", "Maybe"}}, new StringBuilder()];
        // Act
        objs.Deconstruct(out string val1, out double val2, out SongLogic val3, out double val4, out Random val5, out string[,] val6, out StringBuilder val7);
        // Assert
        Assert.IsInstanceOfType<string>(val1);
        Assert.IsInstanceOfType<double>(val2);
        Assert.IsInstanceOfType<SongLogic>(val3);
        Assert.IsInstanceOfType<double>(val4);
        Assert.IsInstanceOfType<Random>(val5);
        Assert.IsInstanceOfType<string[,]>(val6);
        Assert.IsInstanceOfType<StringBuilder>(val7);
    }
    [TestMethod]
    public void Deconstruct_ArrayUtils_EmptyOrNull()
    {
        // Arrange
        object[] empty = [];
        object[] none = null;
        // Act
        Action action1 = () => empty.Deconstruct(out string val1, out string val2);
        Action action2 = () => empty.Deconstruct(out int val3, out StringBuilder val4, out string[,] val5);
        // Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(action1);
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(action2);
    }
}