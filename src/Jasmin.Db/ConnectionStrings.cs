namespace Jasmin.Db;

/// <summary>
/// Настройка строки соединения
/// </summary>
public class ConnectionStrings
{
    /// <summary>
    /// Строка соединения
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Тайм-аут командной строки в секундах
    /// </summary>
    public int TimeOut { get; set; }
}
