using Microsoft.Data.Sqlite;

namespace ObjectTests;

[TestClass]
public sealed class IConnectionTests
{
    [TestMethod]
    [DataRow(null)]
    [DataRow("old.db")]
    public void GetConnection_InFile_ReturnsConString(string fileName)
    {
        // Arrange
        InFile iF = new();
        // Act
        SqliteConnection con = iF.Connect(fileName);
        // Assert
        Assert.AreEqual($"Data Source=Data/DataSource\\{fileName ?? "database.db"}", con.ConnectionString);
    }
}