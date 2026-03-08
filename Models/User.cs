namespace SkillSwap.Api.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = "Не указан"; // Мужской, Женский
    public string? AvatarUrl { get; set; }
    /// <summary>Чему может научить (Учу)</summary>
    public List<int> TeachingSkillIds { get; set; } = new();
    /// <summary>Чему хочет научиться (Учусь)</summary>
    public List<int> LearningSkillIds { get; set; } = new();
}
