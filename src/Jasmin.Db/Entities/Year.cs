namespace Jasmin.Db.Entities;

public class Year : Entity<long>
{
    /// <summary>
    /// Учебный год
    /// </summary>
    public string AcademicYear { get; set; }
}
