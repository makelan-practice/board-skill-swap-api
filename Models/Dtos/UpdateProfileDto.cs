namespace SkillSwap.Api.Models.Dtos;

/// <summary>Тело запроса обновления профиля (все поля опциональны).</summary>
public class UpdateProfileDto
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    /// <summary>Дата рождения (дд.мм.гггг).</summary>
    public DateOnly? DateOfBirth { get; set; }
    public int? CityId { get; set; }
    public int? GenderId { get; set; }
    public string? AvatarUrl { get; set; }
    public string? About { get; set; }
}
