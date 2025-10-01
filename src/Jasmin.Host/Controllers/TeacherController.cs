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
[Route("teacher")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly ILogger<TeacherController> _logger;
    private readonly IAppService _appService;

    /// <summary>
    /// Конструктор
    /// </summary>
    public TeacherController(ILogger<TeacherController> logger,
        IAppService appService)
    {
        _logger = logger;
        _appService = appService;
    }

    /// <summary>
    /// Удалить запись о преподавателе
    /// </summary>
    [HttpGet("del/{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Удалить запись о преподавателе")]
    public async Task<IActionResult> DeleteTeacher(long id)
    {
        var response = await _appService.DeleteTeacher(id);

        return Ok(response);
    }

    /// <summary>
    /// Добавить запись о преподавателе
    /// </summary>
    [HttpPut("add")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Добавить запись о преподавателе")]
    public async Task<IActionResult> AddTeacher(TeacherInDto person)
    {
        var response = await _appService.AddTeacher(person);

        return Ok(response);
    }

    /// <summary>
    /// Cменить пароль
    /// </summary>
    [HttpPost("updatePwd")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Cменить пароль")]
    public async Task<IActionResult> UpdatePassword(PasswordInDto passwordDto)
    {
        var response = await _appService.UpdatePassword(passwordDto);

        return Ok(response);
    }

    /// <summary>
    /// Вывести всех преподавателей
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status200OK)]
    [Description("Вывод преподавателей")]
    public async Task<IActionResult> GetTeachers()
    {
        var response = await _appService.GetTeachers();

        return Ok(response);
    }

    /// <summary>
    /// Вывести всех преподавателей
    /// </summary>
    [HttpPost("byLogin")]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status200OK)]
    [Description("Получить преподавателя")]
    public async Task<IActionResult> GetTeacherByLogin(LoginDto model)
    {
        var response = await _appService.GetTeacherByLogin(model);

        return Ok(response);
    }

    [HttpPut("update")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Description("Редактировать запись о преподавателе")]
    public async Task<IActionResult> UpdateTeacher(TeacherDto updateTeacher)
    {
        var response = await _appService.UpdateTeacher(updateTeacher);

        return Ok(response);
    }
}
