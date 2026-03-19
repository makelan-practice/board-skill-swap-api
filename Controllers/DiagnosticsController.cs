using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Services;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/diagnostics")]
public class DiagnosticsController : ControllerBase
{
    private readonly ApiFailureModeService _apiFailureModeService;

    public DiagnosticsController(ApiFailureModeService apiFailureModeService)
    {
        _apiFailureModeService = apiFailureModeService;
    }

    /// <summary>Текущий статус режима имитации ошибки сервера.</summary>
    [HttpGet("server-error-mode")]
    public IActionResult GetServerErrorMode()
    {
        return Ok(new { enabled = _apiFailureModeService.IsEnabled });
    }

    /// <summary>Включить или выключить глобальную имитацию ошибки 500 для API.</summary>
    /// <param name="enabled">true — все API-методы (кроме этого служебного эндпоинта) отвечают 500.</param>
    [HttpPost("server-error-mode")]
    public IActionResult SetServerErrorMode([FromQuery] bool enabled)
    {
        var current = _apiFailureModeService.SetEnabled(enabled);
        return Ok(new
        {
            enabled = current,
            message = current
                ? "Режим имитации ошибки 500 включён."
                : "Режим имитации ошибки 500 выключен."
        });
    }
}
