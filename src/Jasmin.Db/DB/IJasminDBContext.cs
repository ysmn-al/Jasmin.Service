using Jasmin.Db.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Jasmin.Db.DB;

/// <summary>
/// Интерфейс контекста БД
/// </summary>
public interface IJasminDBContext
{
    #region Таблицы

    /// <summary>
    /// Актуальная нагрузка
    /// </summary>
    DbSet<ActualLoad> ActualLoads { get; }

    /// <summary>
    /// Плановая нагрузка
    /// </summary>
    DbSet<PlannedLoad> PlannedLoads { get; }

    /// <summary>
    /// Аэропорты
    /// </summary>
    DbSet<Year> Years { get; }

    /// <summary>
    /// Преподаватели
    /// </summary>
    DbSet<Teacher> Teachers { get; }

    /// <summary>
    /// Предметы
    /// </summary>
    DbSet<Subject> Subjects { get; }

    /// <summary>
    /// Группы
    /// </summary>
    DbSet<Unit> Units { get; }

    /// <summary>
    /// Руководство
    /// </summary>
    DbSet<Guide> Guides { get; }

    /// <summary>
    /// Часы для разных типов руководства
    /// </summary>
    DbSet<GuideHour> GuideHours { get; }

    #endregion

    #region Методы

    /// <summary>
    /// Сохранить изменения
    /// </summary>
    Task<int> SaveChangesAsync();

    #endregion
}
