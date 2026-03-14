namespace SkillSwap.Api.Models;

public class User
{
    public int Id { get; set; }
    /// <summary>Email для входа (уникальный).</summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>Пароль (в моке хранится в открытом виде; в боевом — только хэш).</summary>
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Дата рождения (дд.мм.гггг).</summary>
    public DateOnly? DateOfBirth { get; set; }
    /// <summary>Id города из справочника (references/cities).</summary>
    public int? CityId { get; set; }
    public int Age { get; set; }
    /// <summary>Id пола из справочника (references/genders). 1 = Не указан, 2 = Мужской, 3 = Женский.</summary>
    public int GenderId { get; set; } = 1;
    public string? AvatarUrl { get; set; }
    /// <summary>О себе (краткое описание для карточки).</summary>
    public string? About { get; set; }
    /// <summary>Чему может научить (Учу)</summary>
    public List<int> TeachingSkillIds { get; set; } = new();
    /// <summary>Чему хочет научиться (Учусь)</summary>
    public List<int> LearningSkillIds { get; set; } = new();
}
