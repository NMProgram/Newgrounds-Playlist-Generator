namespace LogicTests;

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
        Assert.AreEqual("Please enter at least one character.", err);
    }
    [TestMethod]
    [DataRow("1")]
    [DataRow("15")]
    [DataRow("22")]
    public void IsBetween_Ints_Success(string value)
    {
        // ^^^ Arrange ^^^
        int[] values = ConversionLogic.CreateNumArr(1, 22, 1);
        // Act
        var (res, val, err) = InputLogic.IsBetween(value, values.Min(), values.Max());
        // Assert
        Assert.IsTrue(res);
        Assert.AreNotEqual(-1, val);
        Assert.IsNull(err);
    }
    [TestMethod]
    [DataRow("-1")]
    [DataRow("Hello")]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("0")]
    [DataRow("99")]
    [DataRow("23")]
    public void IsBetween_Ints_Fail(string value)
    {
        // ^^^ Arrange ^^^
        string[] errs = [
            "Please enter at least one character.",
            $"{value} is not a valid number.",
            $"{value} is not between 1 and 22."
        ];
        int[] values = ConversionLogic.CreateNumArr(1, 22, 1);
        // Act
        var (res, val, err) = InputLogic.IsBetween(value, values.Min(), values.Max());
        // Assert
        Assert.IsFalse(res);
        Assert.IsLessThanOrEqualTo(0, val);
        Assert.Contains(err, errs);
    }
    [TestMethod]
    [DataRow(true, "YeS")]
    [DataRow(false, "nO")]
    [DataRow(true, "Yo")]
    public void IsBoolean_InputLogic_Success(bool exp, string str)
    {
        // ^^^ Arrange ^^^
        // Act
        var (res, val, err) = InputLogic.IsBoolean(str);
        // Assert
        Assert.IsTrue(res);
        Assert.AreEqual(exp, val);
        Assert.IsNull(err);
    }
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [DataRow("Woah")]
    [DataRow("Unnoticed")]
    [DataRow("Aye")]
    public void IsBoolean_InputLogic_Fail(string str)
    {
        // Arrange
        string[] errs = [
            "Please enter at least one character.",
            $"Please enter an answer starting with {"y".Bold()} or {"n".Bold()}."
        ];
        // Act
        var (res, val, err) = InputLogic.IsBoolean(str);
        // Assert
        Assert.IsFalse(res);
        Assert.IsFalse(val);
        Assert.Contains(err, errs);
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
        string[] errs = [
            "Please enter at least one character.",
            $"{str} is not a valid number."
        ];
        // Act
        var (res, val, err) = InputLogic.IsValidInteger(str);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(-1, val);
        Assert.Contains(err, errs);
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
    [DataRow("Crazy")]
    [DataRow("")]
    [DataRow(null)]
    public void IsValidDate_Date_Fail(string str)
    {
        // ^^^ Arrange ^^^
        string[] formats = [
        "MM/dd/yyyy",
        "MM/d/yyyy",
        "M/dd/yyyy",
        "M/d/yyyy",
        "MM/dd/yy",
        "M/dd/yy",
        "MM/d/yy",
        "M/d/yy",
        "MMM d, yyyy",
        "MMM dd, yyyy"
        ];
        string[] errs = [
            "Please enter at least one character.",
            $"{str} is not a valid date.\nPlease enter one of the following formats: {'\n' + string.Join(", ", formats)}"
        ];
        // Act
        var (res, val, err) = InputLogic.IsValidDate(str);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(DateTime.MinValue, val);
        Assert.Contains(err, errs);
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
    [DataRow("", 2000)]
    [DataRow(null, 2000)]
    public void IsValidDate_DateAndBirthYear_Fail(string str, long birthYear)
    {
        // ^^^ Arrange ^^^
        string[] formats = [
        "MM/dd/yyyy",
        "MM/d/yyyy",
        "M/dd/yyyy",
        "M/d/yyyy",
        "MM/dd/yy",
        "M/dd/yy",
        "MM/d/yy",
        "M/d/yy",
        "MMM d, yyyy",
        "MMM dd, yyyy"
        ];
        string[] errs = [
            "Please enter at least one character.",
            $"{str} is not a valid date.\nPlease enter one of the following formats: {'\n' + string.Join(", ", formats)}",
            $"The Join Date \'{str:MMM dd, yyyy}\' cannot be earlier than the birth year \'{birthYear}\'."
        ];
        // Act
        var (res, val, err) = InputLogic.IsValidDate(str, birthYear);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(DateTime.MinValue, val);
        Assert.Contains(err, errs);
    }

    [TestMethod]
    [DataRow("20", 2007)]
    [DataRow("30", 2025)]
    [DataRow("56", 1970)]
    public void IsValidAge_InputLogic_Success(string age, int year)
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
    [DataRow("10", 1900)]
    [DataRow("0", 2025)]
    [DataRow("999", 1000)]
    [DataRow("", 1900)]
    [DataRow(null, 0)]
    [DataRow("Hello", 1550)]
    public void IsValidAge_Fail(string age, int year)
    {
        // ^^^ Arrange ^^^
        string[] errs = [
            "Please enter at least one character.",
            $"{age} is not a valid number.",
            $"The Composer cannot be younger than the Join Date ({age} <= {DateTime.Today.Year - year})."
        ];
        // Act
        var (res, val, err) = InputLogic.IsValidAge(age, year);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(-1, val);
        Assert.Contains(err, errs);
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
    [DataRow("Vidio Game")]
    [DataRow("")]
    [DataRow(null)]
    public void IsValidGenre_InputLogic_Fail(string str)
    {
        // ^^^ Arrange ^^^
        string[] names = [.. ConversionLogic.GetGenreNames().Order()];
        string[] errs = [
            "Please enter at least one character.",
            $"{str} is not a valid Genre.\nPlease enter one of the following options:\n{string.Join(", ", names)}"
        ];
        // Act
        var (res, val, err) = InputLogic.IsValidGenre(str);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(0, (int)val);
        Assert.Contains(err, errs);
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
    [DataRow("Something else")]
    [DataRow("-1")]
    [DataRow("")]
    [DataRow(null)]
    public void IsValidAvailability_InputLogic_Fail(string str)
    {
        // ^^^ Arrange ^^^
        string[] errs = [
            "Please enter at least one character.",
            "Please enter a valid availability."
        ];
        // Act
        var (res, val, err) = InputLogic.IsValidAvailability(str);
        // Assert
        Assert.IsFalse(res);
        Assert.AreEqual(-1, val);
        Assert.Contains(err, errs);
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
    [DataRow("testingPNG.png")]
    [DataRow("")]
    [DataRow(null)]

    public void IsValidMP3_InputLogic_Fail(string str)
    {
        // ^^^ Arrange ^^^
        string? fileName = "..\\UnitTests";
        string dir = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        string path = dir;
        if (str?.Length > 0) 
        { 
            path = Path.Combine(dir, str);
            fileName = ".." + str[Math.Max(str.LastIndexOf('/'), 0)..]; 
        }
        string[] errs = [
            "Please enter at least one character.",
            $"The given file \'{path}\' was not found.",
            $"The given file \'{fileName}\' was not found.",
            "The given file given cannot be used as an MP3.",
        ];
        // Act
        var (res, val, err) = InputLogic.IsValidMP3(path);
        // Assert
        Assert.IsFalse(res);
        Assert.IsEmpty(val);
        Assert.Contains(err, errs);
    }
}