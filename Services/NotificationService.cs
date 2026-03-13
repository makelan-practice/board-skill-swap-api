using SkillSwap.Api.Models;
using SkillSwap.Api.Models.Dtos;

namespace SkillSwap.Api.Services;

public class NotificationService
{
    private readonly MockDataStore _store;

    public NotificationService(MockDataStore store) => _store = store;

    /// <summary>Список уведомлений пользователя. По умолчанию все; unreadOnly=true — только непрочитанные (новые).</summary>
    public IEnumerable<NotificationDto> GetNotifications(int userId, bool unreadOnly = false)
    {
        var list = _store.Notifications.Where(n => n.UserId == userId);
        if (unreadOnly)
            list = list.Where(n => !n.IsRead);
        return list.OrderByDescending(n => n.CreatedAt).Select(ToDto);
    }

    /// <summary>Отметить уведомление как прочитанное.</summary>
    public bool MarkAsRead(int notificationId, int userId)
    {
        var n = _store.Notifications.FirstOrDefault(x => x.Id == notificationId && x.UserId == userId);
        if (n == null) return false;
        n.IsRead = true;
        return true;
    }

    /// <summary>Отметить все уведомления пользователя как прочитанные (Прочитать все).</summary>
    public int MarkAllAsRead(int userId)
    {
        var list = _store.Notifications.Where(n => n.UserId == userId && !n.IsRead).ToList();
        foreach (var n in list) n.IsRead = true;
        return list.Count;
    }

    /// <summary>Удалить просмотренные уведомления (Очистить).</summary>
    public int ClearViewed(int userId)
    {
        var toRemove = _store.Notifications.Where(n => n.UserId == userId && n.IsRead).ToList();
        foreach (var n in toRemove) _store.Notifications.Remove(n);
        return toRemove.Count;
    }

    /// <summary>Создать уведомление (вызывается при создании/принятии заявки на обмен).</summary>
    public Notification CreateNotification(int userId, string type, int exchangeRequestId, DateTime? createdAt = null)
    {
        var n = new Notification
        {
            Id = _store.NextNotificationId(),
            UserId = userId,
            Type = type,
            ExchangeRequestId = exchangeRequestId,
            IsRead = false,
            CreatedAt = createdAt ?? DateTime.UtcNow
        };
        _store.Notifications.Add(n);
        return n;
    }

    private NotificationDto ToDto(Notification n)
    {
        var req = _store.ExchangeRequests.FirstOrDefault(r => r.Id == n.ExchangeRequestId);
        string fromName = _store.Users.FirstOrDefault(u => u.Id == req?.FromUserId)?.Name ?? "";
        string toName = _store.Users.FirstOrDefault(u => u.Id == req?.ToUserId)?.Name ?? "";
        int relatedUserId;
        string message, subMessage;
        if (n.Type == "ExchangeOffer")
        {
            relatedUserId = req?.FromUserId ?? 0;
            message = $"{fromName} предлагает вам обмен";
            subMessage = "Примите обмен, чтобы обсудить детали";
        }
        else
        {
            relatedUserId = req?.ToUserId ?? 0;
            var toUser = _store.Users.FirstOrDefault(u => u.Id == relatedUserId);
            var accepted = toUser?.GenderId == 3 ? "приняла" : "принял";
            message = $"{toName} {accepted} ваш обмен";
            subMessage = "Перейдите в профиль, чтобы обсудить детали";
        }
        var relatedName = _store.Users.FirstOrDefault(u => u.Id == relatedUserId)?.Name ?? "";
        return new NotificationDto
        {
            Id = n.Id,
            Type = n.Type,
            Message = message,
            SubMessage = subMessage,
            ExchangeRequestId = n.ExchangeRequestId,
            RelatedUserId = relatedUserId,
            RelatedUserName = relatedName,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        };
    }
}
