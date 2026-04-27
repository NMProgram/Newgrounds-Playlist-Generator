namespace ObjectTests;

using System.Reflection;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using NAudio.MediaFoundation;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
[TestClass]
public sealed class AudioTests
{
    static byte[] GetBytes()
    {
        Random random = new();
        List<byte> bytes = [];
        for (int i = 0; i < Math.Pow(2, 10); i++)
        {
            bytes.Add((byte)random.Next(byte.MaxValue));
        }
        return [.. bytes];
    }
    [TestMethod]
    public async Task SetupAudio_AudioPlayer_ReturnsSubscribedTCSAndOutput()
    {
        // Arrange
        byte[] bytes = GetBytes();
        using RawSourceWaveStream wave = new(new MemoryStream(bytes), new(44100, 16, 1));
        VolumeSampleProvider provider = new(wave.ToSampleProvider())
        { Volume = 1.0f };
        // Act
        var (tcs, output) = AudioPlayer.SetupAudio(provider);
        var completed = await tcs.Task;
        // Assert
        Assert.AreEqual(1.0f, output.Volume);
        Assert.IsTrue(completed);
    }
    [TestMethod]
    public void TrimAudio_AudioPlayer_ReturnsTrimmedArray()
    {
        // Arrange
        string dir = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        string path = Path.Combine(dir, "8762_newgrounds_brent_.mp3");
        byte[] bytes = File.ReadAllBytes(path);
        // Act
        short[] newArr = AudioPlayer.TrimAudio(bytes);
        // Assert
        Assert.IsLessThan(bytes.Length, newArr.Length * 2);
    }
    [TestMethod]
    public void CalculateRMS_AudioPlayer_ReturnsRMSGainAndNewBytes()
    {
        // Arrange
        string dir = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        string path = Path.Combine(dir, "8762_newgrounds_brent_.mp3");
        byte[] bytes = File.ReadAllBytes(path);
        // Act
        (float gain, byte[] newArr) = AudioPlayer.CalculateRMS(bytes);
        // Assert
        Assert.IsLessThan(newArr.Length, bytes.Length);
        Assert.IsInRange(0.0f, 1.0f, 0.3f * gain);
    }
    [TestMethod]
    public async Task SetupPlayback_Audio_PreparesRMSCheck()
    {
        // Arrange
        string dir = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        string path = Path.Combine(dir, "8762_newgrounds_brent_.mp3");
        byte[] bytes = File.ReadAllBytes(path);
        Audio audio = new(bytes[..1000000]);
        // Act
        audio.SetupPlayback();
        _ = audio.PlayAsync();
        await Task.Delay(500);
    }
}