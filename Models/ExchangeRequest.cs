namespace SkillSwap.Api.Models;

public class ExchangeRequest
{
    public int Id { get; set; }
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public int OfferedSkillId { get; set; }   // что предлагаю научить
    public int RequestedSkillId { get; set; } // чему хочу научиться
    public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RespondedAt { get; set; }
}
