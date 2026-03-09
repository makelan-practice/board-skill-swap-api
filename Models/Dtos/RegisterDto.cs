namespace SkillSwap.Api.Models.Dtos;

/// <summary>Тело запроса регистрации (расширенная форма).</summary>
public class RegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Дата рождения (дд.мм.гггг).</summary>
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? City { get; set; }
    /// <summary>URL аватара (после загрузки) или null.</summary>
    public string? AvatarUrl { get; set; }
    /// <summary>Id навыков, которым хочет научиться (Учусь).</summary>
    public List<int>? LearningSkillIds { get; set; }
}
