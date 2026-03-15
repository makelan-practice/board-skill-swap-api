using SkillSwap.Api.Models;
using SkillSwap.Api.Models.Dtos;

namespace SkillSwap.Api.Services;

public class UserService
{
    private readonly MockDataStore _store;

    public UserService(MockDataStore store) => _store = store;

    /// <summary>Возвращает список пользователей с фильтрацией по типу активности, навыкам, полу, городу и поиску по названию навыка.</summary>
    /// <param name="activityType">«Хочу научиться» или «Могу научить».</param>
    /// <param name="skillIds">Id навыков: показываются пользователи, у которых есть хотя бы один из них.</param>
    /// <param name="genderId">Id пола из справочника (1=Не указан/не имеет значения, 2=Мужской, 3=Женский).</param>
    /// <param name="cityId">Id города из справочника.</param>
    /// <param name="search">Поиск по названию навыка (в «Учу» или «Учусь»).</param>
    /// <returns>Список карточек пользователей.</returns>
    public IEnumerable<UserCardDto> GetUsers(string? activityType = null, int[]? skillIds = null, int? genderId = null, int? cityId = null, string? search = null)
    {
        var users = _store.Users.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var skillIdsFromSearch = _store.Skills
                .Where(s => s.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .Select(s => s.Id)
                .ToHashSet();
            users = users.Where(u =>
                u.TeachingSkillIds.Any(skillIdsFromSearch.Contains) ||
                u.LearningSkillIds.Any(skillIdsFromSearch.Contains));
        }

        if (!string.IsNullOrEmpty(activityType))
        {
            if (activityType.Equals("Хочу научиться", StringComparison.OrdinalIgnoreCase))
                users = users.Where(u => u.LearningSkillIds.Any());
            else if (activityType.Equals("Могу научить", StringComparison.OrdinalIgnoreCase))
                users = users.Where(u => u.TeachingSkillIds.Any());
        }

        if (skillIds is { Length: > 0 })
            users = users.Where(u =>
                u.TeachingSkillIds.Any(id => skillIds.Contains(id)) ||
                u.LearningSkillIds.Any(id => skillIds.Contains(id)));

        if (genderId.HasValue && genderId.Value != 1) // 1 = Не указан / не имеет значения
            users = users.Where(u => u.GenderId == genderId.Value);

        if (cityId.HasValue)
            users = users.Where(u => u.CityId == cityId.Value);

        return users.Select(ToUserCardDto);
    }

    /// <summary>Возвращает карточку пользователя по идентификатору.</summary>
    /// <param name="id">Id пользователя.</param>
    /// <returns>Карточка пользователя (Учу / Учусь) или null.</returns>
    public UserCardDto? GetUserById(int id)
    {
        var user = _store.Users.FirstOrDefault(u => u.Id == id);
        return user == null ? null : ToUserCardDto(user);
    }

    /// <summary>Возвращает популярных пользователей (по количеству указанных навыков).</summary>
    /// <param name="count">Максимальное количество записей.</param>
    /// <returns>Список карточек пользователей.</returns>
    public IEnumerable<UserCardDto> GetPopular(int count = 6)
    {
        // Мок: считаем "популярными" по количеству навыков
        return _store.Users
            .OrderByDescending(u => u.TeachingSkillIds.Count + u.LearningSkillIds.Count)
            .Take(count)
            .Select(ToUserCardDto);
    }

    /// <summary>Возвращает новых пользователей (последние добавленные по id).</summary>
    /// <param name="count">Максимальное количество записей.</param>
    /// <returns>Список карточек пользователей.</returns>
    public IEnumerable<UserCardDto> GetNew(int count = 6)
    {
        return _store.Users.OrderByDescending(u => u.Id).Take(count).Select(ToUserCardDto);
    }

    /// <summary>Возвращает рекомендуемых для обмена пользователей: те, кто может научить нужному навыку или хочет научиться тому, чему учит текущий.</summary>
    /// <param name="userId">Id текущего пользователя.</param>
    /// <param name="count">Максимальное количество записей.</param>
    /// <returns>Список карточек пользователей.</returns>
    public IEnumerable<UserCardDto> GetRecommended(int userId, int count = 6)
    {
        var user = _store.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return Enumerable.Empty<UserCardDto>();

        // Рекомендуем тех, у кого есть навыки, которым текущий хочет научиться, и кто хочет научиться тому, чему текущий учит
        var recommended = _store.Users
            .Where(u => u.Id != userId)
            .Where(u => user.LearningSkillIds.Any(sid => u.TeachingSkillIds.Contains(sid))
                        || user.TeachingSkillIds.Any(sid => u.LearningSkillIds.Contains(sid)))
            .Take(count)
            .Select(ToUserCardDto);
        return recommended;
    }

    /// <summary>Возвращает похожие предложения: пользователи, у которых есть хотя бы один навык «Может научить» такой же, как у указанного пользователя.</summary>
    /// <param name="userId">Id пользователя, карточку которого просматривают (по нему ищем похожих).</param>
    /// <param name="count">Максимальное количество записей.</param>
    /// <returns>Список карточек пользователей с похожими предложениями.</returns>
    public IEnumerable<UserCardDto> GetSimilar(int userId, int count = 6)
    {
        var user = _store.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null || !user.TeachingSkillIds.Any()) return Enumerable.Empty<UserCardDto>();

        var teachingIds = user.TeachingSkillIds.ToHashSet();
        var similar = _store.Users
            .Where(u => u.Id != userId)
            .Where(u => u.TeachingSkillIds.Any(sid => teachingIds.Contains(sid)))
            .Take(count)
            .Select(ToUserCardDto);
        return similar;
    }

    private UserCardDto ToUserCardDto(User u)
    {
        string SkillName(int id) => _store.Skills.FirstOrDefault(s => s.Id == id)?.Name ?? "";
        var city = u.CityId.HasValue ? _store.Cities.FirstOrDefault(c => c.Id == u.CityId.Value)?.Name ?? "" : "";
        var gender = _store.Genders.FirstOrDefault(g => g.Id == u.GenderId)?.Name ?? "";
        return new UserCardDto
        {
            Id = u.Id,
            Name = u.Name,
            CityId = u.CityId,
            City = city,
            Age = u.Age,
            GenderId = u.GenderId,
            Gender = gender,
            AvatarUrl = u.AvatarUrl,
            About = u.About,
            CanTeach = u.TeachingSkillIds.Select(SkillName).Where(n => n != "").ToList(),
            WantsToLearn = u.LearningSkillIds.Select(SkillName).Where(n => n != "").ToList()
        };
    }
}
