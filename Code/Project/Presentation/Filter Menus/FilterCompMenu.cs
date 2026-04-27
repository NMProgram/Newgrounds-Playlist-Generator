using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class FilterCompMenu : FilterMenu
{
    protected override string MenuStr => @"
    [1] Find Composers By Search Term
    [2] Composers Between Names
    [3] Composers Between Dates
    [4] Composers Between Ages
    [5] Unavailable Composers
    [6] Composers With Song ID
    [7] Composers With Song Name
    [Q] Return to Search Menu
    ";
    protected override Action GetAction(char inp) => inp switch
    {
        '1' => () => CheckActivity(Match),
        '2' => () => CheckActivity(BetweenNames),
        '3' => () => CheckActivity(BetweenDates),
        '4' => () => CheckActivity(BetweenAges),
        '5' => () => CheckActivity(Unavailable),
        '6' => () => CheckActivity(WithSongID),
        '7' => () => CheckActivity(WithSongName),
        _ => () => _active = false
    };
    static string Getter(Composer comp) => $"\'{comp.Name}\' (Total Songs: {comp.Songs.Count})";
    void Match()
    {
        FilterData("Enter a search term to filter by: ", "Search Term", GetString, _cLogic.GetComposerMatches, Getter);
    }
    void BetweenNames()
    {
        string start = "Enter the first name to search for: "; 
        string end = "Enter the last name to search for: ";
        FilterData(start, end, "Name", GetString, _cLogic.GetBetweenCompData, Getter);
    }
    void BetweenDates()
    {
        string start = "Enter the starting Join Date: ";
        string end = "Enter the ending Join Date: ";
        FilterData(start, end, "Join Date", GetDate, _cLogic.GetBetweenCompData, Getter, d => ((DateTime)d).FormatDate());
    }
    void BetweenAges()
    {
        string start = "Enter the starting Age: ";
        string end = "Enter the ending Age: ";
        FilterData(start, end, "Birth Year", GetInteger, _cLogic.GetBetweenCompData, Getter, a => ((long)a).AgeStr());
    }
    void Unavailable()
    {
        FilterData("", "", x => "", (_) => _cLogic.GetUnavailableComposers(), Getter);
    }
    void WithSongID()
    {
        FilterData("Enter the ID of the Song: ", "Song ID", GetInteger, _cLogic.GetBySongID, Getter);
    }
    void WithSongName()
    {
        FilterData("Enter the Name of the Song: ", "Song Name", GetString, _cLogic.GetBySongName, Getter);
    }
}