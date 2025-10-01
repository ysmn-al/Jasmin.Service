namespace Jasmin.Db.Entities;

public class GuideHour : Entity<long>
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
