namespace Jasmin.Common.Dto.Input;

/// <summary>
/// Предмет
/// </summary>
public class SubjectInDto
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
