using System.Buffers;
using System.Numerics;
using NAudio.Wave;
public class AudioPlayer
{
    byte[] Audio { get; }
    public AudioPlayer(byte[] audio) => Audio = audio;
    public async Task<WaveFileReader> CreateWavAsync(double target)
        => await Task.Run(() => CreateWav(target));
    WaveFileReader CreateWav(double target)
    {
        float[] pcm = RunBuffer(out WaveFormat format);
        ApplyGain(pcm, (float)Math.Pow(10, (target - CalculateRMS(pcm)) / 20));
        MemoryStream wavStream = new();
        WaveFileWriter.WriteWavFileToStream(wavStream, new CustomWaveProvider(pcm, format));
        wavStream.Position = 0;
        return new(wavStream);
    }
    float[] RunBuffer(out WaveFormat format)
    {
        Mp3FileReader reader = new(new MemoryStream(Audio, writable: false));
        var provider = reader.ToSampleProvider();
        float[] buffer = ArrayPool<float>.Shared.Rent(262144);
        List<float> samples = []; format = provider.WaveFormat; int read;
        while ((read = provider.Read(buffer, 0, buffer.Length)) > 0)
        { samples.AddRange(buffer.AsSpan(0, read)); }
        ArrayPool<float>.Shared.Return(buffer);
        return [.. samples];
    }
    double CalculateRMS(float[] buffer)
    {
        double sum = 0; long count = buffer.Length;
        int i = 0; int simd = Vector<float>.Count;
        for (; i <= buffer.Length - simd; i += simd)
        {
            Vector<float> vector = new(buffer, i);
            sum += Vector.Dot(vector, vector);
        }
        for (; i < buffer.Length; i++)
        { sum += buffer[i] * buffer[i]; }
        return 20 * Math.Log10(Math.Sqrt(sum / count));
    }
    void ApplyGain(float[] buffer, float factor)
    {
        for (int i = 0; i < buffer.Length; i++)
        { buffer[i] *= factor; }
    }
}