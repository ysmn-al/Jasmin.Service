using Jasmin.Common.Dto.Input;
using Jasmin.Common.Dto.Output;
using Jasmin.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Jasmin.Host.Controllers;

/// <summary>
/// Основной контроллер сервиса
/// </summary>
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("unit")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ApiController]
public class UnitController : ControllerBase
{
    private readonly ILogger<UnitController> _logger;
    private readonly IAppService _appService;

    /// <summary>
    /// Конструктор
    /// </summary>
    public UnitController(ILogger<UnitController> logger,
        IAppService appService)
    {
        _logger = logger;
        _appService = appService;
    }

    /// <summary>
    /// Удалить запись о группе
    /// </summary>
    [HttpPut("del")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Удалить запись о группе")]
    public async Task<IActionResult> DeleteUnit(string number)
    {
        var response = await _appService.DeleteUnit(number);

        return Ok(response);
    }

    /// <summary>
    /// Добавить запись о группе
    /// </summary>
    [HttpPut("add")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Добавить запись о группе")]
    public async Task<IActionResult> AddUnit(UnitInDto newOne)
    {
        var response = await _appService.AddUnit(newOne);

        return Ok(response);
    }
    /// <summary>
    /// Вывести список всех групп
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(UnitDto), StatusCodes.Status200OK)]
    [Description("Вывод списка всех групп")]
    public async Task<IActionResult> GetUnits()
    {
        var response = await _appService.GetUnits();

        return Ok(response);
    }
}
