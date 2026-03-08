namespace SkillSwap.Api.Models.Dtos;

public class UserCardDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Age { get; set; }
    public string? AvatarUrl { get; set; }
    public List<string> CanTeach { get; set; } = new();   // Учу
    public List<string> WantsToLearn { get; set; } = new(); // Учусь
}
