namespace SkillSwap.Api.Models.Dtos;

public class ExchangeRequestDto
{
    public int Id { get; set; }
    public int FromUserId { get; set; }
    public string FromUserName { get; set; } = string.Empty;
    public int ToUserId { get; set; }
    public string ToUserName { get; set; } = string.Empty;
    public string OfferedSkillName { get; set; } = string.Empty;
    public string RequestedSkillName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
