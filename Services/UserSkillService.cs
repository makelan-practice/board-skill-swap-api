using SkillSwap.Api.Models;
using SkillSwap.Api.Models.Dtos;

namespace SkillSwap.Api.Services;

public class UserSkillService
{
    private readonly MockDataStore _store;

    public UserSkillService(MockDataStore store) => _store = store;

    /// <summary>Список навыков пользователя (Учу — с описанием и фото).</summary>
    public IEnumerable<UserSkillDto> GetByUserId(int userId)
    {
        return _store.UserSkills
            .Where(us => us.UserId == userId)
            .Select(ToDto);
    }

    /// <summary>Один навык пользователя по id.</summary>
    public UserSkillDto? GetById(int id)
    {
        var us = _store.UserSkills.FirstOrDefault(x => x.Id == id);
        return us == null ? null : ToDto(us);
    }

    /// <summary>Создать навык пользователя (название, категория, подкатегория, описание, изображения).</summary>
    public UserSkillDto? Create(int userId, CreateUserSkillDto dto)
    {
        if (_store.Users.All(u => u.Id != userId)) return null;
        var us = _store.AddUserSkill(
            userId,
            dto.Title,
            dto.CategoryId,
            dto.SkillId,
            dto.Description,
            dto.ImageUrls?.ToList()
        );
        return ToDto(us);
    }

    /// <summary>Обновить навык пользователя.</summary>
    public UserSkillDto? Update(int id, int userId, CreateUserSkillDto dto)
    {
        var us = _store.UserSkills.FirstOrDefault(x => x.Id == id && x.UserId == userId);
        if (us == null) return null;
        us.Title = dto.Title;
        us.CategoryId = dto.CategoryId;
        us.SkillId = dto.SkillId;
        us.Description = dto.Description;
        us.ImageUrls = dto.ImageUrls?.ToList() ?? us.ImageUrls;
        return ToDto(us);
    }

    /// <summary>Удалить навык пользователя.</summary>
    public bool Delete(int id, int userId)
    {
        var us = _store.UserSkills.FirstOrDefault(x => x.Id == id && x.UserId == userId);
        if (us == null) return false;
        _store.UserSkills.Remove(us);
        return true;
    }

    private UserSkillDto ToDto(UserSkill us)
    {
        var cat = _store.Categories.FirstOrDefault(c => c.Id == us.CategoryId);
        var skill = _store.Skills.FirstOrDefault(s => s.Id == us.SkillId);
        return new UserSkillDto
        {
            Id = us.Id,
            UserId = us.UserId,
            Title = us.Title,
            CategoryId = us.CategoryId,
            CategoryName = cat?.Name ?? "",
            SkillId = us.SkillId,
            SkillName = skill?.Name ?? "",
            Description = us.Description,
            ImageUrls = us.ImageUrls.ToList()
        };
    }
}
