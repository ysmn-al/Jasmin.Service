using System.Text.Json.Serialization;

namespace Jasmin.Common.Dto.Output;

public class GuideDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Id учебного года
    /// </summary>
    [JsonPropertyName("yearId")]
    public long YearId { get; set; }

    /// <summary>
    /// Id преподавателя
    /// </summary>
    [JsonPropertyName("teacherId")]
    public long TeacherId { get; set; }

    /// <summary>
    /// Количество бакалавров
    /// </summary>
    [JsonPropertyName("bachelor")]
    public long Bachelor { get; set; }

    /// <summary>
    /// Количество магистров 1 год
    /// </summary>
    [JsonPropertyName("masterOne")]
    public long MasterOne { get; set; }

    /// <summary>
    /// Количество магистров 2 год
    /// </summary>
    [JsonPropertyName("masterTwo")]
    public long MasterTwo { get; set; }

    /// <summary>
    /// Количество аспирантов
    /// </summary>
    [JsonPropertyName("postgraduate")]
    public long Postgraduate { get; set; }

    /// <summary>
    /// Количество НИРС
    /// </summary>
    [JsonPropertyName("nirs")]
    public long NIRS { get; set; }

    /// <summary>
    /// Кафедра
    /// </summary>
    [JsonPropertyName("department")]
    public long Department { get; set; }

}
