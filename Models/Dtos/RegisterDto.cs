namespace SkillSwap.Api.Models.Dtos;

/// <summary>Тело запроса регистрации (расширенная форма).</summary>
public class RegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Дата рождения (дд.мм.гггг).</summary>
    public DateOnly? DateOfBirth { get; set; }
    /// <summary>Id пола из справочника (api/references/genders).</summary>
    public int? GenderId { get; set; }
    /// <summary>Id города из справочника (api/references/cities).</summary>
    public int? CityId { get; set; }
    /// <summary>URL аватара (после загрузки) или null.</summary>
    public string? AvatarUrl { get; set; }
    /// <summary>О себе (краткое описание для карточки).</summary>
    public string? About { get; set; }
    /// <summary>Id навыков, которым хочет научиться (Учусь).</summary>
    public List<int>? LearningSkillIds { get; set; }
}
