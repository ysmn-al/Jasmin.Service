using System.Text.Json.Serialization;

namespace Jasmin.Common.Dto.Output;

public class GuideHourDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Тип руководства
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// Часы за семестр
    /// </summary>
    [JsonPropertyName("semesterHours")]
    public long SemesterHours { get; set; }

    /// <summary>
    /// Часы за защиту
    /// </summary>
    [JsonPropertyName("defenseHours")]
    public long DefenseHours { get; set; }
}
