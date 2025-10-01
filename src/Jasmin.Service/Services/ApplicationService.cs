using Jasmin.Common.Dto.Input;
using Jasmin.Common.Dto.Output;
using Jasmin.Service.Clients.Jasmin.Host.Client;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jasmin.Service.Services;

/// <summary>
/// Сервис
/// </summary>
public class ApplicationService : IApplicationService
{
    private readonly IJasminClient _jasminClient;
    private readonly ILogger<ApplicationService> _logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    public ApplicationService(IJasminClient jasminClient, ILogger<ApplicationService> logger)
    {
        _jasminClient = jasminClient;
        _logger = logger;
    }

    #region Импорт данных
    public async Task UploadFile(byte[] fileBytes)
    {
        //убрали return, поскольку метод уже будет "возвращать" асинхронно выполнение следующей строки кода
        await _jasminClient.UploadFile(fileBytes);
    }

    #endregion

    #region Год

    /// <inheritdoc/>
    public async Task<List<YearDto>> GetYears()
    {
        var years = await _jasminClient.GetYears();
        return years;
    }

    #endregion

    #region Предмет

    /// <inheritdoc/>
    public async Task<List<SubjectDto>> GetSubjects()
    {
        var subjects = await _jasminClient.GetSubjects();
        return subjects;
    }

    #endregion

    #region Группа

    /// <inheritdoc/>
    public async Task<List<UnitDto>> GetUnits()
    {
        var units = await _jasminClient.GetUnits();
        return units;
    }

    #endregion

    #region Преподаватель

    public async Task<bool> UpdatePassword(PasswordInDto passwordDto)
    {
        return await _jasminClient.UpdatePassword(passwordDto);
    }

    public async Task<TeacherDto> GetTeacherByLogin(LoginDto model)
    {
        return await _jasminClient.GetTeacherByLogin(model);
    }

    public async Task<List<TeacherDto>> GetTeachers()
    {
        return await _jasminClient.GetTeachers();
    }

    public async Task<bool> AddTeacher(TeacherInDto teacher)
    {
        var teachers = await _jasminClient.AddTeacher(teacher);
        return true;
    }

    public async Task<bool> UpdateTeacher(TeacherDto updateTeacher)
    {
        var teachers = await _jasminClient.UpdateTeacher(updateTeacher);
        return true;
    }

    public async Task<bool> DeleteTeacher(long id)
    {

        return await _jasminClient.DeleteTeacher(id);
    }

    #endregion

    #region Плановая нагрузка

    /// <inheritdoc/>
    public async Task<List<PlannedLoadDto>> GetPlannedLoads()
    {
        return await _jasminClient.GetPlannedLoads();
    }

    public async Task<List<PlannedLoadDto>> GetPLforSelf(long yearId, int semesterId, long subjectId, string faculty)
    {
        return await _jasminClient.GetPLforSelf(yearId, semesterId, subjectId, faculty);
    }


    #endregion

    #region Актуальная нагрузка

    /// <inheritdoc/>
    public async Task<List<ActualLoadDto>> GetActualLoads()
    {
        return await _jasminClient.GetActualLoads();
    }

    /// <inheritdoc/>
    public async Task<List<ActualLoadDto>> GetActualLoadsByTeacherIdAndYearId(long teacherId, long yearId)
    {
        return await _jasminClient.GetActualLoadsByTeacherIdAndYearId(teacherId, yearId);
    }

    public async Task<bool> UpdateActualLoad(long id, ActualLoadDto updatedLoad)
    {
        return await _jasminClient.UpdateActualLoad(id, updatedLoad);
    }

    #endregion

    #region Руководство

    public async Task<bool> AddGuide(GuideInDto newOne)
    {
        var guides = await _jasminClient.AddGuide(newOne);
        return true;
    }

    public async Task<bool> DeleteGuide(long teacherId, long yearId)
    {
        return await _jasminClient.DeleteGuide(teacherId, yearId);
    }

    public async Task<List<GuideDto>> GetGuides()
    {
        return await _jasminClient.GetGuides();
    }

    #endregion

    #region Часы руководства

    public async Task<Dictionary<string, long>> CalculateTotalGuideHours(long teacherId, long yearId)
    {
        var hours = await _jasminClient.CalculateTotalGuideHours(teacherId, yearId);
        return hours;
    }

    public async Task<bool> AddGuideHour(GuideHourInDto newOne)
    {
        var guideHours = await _jasminClient.AddGuideHour(newOne);
        return true;
    }
    public async Task<bool> UpdateGuideHour(GuideHourDto updateGuideHour)
    {
        var guideHours = await _jasminClient.UpdateGuideHour(updateGuideHour);
        return true;
    }

    public async Task<bool> DeleteGuideHour(string type)
    {
        return await _jasminClient.DeleteGuideHour(type);
    }

    public async Task<List<GuideHourDto>> GetGuideHours()
    {
        return await _jasminClient.GetGuideHours();
    }

    #endregion
}
