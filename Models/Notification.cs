namespace SkillSwap.Api.Models;

/// <summary>Уведомление для пользователя (предложение обмена, принятие обмена и т.д.).</summary>
public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }              // кому показать уведомление
    public string Type { get; set; } = string.Empty;  // ExchangeOffer | ExchangeAccepted
    public int ExchangeRequestId { get; set; }   // связанная заявка на обмен
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
