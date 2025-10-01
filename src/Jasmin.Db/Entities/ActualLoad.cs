namespace Jasmin.Db.Entities;

public class ActualLoad : Entity<long>
{
    /// <summary>
    /// Id плановой нагрузки
    /// </summary>
    public long PlannedLoadId { get; set; }

    /// <summary>
    /// Id преподавателя
    /// </summary>
    public long TeacherId { get; set; }

    //public long YearId { get; set; }

    /// <summary>
    /// Часы лекции
    /// </summary>
    public long Lecture { get; set; }

    /// <summary>
    /// Часы ПЗ
    /// </summary>
    public long Lesson { get; set; }

    /// <summary>
    /// Часы ЛР
    /// </summary>
    public long Labwork { get; set; }

    /// <summary>
    /// Часы КР
    /// </summary>
    public long Coursework { get; set; }

    /// <summary>
    /// Часы КП
    /// </summary>
    public long CourseProject { get; set; }

    /// <summary>
    /// Часы консультаций
    /// </summary>
    public long Сonsultation { get; set; }

    /// <summary>
    /// Часы экзамена
    /// </summary>
    public long Exam { get; set; }

    /// <summary>
    /// Часы рейтинга
    /// </summary>
    public long Rating { get; set; }

    /// <summary>
    /// Часы зачета
    /// </summary>
    public long Credit { get; set; }

    /// <summary>
    /// Часы практики
    /// </summary>
    public long Practice { get; set; }

}
