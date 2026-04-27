using NAudio.Wave;
using NAudio.Wave.SampleProviders;

public class Audio
{
    public byte[] AudioBytes { get; private set; }
    readonly Audio? _current = null;
    static WaveOutEvent? _output = null;
    static VolumeSampleProvider? _provider = null;
    readonly Task<(float, byte[])> _rmsSetup;
    public Audio(byte[] bytes)
    {
        AudioBytes = bytes;
        _rmsSetup = Task.Run(() => AudioPlayer.CalculateRMS(AudioBytes));
        _current?.RequestFadeOut();
        _current = this;
    }
    static async Task FadeOutAsync()
    {
        if (_output is null || _provider is null) { return; }
        const int DELAY = 5; const int TIME = 500; // in ms
        float decay = (float)Math.Pow(0.01f, 1.0f / (TIME / DELAY)); // converts to percentage for fade based on desired duration
        for (int i = 0; _provider.Volume > 0.01; i++)
        {
            _provider.Volume *= decay;
            await Task.Delay(DELAY);
        }
        _output.Stop(); _output.Dispose();
        _output = null; _provider = null;
    }
    void RequestFadeOut()
    {
        if (_output is null || _provider is null) { return; }
        _ = FadeOutAsync();
    }
    public async Task PlayAsync()
    {
        // using RawSourceWaveStream reader = new(new MemoryStream(, ));
        await FadeOutAsync();
        (float volume, byte[] trimmed) = await _rmsSetup;
        using RawSourceWaveStream wave = new(new MemoryStream(trimmed), new(44100, 16, 2));
        VolumeSampleProvider provider = new(wave.ToSampleProvider())
        { Volume = Math.Clamp(volume, 0f, 1f) };
        var (tcs, output) = AudioPlayer.SetupAudio(provider);
        _output = output;
        _provider = provider;
        await tcs.Task; 
    }
    public void SetupPlayback()
    {
        _ = _rmsSetup;
    }
}