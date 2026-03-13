namespace SkillSwap.Api.Models.Dtos;

public class NotificationDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;  // ExchangeOffer | ExchangeAccepted
    public string Message { get; set; } = string.Empty;   // заголовок, например "Татьяна предлагает вам обмен"
    public string SubMessage { get; set; } = string.Empty; // подсказка, например "Примите обмен, чтобы обсудить детали"
    public int ExchangeRequestId { get; set; }
    public int RelatedUserId { get; set; }   // id пользователя для перехода в профиль
    public string RelatedUserName { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
