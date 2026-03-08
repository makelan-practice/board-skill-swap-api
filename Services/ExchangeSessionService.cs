using SkillSwap.Api.Models;

namespace SkillSwap.Api.Services;

public class ExchangeSessionDto
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public int User1Id { get; set; }
    public string User1Name { get; set; } = string.Empty;
    public int User2Id { get; set; }
    public string User2Name { get; set; } = string.Empty;
    public string Skill1Name { get; set; } = string.Empty;
    public string Skill2Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public class ExchangeSessionService
{
    private readonly MockDataStore _store;

    public ExchangeSessionService(MockDataStore store) => _store = store;

    /// <summary>Возвращает список сессий обмена с фильтрацией по пользователю и статусу.</summary>
    /// <param name="userId">Id пользователя (сессии, где он участник).</param>
    /// <param name="status">Статус: Active, Completed, Cancelled.</param>
    /// <returns>Список сессий в виде DTO.</returns>
    public IEnumerable<ExchangeSessionDto> GetSessions(int? userId = null, string? status = null)
    {
        var list = _store.ExchangeSessions.AsEnumerable();
        if (userId.HasValue)
            list = list.Where(s => s.User1Id == userId.Value || s.User2Id == userId.Value);
        if (!string.IsNullOrEmpty(status))
            list = list.Where(s => s.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        return list.Select(ToDto);
    }

    /// <summary>Возвращает сессию обмена по идентификатору.</summary>
    /// <param name="id">Id сессии.</param>
    /// <returns>DTO сессии или null.</returns>
    public ExchangeSessionDto? GetSessionById(int id)
    {
        var s = _store.ExchangeSessions.FirstOrDefault(x => x.Id == id);
        return s == null ? null : ToDto(s);
    }

    public ExchangeSessionDto? CompleteSession(int sessionId)
    {
        var s = _store.ExchangeSessions.FirstOrDefault(x => x.Id == sessionId);
        if (s == null) return null;
        s.Status = "Completed";
        s.CompletedAt = DateTime.UtcNow;
        return ToDto(s);
    }

    /// <summary>Отменяет сессию обмена.</summary>
    /// <param name="sessionId">Id сессии.</param>
    /// <returns>Обновлённая сессия в виде DTO или null.</returns>
    public ExchangeSessionDto? CancelSession(int sessionId)
    {
        var s = _store.ExchangeSessions.FirstOrDefault(x => x.Id == sessionId);
        if (s == null) return null;
        s.Status = "Cancelled";
        s.CompletedAt = DateTime.UtcNow;
        return ToDto(s);
    }

    private static string UserName(MockDataStore store, int id) => store.Users.FirstOrDefault(u => u.Id == id)?.Name ?? "";
    private static string SkillName(MockDataStore store, int id) => store.Skills.FirstOrDefault(s => s.Id == id)?.Name ?? "";

    private ExchangeSessionDto ToDto(ExchangeSession s)
    {
        return new ExchangeSessionDto
        {
            Id = s.Id,
            RequestId = s.RequestId,
            User1Id = s.User1Id,
            User1Name = UserName(_store, s.User1Id),
            User2Id = s.User2Id,
            User2Name = UserName(_store, s.User2Id),
            Skill1Name = SkillName(_store, s.Skill1Id),
            Skill2Name = SkillName(_store, s.Skill2Id),
            Status = s.Status,
            StartedAt = s.StartedAt,
            CompletedAt = s.CompletedAt
        };
    }
}
