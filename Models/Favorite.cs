namespace SkillSwap.Api.Models;

public class Favorite
{
    public int Id { get; set; }
    public int UserId { get; set; }      // кто добавил в избранное
    public int TargetUserId { get; set; } // какой пользователь в избранном (или 0 если навык)
    public int? TargetSkillId { get; set; } // какой навык в избранном (опционально)
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
