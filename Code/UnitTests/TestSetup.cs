using Dapper;
using Microsoft.Data.Sqlite;
using SQLitePCL;

public class TestStartup
{
    protected static IConnection Factory = new InMemory();
    protected readonly static SongLogic _sAccess = new(Factory);
    protected readonly static ComposerLogic _cAccess = new(Factory);
    protected readonly static SongComposerAccess _scAccess = new(Factory);
    static SqliteConnection Conn = Factory.Connect();
    static void SetupSongs()
    {
        string songSQL = @"INSERT INTO Song 
        VALUES (@ID, @Name, @ReleaseDate, @Genre, @LevelID, @Available, @Audio)";
        Song[] songs = [
            new(311087, "Tiny Tunes", "2010-02-12 00:00:00", 15, 65943, 1, []),
            new(386900, "Silent Hill (Dubstep)", "2010-12-28 00:00:00", 8, 99719, 1, []),
            new(586990, "Aztech + Lockyn: Soulwind", "2014-09-07 00:00:00", 8, 148508, 1, []),
            new(598682, "circles kdrew", "2014-12-24 00:00:00", 8, 188711, 0, []),
            new(643474, "EnV - Ginseng", "2015-09-10 00:00:00", 6, 212295, 1, []),
            new(478433, "-Pixelspace- SIK2 OST", "2012-03-28 00:00:00", 15, 660967, 1, []),
            new(479319, "-Electroman adventures-", "2012-04-04 00:00:00", 15, 731493, 1, []),
        ];
        foreach (var song in songs)
        { Conn.Execute(songSQL, song); }
    }
    static TestStartup()
    {
        SongAccess sAccess = new(Factory);
        ComposerAccess cAccess = new(Factory);
        SongComposerAccess scAccess = new(Factory);
        // Arrange
        string compSQL = @"INSERT INTO Composer 
        VALUES (@ID, @Name, @JoinDate, @BirthYear, @Description, @OnNewgrounds)";
        string scSQL = @"INSERT INTO SongComposer VALUES (@SongID, @ComposerID)";

        Composer[] composers = [
            new(1, "Fantomenk", "2008-10-26 00:00:00", -1, "", 1),
            new(2, "Aydin-Jewelz123", "2007-04-20 00:00:00", 1992, "Back in the flow of things!", 1),
            new(3, "kiynaria", "2012-09-11 00:00:00", 1996, "I use FL Studio. Feel free to PM me about anything at all. All music posted here is free to use for NON-COMMERCIAL PURPOSES, just give me credit and we're all good. (Anything else must be discussed.) Thanks for stopping by!", 1),
            new(4, "Kreyowitz", "2003-04-12 00:00:00", 1990, "A FAT BROWN DUDE WHO MAKES MUSIC.", 1),
            new(5, "KDrew", "2010-07-21 00:00:00", 1989, "KDrew music and videos", 0),
            new(6, "Helpegasus", "2014-12-22 00:00:00", 2003, "", 1),
            new(7, "Envy", "2003-08-07 00:00:00", 1991, "Fruity loops (Duh), Skiing, Snapple!", 1),
            new(8, "Waterflame", "2003-04-04 00:00:00", 1989, "I live in a human box and I make human music. Want me to make music or sound effects for your project? Check out availability and conditions here: http://www.waterflame.com", 1),
            new(9, "Waterflame", "2003-04-04 00:00:00", 1989, "I live in a human box and I make human music. Want me to make music or sound effects for your project? Check out availability and conditions here: http://www.waterflame.com", 1),
        ];
        SongComposer[] scValues = [
            new(311087, 1),
            new(386900, 2),
            new(586990, 3),
            new(586990, 4),
            new(598682, 5),
            new(598682, 6),
            new(643474, 7),
            new(478433, 8),
            new(479319, 9),
        ];

        // Act
        SetupSongs();

        for (int i = 0; i < composers.Length; i++)
        {
            Conn.Execute(compSQL, composers[i]);
            Conn.Execute(scSQL, scValues[i]);
        }
    }
}