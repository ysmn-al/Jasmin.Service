using Microsoft.EntityFrameworkCore;

namespace Jasmin.Db.DB;

public class JasminDBMigration
{
    private readonly string _connectionString;

    /// <summary>
    /// Конструктор
    /// </summary>
    public JasminDBMigration(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Применить изменения
    /// </summary>
    public void ApplyMigrations()
    {
        using (var context = new JasminDBContext(_connectionString))
        {
            context.Database.Migrate();
        }
    }

}
