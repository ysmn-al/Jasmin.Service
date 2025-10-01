using Jasmin.Common.Enums;

namespace Jasmin.Common.Dto.Input;

/// <summary>
/// Преподаватель
/// </summary>
public class TeacherInDto
{
    public long Id { get; set; }
    /// <summary>
    /// Фамилия преподавателя
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    /// Имя преподавателя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Отчество преподавателя
    /// </summary>
    public string Patronymic { get; set; }

    /// <summary>
    /// Логин для входа в ЛК
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Пароль для входа в ЛК
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Активен ли преподаватель, числится ли все еще в штате
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Должность
    /// </summary>
    public PostType Post { get; set; }

    /// <summary>
    ///  Ставка
    /// </summary>
    public float Rate { get; set; }
}
