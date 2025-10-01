using Jasmin.Common.Dto.Input;
using Jasmin.Common.Dto.Output;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Jasmin.Service.Clients.Jasmin.Host.Client
{
    /// <summary>
    /// Интерфейс для клиента Jasmin.Host
    /// </summary>
    public interface IJasminClient
    {
        Task UploadFile(byte[] fileBytes);
        Task<List<PlannedLoadDto>> GetPLforSelf(long yearId, int semesterId, long subjectId, string faculty);
        Task<bool> UpdatePassword(PasswordInDto passwordDto);

        Task<List<ActualLoadDto>> GetActualLoadsByTeacherIdAndYearId(long teacherId, long yearId);

        Task<bool> UpdateActualLoad(long id, ActualLoadDto updatedLoad);

        Task<bool> UpdateGuideHour(GuideHourDto updateGuideHour);

        /// <summary>
        /// Получить список учебных годов
        /// </summary>
        Task<List<YearDto>> GetYears();

        /// <summary>
        /// Получить список всех актуальных нагрузок
        /// </summary>
        Task<List<ActualLoadDto>> GetActualLoads();

        /// <summary>
        /// Получить список всех плановых нагрузок
        /// </summary>
        Task<List<PlannedLoadDto>> GetPlannedLoads();

        /// <summary>
        /// Получить список учителей
        /// </summary>
        Task<List<TeacherDto>> GetTeachers();

        Task<bool> AddTeacher(TeacherInDto teacher);

        Task<bool> UpdateTeacher(TeacherDto updateTeacher);

        Task<bool> DeleteTeacher(long id);

        Task<TeacherDto> GetTeacherByLogin(LoginDto model);

        Task<List<SubjectDto>> GetSubjects();

        Task<List<UnitDto>> GetUnits();

        #region Руководство

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
        #endregion
    }
}
