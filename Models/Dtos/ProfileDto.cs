namespace SkillSwap.Api.Models.Dtos;

/// <summary>Полный профиль пользователя для экрана «Личные данные» (включает почту и дату рождения).</summary>
public class ProfileDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Дата рождения (дд.мм.гггг).</summary>
    public DateOnly? DateOfBirth { get; set; }
    public int? CityId { get; set; }
    public string City { get; set; } = string.Empty;
    public int GenderId { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    /// <summary>О себе.</summary>
    public string? About { get; set; }
}
