namespace SkillSwap.Api.Models;

public class ExchangeSession
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public int User1Id { get; set; }
    public int User2Id { get; set; }
    public int Skill1Id { get; set; }  // чему user1 учит user2
    public int Skill2Id { get; set; }  // чему user2 учит user1
    public string Status { get; set; } = "Active"; // Active, Completed, Cancelled
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
