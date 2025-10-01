namespace Jasmin.Db.Entities;

/// <summary>
/// Класс для определения сущности
/// </summary>
/// <typeparam name="TKey">Тип ключа</typeparam>
public abstract class Entity<TKey>
{
    /// <summary>
    /// Ключ
    /// </summary>
    public TKey Id { get; set; }
}
