namespace Lupion.Business.Common;

public static class TurkishMonthHelper
{
    private static readonly IReadOnlyDictionary<int, string> Names = new Dictionary<int, string>
    {
        { 1, "Ocak" }, { 2, "Åubat" }, { 3, "Mart" }, { 4, "Nisan" }, { 5, "MayÄ±s" },
        { 6, "Haziran" }, { 7, "Temmuz" }, { 8, "AÄŸustos" }, { 9, "EylÃ¼l" },
        { 10, "Ekim" }, { 11, "KasÄ±m" }, { 12, "AralÄ±k" }
    };

    public static string GetName(int month)
    {
        return month >= 1 && month <= 12 ? Names[month] : string.Empty;
    }

    public static IReadOnlyList<(int Value, string Name)> GetAllMonths()
    {
        return Enumerable.Range(1, 12).Select(m => (m, GetName(m))).ToList();
    }
}

