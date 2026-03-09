namespace SkillSwap.Api.Models.Dtos;

public class UserCardDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? CityId { get; set; }
    public string City { get; set; } = string.Empty;  // название из справочника
    public int Age { get; set; }
    public int GenderId { get; set; }
    public string Gender { get; set; } = string.Empty; // название из справочника
    public string? AvatarUrl { get; set; }
    public List<string> CanTeach { get; set; } = new();   // Учу
    public List<string> WantsToLearn { get; set; } = new(); // Учусь
}
