using Jasmin.Common.Dto.Input;
using Jasmin.Common.Dto.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jasmin.Common.Services;

/// <summary>
/// Интерфейс основного сервиса
/// </summary>
public interface IAppService
{
    #region Файл

    /// <summary>
    /// Загрузить файл
    /// </summary>
    Task UploadFile(byte[] fileBytes);

    #endregion

    #region Учебные года

    /// <summary>
    /// Вывести список учебных годов
    /// </summary>
    Task<List<YearDto>> GetYears();

    /// <summary>
    /// Добавить учебный год
    /// </summary>
    Task<bool> AddYear(YearInDto newOne);

    #endregion

    #region Группы

    /// <summary>
    /// Добавить группу
    /// </summary>
    Task<bool> AddUnit(UnitInDto newOne);

    /// <summary>
    /// Удалить группу
    /// </summary>
    Task<bool> DeleteUnit(string number);

    /// <summary>
    /// Вывести весь список групп
    /// </summary>
    Task<List<UnitDto>> GetUnits();

    #endregion

    #region Предметы

    /// <summary>
    /// Добавить предмет
    /// </summary>
    Task<bool> AddSubject(SubjectInDto newOne);

    /// <summary>
    /// Удалить предмет
    /// </summary>
    Task<bool> DeleteSubject(SubjectInDto newOne);

    /// <summary>
    /// Вывести весь список предметов
    /// </summary>
    Task<List<SubjectDto>> GetSubjects();

    #endregion

    #region Преподаватели

    /// <summary>
    /// Обновить пароль
    /// </summary>
    Task<bool> UpdatePassword(PasswordInDto passwordDto);

    /// <summary>
    /// Изменить запись о преподавателе
    /// </summary>
    Task<bool> UpdateTeacher(TeacherDto updateTeacher);

    /// <summary>
    /// Добавить преподавателя
    /// </summary>
    Task<bool> AddTeacher(TeacherInDto person);

    /// <summary>
    /// Удалить преподавателя
    /// </summary>
    Task<bool> DeleteTeacher(long id);

    /// <summary>
    /// Вывести весь список преподавателей
    /// </summary>
    Task<List<TeacherDto>> GetTeachers();

    /// <summary>
    /// Получить преподавателя по логину
    /// </summary>
    Task<TeacherDto> GetTeacherByLogin(LoginDto model);

    #endregion

    #region Акутальная нагрузка

    /// <summary>
    /// Добавить актуальную нагрузку
    /// </summary>
    Task<bool> AddActualLoad(ActualLoadInDto newOne);

    /// <summary>
    /// Удалить актуальную нагрузку
    /// </summary>
    Task<bool> DeleteActualLoad(long delOne, long teacherId);

    /// <summary>
    /// Изменить запись об актуальной нагрузке
    /// </summary>
    Task<bool> UpdateActualLoad(long id, ActualLoadDto updatedLoad);

    /// <summary>
    /// Вывести все актуальные нагрузки
    /// </summary>
    Task<List<ActualLoadDto>> GetActualLoads();

    /// <summary>
    /// Получить запись об актуальной нагрузке преподавателя
    /// </summary>
    Task<List<ActualLoadDto>> GetActualLoadsByTeacherIdAndYearId(long teacherId, long yearId);

    #endregion

    #region Плановая нагрузка

    /// <summary>
    /// Добавить плановую нагрузку
    /// </summary>
    Task<bool> AddPlannedLoad(PlannedLoadInDto newOne);

    /// <summary>
    /// Удалить плановую нагрузку
    /// </summary>
    Task<bool> DeletePlannedLoad(long yearId, int semester, long subjectId, long unitId);

    /// <summary>
    /// Вывести все плановые нагрузки
    /// </summary>
    Task<List<PlannedLoadDto>> GetPlannedLoads();

    /// <summary>
    /// Получить запись о плановой нагрузке по году, семестру, предмету и факультету
    /// </summary>
    Task<List<PlannedLoadDto>> GetPLforSelf(long yearId, int semesterId, long subjectId, string faculty);

    #endregion

    #region Руководство

    /// <summary>
    /// Добавить запись о руководстве
    /// </summary>
    Task<bool> AddGuide(GuideInDto newOne);

    /// <summary>
    /// Удалить запись о руководстве
    /// </summary>
    Task<bool> DeleteGuide(long teacherId, long yearId);

    Task<bool> UpdateGuideHour(GuideHourDto updateGuideHour);

    /// <summary>
    /// Вывести весь список записей о руководстве
    /// </summary>
    Task<List<GuideDto>> GetGuides();

    /// <summary>
    /// Добавить запись о часах руководства
    /// </summary>
    Task<bool> AddGuideHour(GuideHourInDto newOne);

    /// <summary>
    /// Удалить запись о часах руководства
    /// </summary>
    Task<bool> DeleteGuideHour(string type);

    /// <summary>
    /// Вывести весь список записей о часах руководства
    /// </summary>
    Task<List<GuideHourDto>> GetGuideHours();

    Task<Dictionary<string, long>> CalculateTotalGuideHours(long teacherId, long yearId);

    #endregion
}
