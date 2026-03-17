using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Lupion.Data.Enums;

public static class ScheduleDayExtensions
{
    private static readonly Lazy<Dictionary<ScheduleDay, string>> DisplayNames = new(GetDisplayNames);

    public static string ToDisplayString(this ScheduleDay day)
    {
        return DisplayNames.Value.TryGetValue(day, out var name) ? name : day.ToString();
    }

    public static string ToDisplayString(this IEnumerable<ScheduleDay> days)
    {
        return days == null ? "" : string.Join(", ", days.OrderBy(d => d).Select(ToDisplayString));
    }

    /// <summary>VirgÃ¼lle ayrÄ±lmÄ±ÅŸ string'i enum listesine Ã§evirir. Ã–rn: "1,2,3" -> [Pazartesi, SalÄ±, Ã‡arÅŸamba]</summary>
    public static IReadOnlyList<ScheduleDay> ParseScheduleDays(string? scheduleDaysString)
    {
        if (string.IsNullOrWhiteSpace(scheduleDaysString)) return Array.Empty<ScheduleDay>();
        var list = new List<ScheduleDay>();
        foreach (var part in scheduleDaysString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (int.TryParse(part, out var value) && Enum.IsDefined(typeof(ScheduleDay), value))
                list.Add((ScheduleDay)value);
        }
        return list;
    }

    /// <summary>Enum listesini virgÃ¼lle ayrÄ±lmÄ±ÅŸ stringe Ã§evirir. Ã–rn: [Pazartesi, SalÄ±] -> "1,2"</summary>
    public static string ToScheduleDaysString(this IEnumerable<ScheduleDay> days)
    {
        if (days == null) return "";
        return string.Join(",", days.Select(d => (int)d).OrderBy(x => x));
    }

    /// <summary>HaftanÄ±n gÃ¼nÃ¼ (0-6) bu plana dahil mi?</summary>
    public static bool ContainsDay(string? scheduleDaysString, int dayOfWeek)
    {
        if (string.IsNullOrWhiteSpace(scheduleDaysString)) return false;
        var parts = scheduleDaysString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return parts.Any(p => p == dayOfWeek.ToString());
    }

    /// <summary>TÃ¼m gÃ¼nlerin display name'leri (sÄ±rayla).</summary>
    public static IReadOnlyList<(int Value, string Name)> GetAllDaysWithNames()
    {
        return Enum.GetValues<ScheduleDay>()
            .Select(d => ((int)d, d.ToDisplayString()))
            .OrderBy(x => x.Item1)
            .ToList();
    }

    private static Dictionary<ScheduleDay, string> GetDisplayNames()
    {
        var dict = new Dictionary<ScheduleDay, string>();
        foreach (ScheduleDay day in Enum.GetValues<ScheduleDay>())
        {
            var attr = typeof(ScheduleDay).GetField(day.ToString())?.GetCustomAttribute<DisplayAttribute>();
            dict[day] = attr?.Name ?? day.ToString();
        }
        return dict;
    }
}
