using NAudio.Wave;

public class CustomWaveProvider : IWaveProvider
{
    private readonly float[] PCM;
    private readonly WaveFormat Format;
    private int Pos;
    public CustomWaveProvider(float[] pcm, WaveFormat format)
    {
        PCM = pcm;
        Format = format;
    }
    public WaveFormat WaveFormat => Format;
    public int Read(byte[] buffer, int offset, int count)
    {
        int samplesToCopy = Math.Min(PCM.Length - Pos, count / 4);
        Buffer.BlockCopy(PCM, Pos * 4, buffer, offset, samplesToCopy * 4);
        Pos += samplesToCopy;
        return samplesToCopy * 4;
    }
}