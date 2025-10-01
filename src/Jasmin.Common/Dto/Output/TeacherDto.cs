using Jasmin.Common.Enums;
using System.Text.Json.Serialization;

namespace Jasmin.Common.Dto.Output;

/// <summary>
/// Преподаватель
/// </summary>
public class TeacherDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Фамилия преподавателя
    /// </summary>
    [JsonPropertyName("surname")]
    public string Surname { get; set; }

    /// <summary>
    /// Имя преподавателя
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Отчество преподавателя
    /// </summary>
    [JsonPropertyName("patronymic")]
    public string Patronymic { get; set; }

    /// <summary>
    /// Логин для входа в ЛК
    /// </summary>
    [JsonPropertyName("login")]
    public string Login { get; set; }

    /// <summary>
    /// Пароль для входа в ЛК
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; }

    /// <summary>
    /// Активен ли преподаватель, числится ли все еще в штате
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    /// <summary>
    /// Должность
    /// </summary>
    [JsonPropertyName("post")]
    public PostType Post { get; set; }

    /// <summary>
    ///  Ставка
    /// </summary>
    [JsonPropertyName("rate")]
    public float Rate { get; set; }
}
