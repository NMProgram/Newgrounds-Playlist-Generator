public class SongComposer
{
    public long SongID { get; private set; }
    public long ComposerID { get; private set; }
    public SongComposer(long songID, long composerID)
    {
        SongID = songID;
        ComposerID = composerID;
    }
}