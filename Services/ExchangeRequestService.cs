using SkillSwap.Api.Models;
using SkillSwap.Api.Models.Dtos;

namespace SkillSwap.Api.Services;

public class ExchangeRequestService
{
    private readonly MockDataStore _store;

    public ExchangeRequestService(MockDataStore store) => _store = store;

    /// <summary>Возвращает список заявок на обмен с опциональной фильтрацией по пользователю и статусу.</summary>
    /// <param name="userId">Id пользователя (заявки, где он отправитель или получатель).</param>
    /// <param name="status">Статус заявки: Pending, Accepted, Rejected.</param>
    /// <returns>Список заявок в виде DTO.</returns>
    public IEnumerable<ExchangeRequestDto> GetRequests(int? userId = null, string? status = null)
    {
        var list = _store.ExchangeRequests.AsEnumerable();
        if (userId.HasValue)
            list = list.Where(r => r.FromUserId == userId.Value || r.ToUserId == userId.Value);
        if (!string.IsNullOrEmpty(status))
            list = list.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        return list.Select(ToDto);
    }

    /// <summary>Возвращает заявку на обмен по идентификатору.</summary>
    /// <param name="id">Id заявки.</param>
    /// <returns>DTO заявки или null, если не найдена.</returns>
    public ExchangeRequestDto? GetRequestById(int id)
    {
        var r = _store.ExchangeRequests.FirstOrDefault(x => x.Id == id);
        return r == null ? null : ToDto(r);
    }

    /// <summary>Создаёт новую заявку на обмен навыками.</summary>
    /// <param name="dto">Данные заявки: от кого, кому, какой навык предлагает, какому хочет научиться.</param>
    /// <returns>Созданная заявка в виде DTO.</returns>
    public ExchangeRequestDto Create(CreateExchangeRequestDto dto)
    {
        var req = new ExchangeRequest
        {
            Id = _store.NextRequestId(),
            FromUserId = dto.FromUserId,
            ToUserId = dto.ToUserId,
            OfferedSkillId = dto.OfferedSkillId,
            RequestedSkillId = dto.RequestedSkillId,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };
        _store.ExchangeRequests.Add(req);
        return ToDto(req);
    }

    /// <summary>Принимает или отклоняет заявку на обмен. При принятии создаётся сессия обмена.</summary>
    /// <param name="requestId">Id заявки.</param>
    /// <param name="accept">true — принять, false — отклонить.</param>
    /// <returns>Обновлённая заявка в виде DTO или null, если заявка не найдена.</returns>
    public ExchangeRequestDto? Respond(int requestId, bool accept)
    {
        var req = _store.ExchangeRequests.FirstOrDefault(r => r.Id == requestId);
        if (req == null) return null;
        req.Status = accept ? "Accepted" : "Rejected";
        req.RespondedAt = DateTime.UtcNow;
        if (accept)
        {
            _store.ExchangeSessions.Add(new ExchangeSession
            {
                Id = _store.NextSessionId(),
                RequestId = req.Id,
                User1Id = req.FromUserId,
                User2Id = req.ToUserId,
                Skill1Id = req.OfferedSkillId,
                Skill2Id = req.RequestedSkillId,
                Status = "Active",
                StartedAt = DateTime.UtcNow
            });
        }
        return ToDto(req);
    }

    private ExchangeRequestDto ToDto(ExchangeRequest r)
    {
        string UserName(int id) => _store.Users.FirstOrDefault(u => u.Id == id)?.Name ?? "";
        string SkillName(int id) => _store.Skills.FirstOrDefault(s => s.Id == id)?.Name ?? "";
        return new ExchangeRequestDto
        {
            Id = r.Id,
            FromUserId = r.FromUserId,
            FromUserName = UserName(r.FromUserId),
            ToUserId = r.ToUserId,
            ToUserName = UserName(r.ToUserId),
            OfferedSkillName = SkillName(r.OfferedSkillId),
            RequestedSkillName = SkillName(r.RequestedSkillId),
            Status = r.Status,
            CreatedAt = r.CreatedAt
        };
    }
}
