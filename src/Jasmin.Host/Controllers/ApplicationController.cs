using Jasmin.Common.Dto.Input;
using Jasmin.Common.Dto.Output;
using Jasmin.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Jasmin.Host.Controllers;

/// <summary>
/// Основной контроллер сервиса
/// </summary>
//[Produces(MediaTypeNames.Application.Json)]
//[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]  // Указываем, что контроллер возвращает JSON
[Consumes(MediaTypeNames.Application.Octet)]
[Route("api/application")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly ILogger<ApplicationController> _logger;
    private readonly IAppService _appService;

    /// <summary>
    /// Конструктор
    /// </summary>
    public ApplicationController(ILogger<ApplicationController> logger,
        IAppService appService)
    {
        _logger = logger;
        _appService = appService;
    }

    [HttpPost("upload")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Consumes("application/json", "application/octet-stream")]
    public async Task<IActionResult> UploadFile()
    {
        // Читаем массив байтов из тела запроса
        using (var memoryStream = new MemoryStream())
        {
            await Request.Body.CopyToAsync(memoryStream);
            byte[] bytes = memoryStream.ToArray();

            try
            {
                // Вызов сервиса для загрузки файла
                await _appService.UploadFile(bytes);
                return Ok(true); // Возвращаем успешный статус
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                _logger.LogError(ex, "Ошибка при загрузке файла");
                return StatusCode(500, $"Ошибка при загрузке файла: {ex.Message}"); // Возвращаем ошибку 500
            }
        }
    }


}


