using Jasmin.Common.Dto.Input;
using Jasmin.Common.Dto.Output;
using Jasmin.Common.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jasmin.Service.Clients.Jasmin.Host.Client;

/// <summary>
/// Клиент для доступа к сервису Jasmin.Host
/// </summary>
public class JasminClient : IJasminClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<JasminClient> _logger;
    private readonly ApplicationConfig _appConfig;

    /// <summary>
    /// Конструктор
    /// </summary>
    public JasminClient(HttpClient httpClient, ILogger<JasminClient> logger, IOptions<ApplicationConfig> appConfig)
    {
        _httpClient = httpClient;
        _logger = logger;
        _appConfig = appConfig.Value;
    }

    public async Task UploadFile(byte[] fileBytes)
    {
        using var byteArrayContent = new ByteArrayContent(fileBytes);
        // Установите более универсальный тип контента
        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        var response = await _httpClient.PostAsync("api/application/upload", byteArrayContent);
        response.EnsureSuccessStatusCode(); // Проверка успешного ответа
    }

    public async Task<bool> UpdatePassword(PasswordInDto passwordDto)
    {
        var json = JsonSerializer.Serialize(passwordDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("teacher/updatePwd", content);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return false;
        }
        return true;
    }

    /// <inheritdoc/>
    public async Task<List<YearDto>> GetYears()
    {
        var response = await _httpClient.GetAsync("year/all");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null;
        }

        var str = await response.Content.ReadAsStringAsync();
        var years = JsonSerializer.Deserialize<List<YearDto>>(str);
        return years;
    }

    /// <inheritdoc/>
    public async Task<List<ActualLoadDto>> GetActualLoads()
    {
        var response = await _httpClient.GetAsync("actualLoad/all");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null;
        }

        var str = await response.Content.ReadAsStringAsync();
        var actLoads = JsonSerializer.Deserialize<List<ActualLoadDto>>(str);
        return actLoads;
    }

    public async Task<bool> UpdateActualLoad(long id, ActualLoadDto updatedLoad)
    {
        var jsonContent = JsonSerializer.Serialize(updatedLoad);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"actualLoad/update/{id}", content);

        if (response.IsSuccessStatusCode)
        {
            return true; // Успешно обновлено
        }
        else
        {
            // Логирование ошибки
            var errorMessage = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Ошибка при обновлении нагрузки: {errorMessage}");
            return false; // Ошибка при обновлении
        }
    }

    public async Task<List<ActualLoadDto>> GetActualLoadsByTeacherIdAndYearId(long teacherId, long yearId)
    {
        // Формируем URL с параметрами
        var requestUri = $"actualLoad/byIds?teacherId={teacherId}&yearId={yearId}";

        // Выполняем GET-запрос
        var response = await _httpClient.GetAsync(requestUri);

        // Проверяем успешность ответа
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null;
        }

        // Читаем содержимое ответа
        var str = await response.Content.ReadAsStringAsync();

        // Десериализуем JSON в список ActualLoadDto
        var actLoads = JsonSerializer.Deserialize<List<ActualLoadDto>>(str, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Убедитесь в соответствии с нотацией именования
        });

        return actLoads;
    }

    /// <inheritdoc/>
    public async Task<List<PlannedLoadDto>> GetPlannedLoads()
    {
        var response = await _httpClient.GetAsync("plannedLoad/all");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null;
        }

        var str = await response.Content.ReadAsStringAsync();
        var plLoads = JsonSerializer.Deserialize<List<PlannedLoadDto>>(str);
        return plLoads;
    }

    public async Task<List<PlannedLoadDto>> GetPLforSelf(long yearId, int semesterId, long subjectId, string faculty)
    {
        var requestUrl = $"plannedLoad/forSelf?yearId={yearId}&semesterId={semesterId}&subjectId={subjectId}&faculty={faculty}";
        var response = await _httpClient.GetAsync(requestUrl);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null;
        }

        var str = await response.Content.ReadAsStringAsync();
        var plLoads = JsonSerializer.Deserialize<List<PlannedLoadDto>>(str);

        return plLoads;
    }

    /// <inheritdoc/>
    public async Task<List<TeacherDto>> GetTeachers()
    {
        var response = await _httpClient.GetAsync("teacher/all");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null;
        }

        var str = await response.Content.ReadAsStringAsync();
        var teachers = JsonSerializer.Deserialize<List<TeacherDto>>(str);
        return teachers;
    }

    public async Task<bool> AddTeacher(TeacherInDto teacher)
    {
        var json = JsonSerializer.Serialize(teacher);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("teacher/add", content);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateTeacher(TeacherDto updateTeacher)
    {
        var json = JsonSerializer.Serialize(updateTeacher);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("teacher/update", content);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateGuideHour(GuideHourDto updateGuideHour)
    {
        var json = JsonSerializer.Serialize(updateGuideHour);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("actualLoad/guideHour/update", content);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteTeacher(long id)
    {
        var response = await _httpClient.GetAsync($"teacher/del/{id}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return false;
        }
        var msg = await response.Content.ReadAsStringAsync();
        return true;
    }


    public async Task<TeacherDto> GetTeacherByLogin(LoginDto model)
    {
        var content = JsonContent.Create(model);

        var response = await _httpClient.PostAsync("teacher/byLogin", content);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null;
        }
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return null;

        var str = await response.Content.ReadAsStringAsync();
        var teacher = JsonSerializer.Deserialize<TeacherDto>(str);
        return teacher;
    }

    public async Task<List<SubjectDto>> GetSubjects()
    {
        var response = await _httpClient.GetAsync("subject/all");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null;
        }

        var str = await response.Content.ReadAsStringAsync();
        var subjects = JsonSerializer.Deserialize<List<SubjectDto>>(str);
        return subjects;
    }

    public async Task<List<UnitDto>> GetUnits()
    {
        var response = await _httpClient.GetAsync("unit/all");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null;
        }

        var str = await response.Content.ReadAsStringAsync();
        var units = JsonSerializer.Deserialize<List<UnitDto>>(str);
        return units;
    }

    #region Руководство

    public async Task<bool> AddGuide(GuideInDto newOne)
    {
        var json = JsonSerializer.Serialize(newOne);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("actualLoad/guide/add", content);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return false;
        }
        return true;
    }

    public async Task<bool> DeleteGuide(long teacherId, long yearId)
    {
        var response = await _httpClient.GetAsync($"actualLoad/guide/del/{teacherId}/{yearId}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return false;
        }
        var msg = await response.Content.ReadAsStringAsync();
        return true;
    }

    public async Task<List<GuideDto>> GetGuides()
    {
        var response = await _httpClient.GetAsync("actualLoad/guide/all");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null;
        }

        var str = await response.Content.ReadAsStringAsync();
        var guides = JsonSerializer.Deserialize<List<GuideDto>>(str);
        return guides;
    }

    #endregion

    #region Часы руководства

    public async Task<Dictionary<string, long>> CalculateTotalGuideHours(long teacherId, long yearId)
    {
        var response = await _httpClient.GetAsync($"actualLoad/guideHour/sum/{teacherId}/{yearId}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null; // Или выбросьте исключение, если это более уместно
        }

        // Читаем содержимое ответа
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Десериализуем JSON в Dictionary
        var result = JsonSerializer.Deserialize<Dictionary<string, long>>(jsonResponse);

        return result;
    }

    public async Task<bool> AddGuideHour(GuideHourInDto newOne)
    {
        var json = JsonSerializer.Serialize(newOne);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync("actualLoad/guideHour/add", content);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return false;
        }
        return true;
    }

    public async Task<bool> DeleteGuideHour(string type)
    {
        var response = await _httpClient.GetAsync($"actualLoad/guideHour/del/{type}");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return false;
        }
        var msg = await response.Content.ReadAsStringAsync();
        return true;
    }

    public async Task<List<GuideHourDto>> GetGuideHours()
    {
        var response = await _httpClient.GetAsync("actualLoad/guideHour/all");
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Ошибка при обращении к сервису Jasmin.Host");
            return null;
        }

        var str = await response.Content.ReadAsStringAsync();
        var guideHours = JsonSerializer.Deserialize<List<GuideHourDto>>(str);
        return guideHours;
    }


    #endregion
}
