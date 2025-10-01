namespace Jasmin.Db.Entities;

public class Unit : Entity<long>
{
    /// <summary>
    /// Номер потока
    /// </summary>
    public string Faculty { get; set; }

    /// <summary>
    /// Номер группы
    /// </summary>
    public string Number{ get; set; }
}
