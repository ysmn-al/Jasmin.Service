namespace Jasmin.Common.Dto.Input;

public class GuideHourInDto
{
    /// <summary>
    /// Тип руководства
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Часы за семестр
    /// </summary>
    public long SemesterHours { get; set; }

    /// <summary>
    /// Часы за защиту
    /// </summary>
    public long DefenseHours { get; set; }
}
