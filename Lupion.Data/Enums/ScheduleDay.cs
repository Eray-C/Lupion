using System.ComponentModel.DataAnnotations;

namespace Lupion.Data.Enums;

/// <summary>
/// Planlı sevkiyat günleri. Değerler .NET DayOfWeek ile uyumlu (0 = Pazar, 6 = Cumartesi).
/// </summary>
public enum ScheduleDay
{
    [Display(Name = "Pazar")]
    Pazar = 0,

    [Display(Name = "Pazartesi")]
    Pazartesi = 1,

    [Display(Name = "Salı")]
    Sali = 2,

    [Display(Name = "Çarşamba")]
    Carsamba = 3,

    [Display(Name = "Perşembe")]
    Persembe = 4,

    [Display(Name = "Cuma")]
    Cuma = 5,

    [Display(Name = "Cumartesi")]
    Cumartesi = 6
}
