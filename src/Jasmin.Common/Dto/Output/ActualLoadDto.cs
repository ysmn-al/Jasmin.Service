using System.Text.Json.Serialization;

namespace Jasmin.Common.Dto.Output;

public class ActualLoadDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Id плановой нагрузки
    /// </summary>
    [JsonPropertyName("plannedLoadId")]
    public long PlannedLoadId { get; set; }

    /// <summary>
    /// Id преподавателя
    /// </summary>
    [JsonPropertyName("teacherId")]
    public long TeacherId { get; set; }

    [JsonPropertyName("teacherName")]
    public string TeacherName { get; set; }

    [JsonPropertyName("unitNunber")]
    public string UnitNumber { get; set; }

    ///// <summary>
    ///// Id учебного года
    ///// </summary>
    //[JsonPropertyName("yearId")]
    //public long YearId { get; set; }
    [JsonPropertyName("subjectName")]
    public string SubjectName { get; set; }

    [JsonPropertyName("semecter")]
    public int Semester { get; set; }

    /// <summary>
    /// Часы лекции
    /// </summary>
    [JsonPropertyName("lecture")]
    public long Lecture { get; set; }

    /// <summary>
    /// Часы ПЗ
    /// </summary>
    [JsonPropertyName("lesson")]
    public long Lesson { get; set; }

    /// <summary>
    /// Часы ЛР
    /// </summary>
    [JsonPropertyName("labwork")]
    public long Labwork { get; set; }

    /// <summary>
    /// Часы КР
    /// </summary>
    [JsonPropertyName("coursework")]
    public long Coursework { get; set; }

    /// <summary>
    /// Часы КП
    /// </summary>
    [JsonPropertyName("courseProject")]
    public long CourseProject { get; set; }

    /// <summary>
    /// Часы консультаций
    /// </summary>
    [JsonPropertyName("consultation")]
    public long Consultation { get; set; }

    /// <summary>
    /// Часы экзамена
    /// </summary>
    [JsonPropertyName("exam")]
    public long Exam { get; set; }

    /// <summary>
    /// Часы рейтинга
    /// </summary>
    [JsonPropertyName("rating")]
    public long Rating { get; set; }

    /// <summary>
    /// Часы зачета
    /// </summary>
    [JsonPropertyName("credit")]
    public long Credit { get; set; }

    /// <summary>
    /// Часы практики
    /// </summary>
    [JsonPropertyName("practice")]
    public long Practice { get; set; }
}
