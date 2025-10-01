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
[Route("subject")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly ILogger<SubjectController> _logger;
    private readonly IAppService _appService;

    /// <summary>
    /// Конструктор
    /// </summary>
    public SubjectController(ILogger<SubjectController> logger,
        IAppService appService)
    {
        _logger = logger;
        _appService = appService;
    }

    /// <summary>
    /// Удалить запись о предмете
    /// </summary>
    [HttpPut("del")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Удалить запись о предмете")]
    public async Task<IActionResult> DeleteSubject(SubjectInDto newOne)
    {
        var response = await _appService.DeleteSubject(newOne);

        return Ok(response);
    }

    /// <summary>
    /// Добавить запись о предмете
    /// </summary>
    [HttpPut("add")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Добавить запись о предмете")]
    public async Task<IActionResult> AddSubject(SubjectInDto newOne)
    {
        var response = await _appService.AddSubject(newOne);

        return Ok(response);
    }
    /// <summary>
    /// Вывести список всех предметов
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(SubjectDto), StatusCodes.Status200OK)]
    [Description("Вывод списка всех предметов")]
    public async Task<IActionResult> GetSubjects()
    {
        var response = await _appService.GetSubjects();

        return Ok(response);
    }
}
