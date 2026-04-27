namespace ObjectTests;

[TestClass]
public sealed class CustomExceptionTests
{
    [TestMethod]
    [DataRow("Hello")]
    [DataRow("Invalid input")]
    [DataRow("User entered input to return to menu.")]
    [DataRow("")]
    public void Constructor_ReturnedException_HasMessage(string msg)
    {
        // Arrange
        ReturnedException re = new(msg);
        // Act
        bool hasMsg = re.Message == msg;
        // Assert
        Assert.IsTrue(hasMsg);
    }
    [TestMethod]
    public void Constructor_ReturnedException_HasNoMessage()
    {
        // Arrange
        ReturnedException re = new();
        // Act
        bool hasMsg = re.Message != "Exception of type \'ReturnedException\' was thrown.";
        // Assert
        Assert.IsFalse(hasMsg);
    }
    [TestMethod]
    [DataRow("Hello")]
    [DataRow("Invalid input")]
    [DataRow("User entered input to return to menu.")]
    [DataRow("")]
    public void Constructor_TracebackException_HasMessage(string msg)
    {
        // Arrange
        TracebackException re = new(msg);
        // Act
        bool hasMsg = re.Message == msg;
        // Assert
        Assert.IsTrue(hasMsg);
    }
    [TestMethod]
    public void Constructor_TracebackException_HasNoMessage()
    {
        // Arrange
        TracebackException re = new();
        // Act
        bool hasMsg = re.Message != "Exception of type \'TracebackException\' was thrown.";
        // Assert
        Assert.IsFalse(hasMsg);
    }
}