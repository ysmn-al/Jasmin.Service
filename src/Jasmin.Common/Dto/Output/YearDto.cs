using System.Text.Json.Serialization;

namespace Jasmin.Common.Dto.Output;

public class YearDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Учебный год
    /// </summary>
    [JsonPropertyName("academicYear")]
    public string AcademicYear { get; set; }
}
