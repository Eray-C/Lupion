using System.ComponentModel.DataAnnotations;

namespace Lupion.Data.Enums;

/// <summary>
/// PlanlÄ± sevkiyat gÃ¼nleri. DeÄŸerler .NET DayOfWeek ile uyumlu (0 = Pazar, 6 = Cumartesi).
/// </summary>
public enum ScheduleDay
{
    [Display(Name = "Pazar")]
    Pazar = 0,

    [Display(Name = "Pazartesi")]
    Pazartesi = 1,

    [Display(Name = "SalÄ±")]
    Sali = 2,

    [Display(Name = "Ã‡arÅŸamba")]
    Carsamba = 3,

    [Display(Name = "PerÅŸembe")]
    Persembe = 4,

    [Display(Name = "Cuma")]
    Cuma = 5,

    [Display(Name = "Cumartesi")]
    Cumartesi = 6
}
