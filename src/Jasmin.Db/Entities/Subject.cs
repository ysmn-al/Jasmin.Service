namespace Jasmin.Db.Entities;

public class Subject : Entity<long>
{
    /// <summary>
    /// Наименование предмета
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Учебный год начала существования предмета
    /// </summary>
    public string BeginYearId { get; set; }

    /// <summary>
    /// Учебный год окончания существования предмета
    /// </summary>
    public string EndYearId { get; set; }
}
