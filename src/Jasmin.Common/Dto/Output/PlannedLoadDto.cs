using Jasmin.Common.Enums;
using System.Text.Json.Serialization;
namespace Jasmin.Common.Dto.Output
{
    public class PlannedLoadDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Id предмета
        /// </summary>
        [JsonPropertyName("subjectId")]
        public long SubjectId { get; set; }

        [JsonPropertyName("subjectName")]
        public string SubjectName { get; set; } 

        /// <summary>
        /// Id учебного года
        /// </summary>
        [JsonPropertyName("yearId")]
        public long YearId { get; set; }

        /// <summary>
        /// Семестр
        /// </summary>
        [JsonPropertyName("semester")]
        public SemesterType Semester { get; set; }

        /// <summary>
        /// Id группы
        /// </summary>
        [JsonPropertyName("unitId")]
        public long UnitId { get; set; }

        [JsonPropertyName("unitName")]
        public string UnitName { get; set; }

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
        public long Сonsultation { get; set; }

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
}
