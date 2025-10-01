using Jasmin.Common.Dto.Input;
using Jasmin.Common.Dto.Output;
using Jasmin.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Jasmin.Host.Controllers;

/// <summary>
/// Основной контроллер сервиса
/// </summary>
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("actualLoad")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ApiController]
public class ActualLoadController : ControllerBase
{
    private readonly ILogger<ActualLoadController> _logger;
    private readonly IAppService _appService;

    /// <summary>
    /// Конструктор
    /// </summary>
    public ActualLoadController(ILogger<ActualLoadController> logger,
        IAppService appService)
    {
        _logger = logger;
        _appService = appService;
    }

    /// <summary>
    /// Удалить запись об актуальной нагрузке
    /// </summary>
    [HttpPut("del")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Удалить запись об актуальной нагрузке")]
    public async Task<IActionResult> DeleteActualLoad(long delOne, long teacherId)
    {
        var response = await _appService.DeleteActualLoad(delOne, teacherId);

        return Ok(response);
    }

    /// <summary>
    /// Добавить запись об актуальной нагрузке
    /// </summary>
    [HttpPut("add")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Добавить запись об актуальной нагрузке")]
    public async Task<IActionResult> AddActualLoad(ActualLoadInDto newOne)
    {
        var response = await _appService.AddActualLoad(newOne);

        return Ok(response);
    }

    /// <summary>
    /// Вывести список всех актуальных нагрузок
    /// </summary>
    [HttpGet("byIds")]
    [ProducesResponseType(typeof(ActualLoadDto), StatusCodes.Status200OK)]
    [Description("Вывод списка всех актуальных нагрузок")]
    public async Task<IActionResult> GetActualLoadsByTeacherIdAndYearId(long teacherId, long yearId)
    {
        var response = await _appService.GetActualLoadsByTeacherIdAndYearId(teacherId, yearId);

        return Ok(response);
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(ActualLoadDto), StatusCodes.Status200OK)]
    [Description("Вывод списка всех актуальных нагрузок")]
    public async Task<IActionResult> GetActualLoads()
    {
        var response = await _appService.GetActualLoads();

        return Ok(response);
    }

    [HttpPost("update/{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Обновление записи актуальной нагрузки")]
    public async Task<IActionResult> UpdateActualLoad(long id, ActualLoadDto updatedLoad)
    {
        var response = await _appService.UpdateActualLoad(id, updatedLoad);

        return Ok(response);
    }

    /// <summary>
    /// Добавить запись об актуальной нагрузке
    /// </summary>
    [HttpPut("guide/add")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Добавить запись о руководстве")]
    public async Task<IActionResult> AddGuide(GuideInDto newOne)
    {
        var response = await _appService.AddGuide(newOne);

        return Ok(response);
    }

    [HttpGet("guide/all")]
    [ProducesResponseType(typeof(GuideDto), StatusCodes.Status200OK)]
    [Description("Вывод списка всех записей о руководстве")]
    public async Task<IActionResult> GetGuides()
    {
        var response = await _appService.GetGuides();

        return Ok(response);
    }

    [HttpPut("guide/del/{teacherId}/{yearId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Удалить запись о руководстве")]
    public async Task<IActionResult> DeleteGuide(long teacherId, long yearId)
    {
        var response = await _appService.DeleteGuide(teacherId, yearId);

        return Ok(response);
    }

    [HttpPut("guideHour/add")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Добавить запись о часах руководства")]
    public async Task<IActionResult> AddGuideHour(GuideHourInDto newOne)
    {
        var response = await _appService.AddGuideHour(newOne);

        return Ok(response);
    }

    [HttpGet("guideHour/all")]
    [ProducesResponseType(typeof(GuideHourDto), StatusCodes.Status200OK)]
    [Description("Вывод списка всех записей о часах руководстве")]
    public async Task<IActionResult> GetGuideHours()
    {
        var response = await _appService.GetGuideHours();

        return Ok(response);
    }

    [HttpPut("guideHour/del")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Удалить запись о часах руководства")]
    public async Task<IActionResult> DeleteGuideHour(string type)
    {
        var response = await _appService.DeleteGuideHour(type);

        return Ok(response);
    }

    [HttpGet("guideHour/sum/{teacherId}/{yearId}")]
    [ProducesResponseType(typeof(Dictionary<string, long>), StatusCodes.Status200OK)]
    [Description("Подсчет часов руководства для преподвателя")]
    public async Task<ActionResult<Dictionary<string, long>>> CalculateTotalGuideHours(long teacherId, long yearId)
    {
        // Логика получения данных о часах руководства по teacherId
        var guideHours = await _appService.CalculateTotalGuideHours(teacherId,yearId);

        return Ok(guideHours);
    }

    [HttpPut("guideHour/update")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Редактировать запись о преподавателе")]
    public async Task<IActionResult> UpdateGuideHour(GuideHourDto updateGuideHour)
    {
        var response = await _appService.UpdateGuideHour(updateGuideHour);

        return Ok(response);
    }
}
