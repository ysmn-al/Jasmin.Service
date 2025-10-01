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
[Route("year")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ApiController]
public class YearController : ControllerBase
{
    private readonly ILogger<UnitController> _logger;
    private readonly IAppService _appService;

    /// <summary>
    /// Конструктор
    /// </summary>
    public YearController(ILogger<UnitController> logger,
        IAppService appService)
    {
        _logger = logger;
        _appService = appService;
    }

    /// <summary>
    /// Вывести список всех групп
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(YearDto), StatusCodes.Status200OK)]
    [Description("Вывод списка всех учебных годов")]
    public async Task<IActionResult> GetYears()
    {
        var response = await _appService.GetYears();

        return Ok(response);
    }

    /// <summary>
    /// Вывести список всех групп
    /// </summary>
    [HttpPut("add")]
    [ProducesResponseType(typeof(YearDto), StatusCodes.Status200OK)]
    [Description("Вывод списка всех учебных годов")]
    public async Task<IActionResult> AddYears(YearInDto newOne)
    {
        var response = await _appService.AddYear(newOne);

        return Ok(response);
    }

}
