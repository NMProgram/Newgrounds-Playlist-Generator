// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography.X509Certificates;

public static class Program
{
    static void Main()
    {
        // Console.WriteLine(InputLogic.IsValidDate("Mar 10, 1800", 1900));
        // new UpdateCompMenu().UpdateSong();
        // AlterSongMenu menu = new();
        // AccessLogic access = new(new InFile());
        // SongAccess sAccess = new(new InFile());
        
        // Console.WriteLine(sAccess.GetByID(2));
        // Console.ReadLine();
        Console.Clear();
        new MainMenu().Start();
    }
}
