namespace SkillSwap.Api.Models;

/// <summary>Навык, который пользователь предлагает (Учу): название, категория, подкатегория, описание, фото.</summary>
public class UserSkill
{
    public int Id { get; set; }
    public int UserId { get; set; }
    /// <summary>Название навыка (как пользователь его назвал).</summary>
    public string Title { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    /// <summary>Подкатегория — id навыка из справочника Skills.</summary>
    public int SkillId { get; set; }
    /// <summary>Коротко опишите, чему можете научить.</summary>
    public string? Description { get; set; }
    /// <summary>URL изображений навыка (загруженные фото).</summary>
    public List<string> ImageUrls { get; set; } = new();
}
