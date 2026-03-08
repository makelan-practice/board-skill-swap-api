using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Models.Dtos;
using SkillSwap.Api.Services;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeRequestsController : ControllerBase
{
    private readonly ExchangeRequestService _requestService;

    public ExchangeRequestsController(ExchangeRequestService requestService) => _requestService = requestService;

    /// <summary>Список заявок (по userId и/или status).</summary>
    /// <param name="userId">Id пользователя (отправитель или получатель).</param>
    /// <param name="status">Статус: Pending, Accepted, Rejected.</param>
    /// <returns>Список заявок.</returns>
    [HttpGet]
    public IActionResult GetRequests([FromQuery] int? userId, [FromQuery] string? status) =>
        Ok(_requestService.GetRequests(userId, status));

    /// <summary>Заявка по id.</summary>
    /// <param name="id">Id заявки.</param>
    /// <returns>Заявка или 404.</returns>
    [HttpGet("{id:int}")]
    public IActionResult GetRequest(int id)
    {
        var req = _requestService.GetRequestById(id);
        if (req == null) return NotFound();
        return Ok(req);
    }

    /// <summary>Создать заявку на обмен.</summary>
    /// <param name="dto">Данные заявки: FromUserId, ToUserId, OfferedSkillId, RequestedSkillId.</param>
    /// <returns>Созданная заявка (201).</returns>
    [HttpPost]
    public IActionResult CreateRequest([FromBody] CreateExchangeRequestDto dto)
    {
        var created = _requestService.Create(dto);
        return CreatedAtAction(nameof(GetRequest), new { id = created.Id }, created);
    }

    /// <summary>Принять или отклонить заявку. При принятии создаётся сессия обмена.</summary>
    /// <param name="id">Id заявки.</param>
    /// <param name="accept">true — принять, false — отклонить.</param>
    /// <returns>Обновлённая заявка или 404.</returns>
    [HttpPost("{id:int}/respond")]
    public IActionResult Respond(int id, [FromQuery] bool accept)
    {
        var req = _requestService.Respond(id, accept);
        if (req == null) return NotFound();
        return Ok(req);
    }
}
