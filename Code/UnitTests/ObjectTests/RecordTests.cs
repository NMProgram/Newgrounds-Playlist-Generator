namespace ObjectTests;
[TestClass]
public sealed class RecordTests
{
    [TestMethod]
    public void Constructor_Step_HasSetProperties()
    {
        // Arrange
        string key = "Hello";
        Func<object> action = () => 15 * 100;
        Func<object, string> repr = x => $"{x} is a cool number!";
        // Act
        Step step = new(key, action, repr);
        object result = action();
        // Assert
        Assert.AreEqual(key, step.Key);
        Assert.AreEqual(action(), step.Action());
        Assert.AreEqual(repr(result), step.Formatter(result));
    }
}