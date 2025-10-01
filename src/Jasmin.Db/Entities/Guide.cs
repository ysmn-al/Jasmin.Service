namespace Jasmin.Db.Entities;

public class Guide : Entity<long>
{
    /// <summary>
    /// Id учебного года
    /// </summary>
    public long YearId { get; set; }

    /// <summary>
    /// Id преподавателя
    /// </summary>
    public long TeacherId { get; set; }

    /// <summary>
    /// Количество бакалавров
    /// </summary>
    public long Bachelor { get; set; }

    /// <summary>
    /// Количество магистров 1 год
    /// </summary>
    public long MasterOne { get; set; }

    /// <summary>
    /// Количество магистров 2 год
    /// </summary>
    public long MasterTwo { get; set; }

    /// <summary>
    /// Количество аспирантов
    /// </summary>
    public long Postgraduate { get; set; }

    /// <summary>
    /// Количество НИРС
    /// </summary>
    public long NIRS { get; set; }

    /// <summary>
    /// Кафедра
    /// </summary>
    public long Department { get; set; }
}
