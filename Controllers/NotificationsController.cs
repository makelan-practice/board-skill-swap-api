using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Services;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationsController(NotificationService notificationService) => _notificationService = notificationService;

    /// <summary>Список уведомлений пользователя. Новые и просмотренные можно разделить по флагу unreadOnly.</summary>
    /// <param name="userId">Id пользователя.</param>
    /// <param name="unreadOnly">true — только непрочитанные (новые), false — все.</param>
    /// <returns>Список уведомлений (Message, SubMessage, ExchangeRequestId, RelatedUserId для кнопки «Перейти»).</returns>
    [HttpGet]
    public IActionResult GetNotifications([FromQuery] int userId, [FromQuery] bool unreadOnly = false) =>
        Ok(_notificationService.GetNotifications(userId, unreadOnly));

    /// <summary>Отметить одно уведомление как прочитанное.</summary>
    /// <param name="id">Id уведомления.</param>
    /// <param name="userId">Id пользователя (проверка владельца).</param>
    [HttpPatch("{id:int}/read")]
    public IActionResult MarkAsRead(int id, [FromQuery] int userId)
    {
        if (!_notificationService.MarkAsRead(id, userId)) return NotFound();
        return NoContent();
    }

    /// <summary>Прочитать все — отметить все уведомления пользователя как прочитанные.</summary>
    /// <param name="userId">Id пользователя.</param>
    /// <returns>Количество отмеченных уведомлений.</returns>
    [HttpPost("mark-all-read")]
    public IActionResult MarkAllAsRead([FromQuery] int userId) =>
        Ok(new { markedCount = _notificationService.MarkAllAsRead(userId) });

    /// <summary>Очистить просмотренные — удалить все прочитанные уведомления пользователя.</summary>
    /// <param name="userId">Id пользователя.</param>
    /// <returns>Количество удалённых уведомлений.</returns>
    [HttpDelete("clear-viewed")]
    public IActionResult ClearViewed([FromQuery] int userId) =>
        Ok(new { removedCount = _notificationService.ClearViewed(userId) });
}
