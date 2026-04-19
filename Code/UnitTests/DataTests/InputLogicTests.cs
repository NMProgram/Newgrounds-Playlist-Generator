namespace DataTests;

[TestClass]
public sealed class InputLogicTests
{
    [TestMethod]
    [DataRow("Hello")]
    [DataRow("H")]
    [DataRow("DWKLFJWRKJLSJFDKSLJDKSJK")]
    public void IsNotEmpty_InputLogic_Success(string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsNotEmpty(str);
        // Assert
        Assert.IsTrue(res);
        Assert.IsNotEmpty(val);
        Assert.IsNull(err);
    }
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void IsNotEmpty_InputLogic_Fail(string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsNotEmpty(str);
        // Assert
        Assert.IsFalse(res);
        Assert.IsEmpty(val);
        Assert.IsNotNull(err);
    }
    [TestMethod]
    [DataRow(1)]
    [DataRow(15)]
    [DataRow(22)]
    public void IsInOptions_Ints_Success(int value)
    {
        // ^^^ Arrange ^^^
        int[] values = ConversionLogic.CreateNumArr(1, 22, 1);
        // Act
        var (res, val, err) = InputLogic.IsInOptions(value, values);
        // Assert
        Assert.IsTrue(res);
        Assert.AreNotEqual(-1, val);
        Assert.IsNull(err);
    }
    [TestMethod]
    [DataRow(-1)]
    [DataRow(0)]
    [DataRow(23)]
    public void IsNotEmpty_Ints_Fail(int value)
    {
        // ^^^ Arrange ^^^
        int[] values = ConversionLogic.CreateNumArr(1, 22, 1);
        // Act
        var (res, val, err) = InputLogic.IsInOptions(value, values);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(0, val);
        Assert.IsNotNull(err);
    }

    [TestMethod]
    [DataRow("1")]
    [DataRow("0")]
    [DataRow("1234567")]
    public void IsValidInteger_InputLogic_Success(string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidInteger(str);
        // Assert
        Assert.IsTrue(res);
        Assert.AreNotEqual(-1, val);
        Assert.IsNull(err);
    }
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [DataRow("Crazy")]
    public void IsValidInteger_InputLogic_Fail(string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidInteger(str);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(-1, val);
        Assert.IsNotNull(err);
    }

    [TestMethod]
    [DataRow("03/18/2006")]
    [DataRow("03/8/2006")]
    [DataRow("3/18/2006")]
    [DataRow("3/8/2006")]
    [DataRow("03/18/06")]
    [DataRow("3/18/06")]
    [DataRow("03/8/06")]
    [DataRow("3/8/06")]
    [DataRow("Mar 18, 2006")]
    [DataRow("Mar 8, 2006")]
    public void IsValidDate_Date_Success(string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidDate(str);
        // Assert
        Assert.IsTrue(res);
        Assert.AreNotEqual(DateTime.MinValue, val);
        Assert.IsNull(err);
    }
    [TestMethod]
    [DataRow("03-18-2006")]
    [DataRow("18/03/2006")]
    [DataRow("")]
    [DataRow(null)]
    [DataRow("Crazy")]
    public void IsValidDate_Date_Fail(string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidDate(str);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(DateTime.MinValue, val);
        Assert.IsNotNull(err);
    }

    [TestMethod]
    [DataRow("03/18/2006", 1999)]
    [DataRow("03/8/2006", 2006)]
    [DataRow("3/18/2006", 1700)]
    public void IsValidDate_DateAndBirthYear_Success(string str, long birthYear)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidDate(str, birthYear);
        // Assert
        Assert.IsTrue(res);
        Assert.AreNotEqual(DateTime.MinValue, val);
        Assert.IsNull(err);
    }
    [TestMethod]
    [DataRow("03/18/2006", 2007)]
    [DataRow("03/8/2006", 2020)]
    [DataRow("18/03/2006", 2000)]
    public void IsValidDate_DateAndBirthYear_Fail(string str, long birthYear)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidDate(str, birthYear);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(DateTime.MinValue, val);
        Assert.IsNotNull(err);
    }

    [TestMethod]
    [DataRow(20, 2007)]
    [DataRow(30, 2025)]
    [DataRow(56, 1970)]
    public void IsValidAge_InputLogic_Success(long age, int year)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidAge(age, year);
        // Assert
        Assert.IsTrue(res);
        Assert.AreNotEqual(-1, val);
        Assert.IsNull(err);
    }
    [TestMethod]
    [DataRow(10, 1900)]
    [DataRow(0, 2025)]
    [DataRow(999, 1000)]
    public void IsValidAge_Fail(long age, int year)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidAge(age, year);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(-1, val);
        Assert.IsNotNull(err);
    }

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
    [TestMethod]
    [DataRow("Nothing")]
    [DataRow("")]
    public void IsValidGenre_InputLogic_Fail(string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidGenre(str);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(0, (int)val);
        Assert.IsNotNull(err);
    }

    [TestMethod]
    [DataRow(1, "1")]
    [DataRow(0, "0")]
    [DataRow(1, "True")]
    [DataRow(0, "False")]
    [DataRow(1, "Yes")]
    [DataRow(0, "No")]
    public void IsValidAvailability_InputLogic_Success(int exp, string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidAvailability(str);
        // Assert
        Assert.IsTrue(res);
        Assert.AreEqual(exp, val);
        Assert.IsNull(err);
    }
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [DataRow("Something else")]
    [DataRow("-1")]
    public void IsValidAvailability_InputLogic_Fail(string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidAvailability(str);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(-1, val);
        Assert.IsNotNull(err);
    }
    [TestMethod]
    [DataRow("\"8762_newgrounds_brent_.mp3\"")]
    [DataRow("8762_newgrounds_brent_.mp3\"")]
    [DataRow("8762_newgrounds_brent_.mp3")]
    public void IsValidMP3_InputLogic_Success(string path)
    {
        // ^^^ Arrange ^^^
        string dir = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        string pth = Path.Combine(dir, path);
        // Act
        var (res, val, err) = InputLogic.IsValidMP3(pth);
        // Assert
        Assert.IsTrue(res);
        Assert.IsNotEmpty(val);
        Assert.IsNull(err);
    }
    [TestMethod]
    [DataRow("Random/path/to/nothing.mp3")]
    [DataRow("")]
    [DataRow(null)]
    public void IsValidMP3_InputLogic_Fail(string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsValidMP3(str);
        // Assert
        Assert.IsFalse(res);
        Assert.IsEmpty(val);
        Assert.IsNotNull(err);
    }
}