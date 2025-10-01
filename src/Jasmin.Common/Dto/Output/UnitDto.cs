using System.Text.Json.Serialization;

namespace Jasmin.Common.Dto.Output;

/// <summary>
/// Группа
/// </summary>
public class UnitDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Номер потока
    /// </summary>
    [JsonPropertyName("faculty")]
    public string Faculty { get; set; }

    /// <summary>
    /// Номер группы
    /// </summary>
    [JsonPropertyName("number")]
    public string Number { get; set; }

}
