using System.Diagnostics.CodeAnalysis;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
public static class AudioPlayer
{
    const float _minRMS = 1E-3f;
    const int _blockSize = 2048;
    const int _samplesPerBlock = _blockSize * 2;
    public static (TaskCompletionSource<bool>, WaveOutEvent) SetupAudio(ISampleProvider provider)
    {
        WaveOutEvent output = new();
        TaskCompletionSource<bool> tcs = new();
        output.PlaybackStopped += (s, e) => tcs.TrySetResult(true); // subscribes PlaybackStopped event to tcs
        output.Init(provider);
        output.Play();
        return (tcs, output);
    }
    public static short[] TrimAudio(byte[] bytes)
    {
        short[] samples = bytes.ToShorts();
        int first = -1; int last = -1;
        for (int b = 0; b < samples.Length / _samplesPerBlock; b++)
        {
            if (CheckBlock(b, samples, out double rms) && rms > _minRMS)
            { first = first == -1 ? b : first; last = b; }
        }
        if (first == -1 || last == -1) { return samples; }
        int start = first * _samplesPerBlock; int end = last * _samplesPerBlock;
        return samples[start..(end + 1)];
    }
    static byte[] CreatePCMBytes(byte[] mp3Bytes)
    {
        using var reader = new Mp3FileReader(new MemoryStream(mp3Bytes));
        using var resampler = new MediaFoundationResampler(reader, new WaveFormat(44100, 16, 2))
        { ResamplerQuality = 60 };
        using var ms = new MemoryStream();
        byte[] buffer = new byte[8192]; // reads in chunks
        int read;
        while ((read = resampler.Read(buffer, 0, buffer.Length)) > 0)
        { ms.Write(buffer, 0, read); }
        return ms.ToArray();
    }
    public static (float, byte[]) CalculateRMS(byte[] mp3Bytes)
    {
        byte[] pcmBytes = CreatePCMBytes(mp3Bytes);
        if (pcmBytes.Length < 2)
        { return (0, pcmBytes); }
        short[] samples = TrimAudio(pcmBytes);
        int blocks = samples.Length / _samplesPerBlock;
        double totalRMS = 0; int valid = 0;
        for (int b = 0; b < blocks; b++)
        {
            if (CheckBlock(b, samples, out double rms))
            { totalRMS += rms; valid++; }
        }
        if (totalRMS == 0) { return (0, samples.ToBytes()); }
        float rmsVal = (float)(totalRMS / valid);
        return (rmsVal > 0f ? 0.3f / rmsVal : 1f, samples.ToBytes());
    }
    static bool CheckBlock(int block, short[] samples, out double rms)
    {
        double sum = 0;
        int start = block * _samplesPerBlock;
        for (int i = 0; i < _samplesPerBlock; i++)
        {
            if (start + i >= samples.Length) { break; }
            double s = samples[start + i] / 32768.0;
            sum += s * s;
        }
        rms = Math.Sqrt(sum / _samplesPerBlock);
        return rms > _minRMS;
    }
}