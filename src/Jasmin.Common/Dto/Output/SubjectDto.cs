using System.Text.Json.Serialization;

namespace Jasmin.Common.Dto.Output;

/// <summary>
/// Предмет
/// </summary>
public class SubjectDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Наименование предмета
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Учебный год начала существования предмета
    /// </summary>
    [JsonPropertyName("beginYearId")]
    public string BeginYearId { get; set; }

    /// <summary>
    /// Учебный год окончания существования предмета
    /// </summary>
    [JsonPropertyName("endYearId")]
    public string EndYearId { get; set; }
}
