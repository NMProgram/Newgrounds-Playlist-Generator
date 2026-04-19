namespace DataTests;

[TestClass]
public sealed class StringUtilsTests
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
}