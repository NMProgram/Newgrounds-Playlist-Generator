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
    void PrintDetails(IEnumerable<Composer> comps)
    {
        PrintDetails([..comps.DistinctBy(x => x.Name)], comp => $"\'{comp.Name}\' (Total Songs: {comp.Songs.Count})");
    }
    void Match()
    {
        string search = Validate("Enter a search term to filter by: ", InputLogic.IsNotEmpty);
        IEnumerable<Composer> composers = _cLogic.GetComposerMatches(search);
        PrintDetails(composers);
    }
    void BetweenNames()
    {
        string nameFirst = Validate("Enter the first name to search for: ", InputLogic.IsNotEmpty);
        string nameLast = Validate("Enter the last name to search for: ", InputLogic.IsNotEmpty);
        IEnumerable<Composer> composers = _cLogic.GetBetweenCompData(nameFirst, nameLast);
        PrintDetails(composers);
    }
    void BetweenDates()
    {
        DateTime start = Validate("Enter the starting Join Date: ", x => ValidString(x, InputLogic.IsValidDate));
        DateTime end = Validate("Enter the ending Join Date: ", x => ValidString(x, InputLogic.IsValidDate));
        IEnumerable<Composer> composers = _cLogic.GetBetweenCompData(start, end);
        PrintDetails(composers);
    }
    void BetweenAges()
    {
        long startAge = Validate("Enter the starting Age: ", x => ValidString(x, InputLogic.IsValidInteger));
        long endAge = Validate("Enter the ending Age: ", x => ValidString(x, InputLogic.IsValidInteger));
        IEnumerable<Composer> composers = _cLogic.GetBetweenCompData(startAge, endAge);
        PrintDetails(composers);
    }
    void Unavailable()
    {
        IEnumerable<Composer> composers = _cLogic.GetUnavailableComposers();
        PrintDetails(composers);
    }
    void WithSong<T>(Func<string, (bool, T, string?)> validator, Func<T, IEnumerable<Composer>> getter)
    {
        string type = typeof(T) == typeof(string) ? "Name" : "ID";
        T value = Validate($"Enter the {type} of the Song: ", validator);
        IEnumerable<Composer> composers = getter(value);
        PrintDetails(composers);
    }
    void WithSongID()
    {
        WithSong<long>(x => ValidString(x, InputLogic.IsValidInteger)!, _cLogic.GetBySongID);
    }
    void WithSongName()
    {
        WithSong(InputLogic.IsNotEmpty, _cLogic.GetBySongName);
    }
}