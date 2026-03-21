using SkillSwap.Api.Models;
using SkillSwap.Api.Models.Dtos;

namespace SkillSwap.Api.Services;

public class UserService
{
    private readonly MockDataStore _store;

    public UserService(MockDataStore store) => _store = store;

    /// <summary>Возвращает список пользователей с фильтрацией по типу активности, навыкам, полу, городу и поиску по названию навыка.</summary>
    /// <param name="activityType">Латиница: <c>can_teach</c> / <c>want_to_learn</c> (или <c>canTeach</c>, <c>wantToLearn</c>, дефис вместо подчёркивания). По-русски по-прежнему: «Могу научить», «Хочу научиться».</param>
    /// <param name="skillIds">Id навыков: показываются пользователи, у которых есть хотя бы один из них.</param>
    /// <param name="genderId">Id пола из справочника (1=Не указан/не имеет значения, 2=Мужской, 3=Женский).</param>
    /// <param name="cityIds">Id городов из справочника; пользователь попадает в выборку, если его город в списке.</param>
    /// <param name="search">Поиск по подстроке названия навыка. Без <paramref name="activityType"/> — совпадение в «Учу» или «Учусь». С <c>can_teach</c> — только в «Могу научить»; с <c>want_to_learn</c> — только в «Хочу научиться».</param>
    /// <returns>Список карточек пользователей.</returns>
    public IEnumerable<UserCardDto> GetUsers(string? activityType = null, int[]? skillIds = null, int? genderId = null, int[]? cityIds = null, string? search = null)
    {
        var users = _store.Users.AsEnumerable();
        var activityKind = ParseActivityType(activityType);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var skillIdsFromSearch = _store.Skills
                .Where(s => s.Name.Contains(search, StringComparison.OrdinalIgnoreCase))
                .Select(s => s.Id)
                .ToHashSet();

            users = activityKind switch
            {
                UserActivityKind.CanTeach => users.Where(u => u.TeachingSkillIds.Any(skillIdsFromSearch.Contains)),
                UserActivityKind.WantToLearn => users.Where(u => u.LearningSkillIds.Any(skillIdsFromSearch.Contains)),
                _ => users.Where(u =>
                    u.TeachingSkillIds.Any(skillIdsFromSearch.Contains) ||
                    u.LearningSkillIds.Any(skillIdsFromSearch.Contains))
            };
        }
        else
        {
            if (activityKind == UserActivityKind.WantToLearn)
                users = users.Where(u => u.LearningSkillIds.Any());
            else if (activityKind == UserActivityKind.CanTeach)
                users = users.Where(u => u.TeachingSkillIds.Any());
        }

        if (skillIds is { Length: > 0 })
            users = users.Where(u =>
                u.TeachingSkillIds.Any(id => skillIds.Contains(id)) ||
                u.LearningSkillIds.Any(id => skillIds.Contains(id)));

        if (genderId.HasValue && genderId.Value != 1) // 1 = Не указан / не имеет значения
            users = users.Where(u => u.GenderId == genderId.Value);

        if (cityIds is { Length: > 0 })
            users = users.Where(u => u.CityId.HasValue && cityIds.Contains(u.CityId.Value));

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

    /// <summary>Возвращает полный профиль пользователя для экрана «Личные данные» (включает Email и DateOfBirth).</summary>
    public ProfileDto? GetProfile(int id)
    {
        var user = _store.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return null;
        var city = user.CityId.HasValue ? _store.Cities.FirstOrDefault(c => c.Id == user.CityId.Value)?.Name ?? "" : "";
        var gender = _store.Genders.FirstOrDefault(g => g.Id == user.GenderId)?.Name ?? "";
        return new ProfileDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            DateOfBirth = user.DateOfBirth,
            CityId = user.CityId,
            City = city,
            GenderId = user.GenderId,
            Gender = gender,
            AvatarUrl = user.AvatarUrl,
            About = user.About
        };
    }

    /// <summary>Обновляет профиль пользователя. Проверяет уникальность email при смене.</summary>
    /// <returns>Обновлённый профиль или null, если пользователь не найден. Ошибки: EmailAlreadyExists.</returns>
    public (ProfileDto? Profile, string? ErrorCode, string? ErrorMessage) UpdateProfile(int id, UpdateProfileDto dto)
    {
        var user = _store.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return (null, null, null);

        if (dto.Email != null)
        {
            var email = dto.Email.Trim().ToLowerInvariant();
            if (_store.Users.Any(u => u.Id != id && u.Email.Trim().ToLowerInvariant() == email))
                return (null, "EmailAlreadyExists", "Email уже используется");
            user.Email = dto.Email.Trim();
        }
        if (dto.Name != null) user.Name = dto.Name.Trim();
        if (dto.DateOfBirth.HasValue)
        {
            user.DateOfBirth = dto.DateOfBirth;
            user.Age = (int)((DateTime.Today - dto.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)).TotalDays / 365.25);
        }
        if (dto.CityId.HasValue) user.CityId = dto.CityId;
        if (dto.GenderId.HasValue) user.GenderId = dto.GenderId.Value;
        if (dto.AvatarUrl != null) user.AvatarUrl = string.IsNullOrWhiteSpace(dto.AvatarUrl) ? null : dto.AvatarUrl.Trim();
        if (dto.About != null) user.About = string.IsNullOrWhiteSpace(dto.About) ? null : dto.About.Trim();

        var updated = GetProfile(id);
        return (updated, null, null);
    }

    /// <summary>Обновляет только URL аватара пользователя (после загрузки файла).</summary>
    public bool UpdateAvatar(int userId, string avatarUrl)
    {
        var user = _store.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return false;
        user.AvatarUrl = avatarUrl;
        return true;
    }

    /// <summary>Возвращает популярных пользователей (по числу лайков — уникальных пользователей, добавивших в избранное).</summary>
    /// <param name="count">Максимальное количество записей.</param>
    /// <returns>Список карточек пользователей.</returns>
    public IEnumerable<UserCardDto> GetPopular(int count = 6)
    {
        var likeCountByTarget = _store.Favorites
            .GroupBy(f => f.TargetUserId)
            .ToDictionary(g => g.Key, g => g.Select(f => f.UserId).Distinct().Count());

        return _store.Users
            .OrderByDescending(u => likeCountByTarget.GetValueOrDefault(u.Id))
            .ThenByDescending(u => u.Id)
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

    private enum UserActivityKind { None, WantToLearn, CanTeach }

    /// <summary>Разбор activityType: латиница для Swagger/клиентов, русский — для совместимости.</summary>
    private static UserActivityKind ParseActivityType(string? activityType)
    {
        if (string.IsNullOrWhiteSpace(activityType)) return UserActivityKind.None;
        var t = activityType.Trim();
        if (t.Equals("Хочу научиться", StringComparison.OrdinalIgnoreCase)) return UserActivityKind.WantToLearn;
        if (t.Equals("Могу научить", StringComparison.OrdinalIgnoreCase)) return UserActivityKind.CanTeach;

        var norm = t.Replace("-", "_", StringComparison.Ordinal);
        if (norm.Equals("want_to_learn", StringComparison.OrdinalIgnoreCase)
            || t.Equals("wantToLearn", StringComparison.OrdinalIgnoreCase)
            || norm.Equals("learning", StringComparison.OrdinalIgnoreCase))
            return UserActivityKind.WantToLearn;
        if (norm.Equals("can_teach", StringComparison.OrdinalIgnoreCase)
            || t.Equals("canTeach", StringComparison.OrdinalIgnoreCase)
            || norm.Equals("teaching", StringComparison.OrdinalIgnoreCase))
            return UserActivityKind.CanTeach;

        return UserActivityKind.None;
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
