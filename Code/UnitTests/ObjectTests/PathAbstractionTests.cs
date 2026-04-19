using System.Reflection.Metadata;

namespace ObjectTests;

[TestClass]
public sealed class PathAbstractionTests
{
    [TestMethod]
    [DataRow("Memory.mp3", "8762_newgrounds_brent_.mp3")]
    public void Constructor_PathAbstraction_HasProperties(string name, string fileName)
    {
        // Arrange
        string dir = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
        string path = Path.Combine(dir, fileName);
        MemoryStream stream = new(File.ReadAllBytes(path));
        // Act
        PathAbstraction abstraction = new(name, stream);
        // Assert
        Assert.AreEqual(name, abstraction.Name);
        Assert.AreEqual(stream, abstraction.ReadStream);
        Assert.AreEqual(stream, abstraction.WriteStream);
    }
    [TestMethod]
    public void CloseStream_PathAbstraction_ClosesReadAndWrite()
    {
        string dir = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
        string path = Path.Combine(dir, "8762_newgrounds_brent_.mp3");
        MemoryStream ms = new(File.ReadAllBytes(path));
        PathAbstraction pa = new("Memory.mp3", ms);
        // Act
        pa.CloseStream(ms);
        // Assert
        Assert.ThrowsExactly<ObjectDisposedException>(() => ms.Read(new())); 
    }
    [TestMethod]
    public void AbstractMP3Path_PathAbstractionInTagLib_CreatesNewFile()
    {
        // Arrange
        string dir = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
        string path = Path.Combine(dir, "8762_newgrounds_brent_.mp3");
        using MemoryStream ms = new(File.ReadAllBytes(path));
        // Act
        var file = TagLib.File.Create(new PathAbstraction("in-memory.mp3", ms));
        // Assert
        Assert.IsNotNull(file);
        Assert.IsNotNull(file.Tag);
        Assert.IsGreaterThan(0, file.Properties.AudioBitrate);
    }
}