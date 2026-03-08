namespace SkillSwap.Api.Models.Dtos;

public class CreateExchangeRequestDto
{
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public int OfferedSkillId { get; set; }
    public int RequestedSkillId { get; set; }
}
