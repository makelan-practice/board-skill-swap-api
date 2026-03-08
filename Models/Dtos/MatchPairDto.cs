namespace SkillSwap.Api.Models.Dtos;

public class MatchPairDto
{
    public UserCardDto User { get; set; } = null!;
    public string YouTeachSkill { get; set; } = string.Empty;
    public string TheyTeachSkill { get; set; } = string.Empty;
}
