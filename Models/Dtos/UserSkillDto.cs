namespace SkillSwap.Api.Models.Dtos;

/// <summary>Навык пользователя в ответе API (с названиями категории и подкатегории).</summary>
public class UserSkillDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int SkillId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}
