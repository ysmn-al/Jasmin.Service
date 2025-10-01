using Jasmin.Common.Dto.Input;
using Jasmin.Common.Dto.Output;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Jasmin.Service.Services;

/// <summary>
/// Основной сервис
/// </summary>
public interface IApplicationService
{
    Task UploadFile(byte[] fileBytes);
    Task<List<PlannedLoadDto>> GetPLforSelf(long yearId, int semesterId, long subjectId, string faculty);
    Task<bool> UpdatePassword(PasswordInDto passwordDto);

    Task<bool> UpdateActualLoad(long id, ActualLoadDto updatedLoad);
    Task<List<ActualLoadDto>> GetActualLoadsByTeacherIdAndYearId(long teacherId, long yearId);

    Task<bool> UpdateGuideHour(GuideHourDto updateGuideHour);

    /// <summary>
    /// Получить список учебных годов
    /// </summary>
    Task<List<YearDto>> GetYears();

    /// <summary>
    /// Получить список актуальных нагрузок
    /// </summary>
    Task<List<ActualLoadDto>> GetActualLoads();

    /// <summary>
    /// Получить список плановых нагрузок
    /// </summary>
    Task<List<PlannedLoadDto>> GetPlannedLoads();

    Task<bool> AddTeacher(TeacherInDto teacher);

    Task<bool> UpdateTeacher(TeacherDto updateTeacher);

    Task<List<SubjectDto>> GetSubjects();

    Task<bool> DeleteTeacher(long id);

    Task<List<TeacherDto>> GetTeachers();

    Task<TeacherDto> GetTeacherByLogin(LoginDto model);

    Task<List<UnitDto>> GetUnits();

    Task<bool> AddGuide(GuideInDto newOne);

    /// <summary>
    /// Удалить запись о руководстве
    /// </summary>
    Task<bool> DeleteGuide(long teacherId, long yearId);

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
}
