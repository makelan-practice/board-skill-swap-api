using SkillSwap.Api.Models;

namespace SkillSwap.Api.Services;

public class SkillService
{
    private readonly MockDataStore _store;

    public SkillService(MockDataStore store) => _store = store;

    /// <summary>Возвращает все категории навыков.</summary>
    public IEnumerable<Category> GetCategories() => _store.Categories;

    /// <summary>Возвращает категорию по идентификатору.</summary>
    /// <param name="id">Id категории.</param>
    /// <returns>Категория или null.</returns>
    public Category? GetCategoryById(int id) => _store.Categories.FirstOrDefault(c => c.Id == id);

    /// <summary>Возвращает список навыков с опциональной фильтрацией по категории и поиску по названию.</summary>
    /// <param name="categoryId">Id категории (опционально).</param>
    /// <param name="search">Строка поиска по названию навыка.</param>
    /// <returns>Список навыков.</returns>
    public IEnumerable<Skill> GetSkills(int? categoryId = null, string? search = null)
    {
        var skills = _store.Skills.AsEnumerable();
        if (categoryId.HasValue)
            skills = skills.Where(s => s.CategoryId == categoryId.Value);
        if (!string.IsNullOrWhiteSpace(search))
            skills = skills.Where(s => s.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        foreach (var skill in skills)
            skill.Category = _store.Categories.FirstOrDefault(c => c.Id == skill.CategoryId);
        return skills;
    }

    /// <summary>Возвращает навык по идентификатору.</summary>
    /// <param name="id">Id навыка.</param>
    /// <returns>Навык или null.</returns>
    public Skill? GetSkillById(int id)
    {
        var skill = _store.Skills.FirstOrDefault(s => s.Id == id);
        if (skill != null)
            skill.Category = _store.Categories.FirstOrDefault(c => c.Id == skill.CategoryId);
        return skill;
    }
}
