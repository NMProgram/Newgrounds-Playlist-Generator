using NAudio.Wave;

namespace ObjectTests;
[TestClass]
public sealed class CustomWaveProviderTests
{
    [TestMethod]
    public void Constructor_CustomWaveProvider_HasSetProperties()
    {
        // Arrange
        float[] pcm = [0.1f, 0.2f, 0.3f, 0.4f];
        WaveFormat format = new(44100, 32, 1);
        byte[] expected = new byte[pcm.Length * 4];
        Buffer.BlockCopy(pcm, 0, expected, 0, expected.Length);
        byte[] buffer = new byte[12];
        // Act
        CustomWaveProvider reader = new(pcm, format);
        int read = reader.Read(buffer, 0, 8);
        // Assert
        Assert.AreEqual(format, reader.WaveFormat);
        Assert.AreEqual(8, read);
        Assert.AreEqual(string.Join(", ", expected.Take(8)), string.Join(", ", buffer.Take(8)));
    }
}