namespace SkillSwap.Api.Models.Dtos;

/// <summary>Создание/редактирование навыка пользователя (форма: название, категория, подкатегория, описание, фото).</summary>
public class CreateUserSkillDto
{
    public string Title { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int SkillId { get; set; }
    public string? Description { get; set; }
    /// <summary>URL загруженных изображений навыка (после загрузки через API или статику).</summary>
    public List<string>? ImageUrls { get; set; }
}
