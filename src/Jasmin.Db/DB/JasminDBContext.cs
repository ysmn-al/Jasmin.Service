using Jasmin.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Jasmin.Db.DB;

/// <summary>
/// Контекст БД
/// </summary>
public class JasminDBContext : DbContext, IJasminDBContext
{
    private readonly string _connectionString;

    #region Конструкторы

    public JasminDBContext()
    {
    }

    public JasminDBContext(DbContextOptions<JasminDBContext> options, IConfiguration configuration) : base(options)
    {
        _connectionString = configuration.GetConnectionString(nameof(ConnectionStrings.ConnectionString));
    }

    public JasminDBContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    #endregion

    #region Таблицы

    /// <inheritdoc/>
    public virtual DbSet<ActualLoad> ActualLoads { get; set; }

    /// <inheritdoc/>
    public virtual DbSet<PlannedLoad> PlannedLoads { get; set; }

    /// <inheritdoc/>
    public virtual DbSet<Year> Years { get; set; }

    /// <inheritdoc/>
    public virtual DbSet<Teacher> Teachers { get; set; }

    /// <inheritdoc/>
    public virtual DbSet<Subject> Subjects { get; set; }

    /// <inheritdoc/>
    public virtual DbSet<Unit> Units { get; set; }

    /// <inheritdoc/>
    public virtual DbSet<Guide> Guides { get; set; }

    /// <inheritdoc/>
    public virtual DbSet<GuideHour> GuideHours { get; set; }

    #endregion

    #region Методы

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // MS SQL
        //optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=JasminServiceDB;Integrated Security=true;TrustServerCertificate=true").UseLazyLoadingProxies();
        //optionsBuilder.UseSqlServer(_connectionString).UseLazyLoadingProxies();

        // Postgres
        //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ServiceDB;Username=postgres;Password=New0101*").UseLazyLoadingProxies();
        optionsBuilder.UseNpgsql(_connectionString).UseLazyLoadingProxies();
    }
}
