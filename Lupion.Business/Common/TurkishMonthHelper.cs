namespace Empty_ERP_Template.Business.Common;

public static class TurkishMonthHelper
{
    private static readonly IReadOnlyDictionary<int, string> Names = new Dictionary<int, string>
    {
        { 1, "Ocak" }, { 2, "Şubat" }, { 3, "Mart" }, { 4, "Nisan" }, { 5, "Mayıs" },
        { 6, "Haziran" }, { 7, "Temmuz" }, { 8, "Ağustos" }, { 9, "Eylül" },
        { 10, "Ekim" }, { 11, "Kasım" }, { 12, "Aralık" }
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

