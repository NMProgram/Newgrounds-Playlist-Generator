// used to avoid using paths with TagLib.File.Create
public class PathAbstraction : TagLib.File.IFileAbstraction
{
    public string Name { get; }
    public Stream ReadStream { get; }
    public Stream WriteStream { get; }
    public PathAbstraction(string name, Stream stream)
    {
        Name = name;
        ReadStream = stream;
        WriteStream = stream;
    }
    public void CloseStream(Stream stream)
    {
        stream.Dispose();
    }
}