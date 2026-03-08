using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Services;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeSessionsController : ControllerBase
{
    private readonly ExchangeSessionService _sessionService;

    public ExchangeSessionsController(ExchangeSessionService sessionService) => _sessionService = sessionService;

    /// <summary>Список сессий обмена (по userId и/или status).</summary>
    /// <param name="userId">Id пользователя-участника.</param>
    /// <param name="status">Статус: Active, Completed, Cancelled.</param>
    /// <returns>Список сессий.</returns>
    [HttpGet]
    public IActionResult GetSessions([FromQuery] int? userId, [FromQuery] string? status) =>
        Ok(_sessionService.GetSessions(userId, status));

    /// <summary>Сессия по id.</summary>
    /// <param name="id">Id сессии.</param>
    /// <returns>Сессия или 404.</returns>
    [HttpGet("{id:int}")]
    public IActionResult GetSession(int id)
    {
        var s = _sessionService.GetSessionById(id);
        if (s == null) return NotFound();
        return Ok(s);
    }

    /// <summary>Завершить сессию обмена.</summary>
    /// <param name="id">Id сессии.</param>
    /// <returns>Обновлённая сессия или 404.</returns>
    [HttpPost("{id:int}/complete")]
    public IActionResult Complete(int id)
    {
        var s = _sessionService.CompleteSession(id);
        if (s == null) return NotFound();
        return Ok(s);
    }

    /// <summary>Отменить сессию обмена.</summary>
    /// <param name="id">Id сессии.</param>
    /// <returns>Обновлённая сессия или 404.</returns>
    [HttpPost("{id:int}/cancel")]
    public IActionResult Cancel(int id)
    {
        var s = _sessionService.CancelSession(id);
        if (s == null) return NotFound();
        return Ok(s);
    }
}
