using Jasmin.Common.Dto.Input;
using Jasmin.Common.Dto.Output;
using Jasmin.Common.Services;
using Jasmin.Db.Entities;
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
[Route("plannedLoad")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ApiController]
public class PlannedLoadController : ControllerBase
{
    private readonly ILogger<PlannedLoadController> _logger;
    private readonly IAppService _appService;

    /// <summary>
    /// Конструктор
    /// </summary>
    public PlannedLoadController(ILogger<PlannedLoadController> logger,
        IAppService appService)
    {
        _logger = logger;
        _appService = appService;
    }

    /// <summary>
    /// Удалить запись о плановой нагрузке
    /// </summary>
    [HttpPut("del")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Удалить запись о плановой нагрузке")]
    public async Task<IActionResult> DeletePlannedLoad(long yearId, int semester, long subjectId, long unitId)
    {
        var response = await _appService.DeletePlannedLoad(yearId, semester, subjectId, unitId);

        return Ok(response);
    }

    /// <summary>
    /// Добавить запись о плановой нагрузке
    /// </summary>
    [HttpPut("add")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Добавить запись о плановой нагрузке")]
    public async Task<IActionResult> AddPlannedLoad(PlannedLoadInDto newOne)
    {
        var response = await _appService.AddPlannedLoad(newOne);

        return Ok(response);
    }

    /// <summary>
    /// Вывести список всех плановых нагрузок
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(PlannedLoadDto), StatusCodes.Status200OK)]
    [Description("Вывод списка всех плановых нагрузок")]
    public async Task<IActionResult> GetPlannedLoads()
    {
        var response = await _appService.GetPlannedLoads();

        return Ok(response);
    }

    [HttpGet("forSelf")]
    [ProducesResponseType(typeof(List<PlannedLoadDto>), StatusCodes.Status200OK)]
    [Description("Вывод списка всех плановых нагрузок")]
    public async Task<IActionResult> GetPLforSelf(long yearId, int semesterId, long subjectId, string faculty)
    {
        var response = await _appService.GetPLforSelf(yearId, semesterId, subjectId, faculty);
        return Ok(response);
    }
}
