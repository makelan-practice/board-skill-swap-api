using SkillSwap.Api.Models;
using SkillSwap.Api.Models.Dtos;

namespace SkillSwap.Api.Services;

public class FavoriteService
{
    private readonly MockDataStore _store;
    private readonly UserService _userService;

    public FavoriteService(MockDataStore store, UserService userService)
    {
        _store = store;
        _userService = userService;
    }

    /// <summary>Возвращает избранных пользователей для указанного пользователя.</summary>
    /// <param name="userId">Id пользователя, чьё избранное запрашивается.</param>
    /// <param name="usersOnly">Если true — только пользователи; иначе включаются записи по навыкам.</param>
    /// <returns>Список карточек избранных пользователей.</returns>
    public IEnumerable<UserCardDto> GetFavoritesByUserId(int userId, bool usersOnly = true)
    {
        var favs = _store.Favorites.Where(f => f.UserId == userId);
        if (usersOnly)
            favs = favs.Where(f => f.TargetUserId > 0);
        var targetUserIds = favs.Select(f => f.TargetUserId).Distinct().ToList();
        return targetUserIds
            .Select(id => _userService.GetUserById(id))
            .Where(dto => dto != null)!;
    }

    /// <summary>Добавляет пользователя или навык в избранное.</summary>
    /// <param name="userId">Id пользователя, который добавляет в избранное.</param>
    /// <param name="targetUserId">Id пользователя, которого добавляют в избранное.</param>
    /// <param name="targetSkillId">Опционально: id навыка (для избранных навыков).</param>
    /// <returns>Созданная запись избранного или null, если уже в избранном.</returns>
    public Favorite? AddFavorite(int userId, int targetUserId, int? targetSkillId = null)
    {
        if (_store.Favorites.Any(f => f.UserId == userId && f.TargetUserId == targetUserId && f.TargetSkillId == targetSkillId))
            return null;
        var fav = new Favorite
        {
            Id = _store.NextFavoriteId(),
            UserId = userId,
            TargetUserId = targetUserId,
            TargetSkillId = targetSkillId,
            AddedAt = DateTime.UtcNow
        };
        _store.Favorites.Add(fav);
        return fav;
    }

    public bool RemoveFavorite(int userId, int targetUserId, int? targetSkillId = null)
    {
        var fav = _store.Favorites.FirstOrDefault(f =>
            f.UserId == userId && f.TargetUserId == targetUserId &&
            (targetSkillId == null ? f.TargetSkillId == null : f.TargetSkillId == targetSkillId));
        if (fav == null) return false;
        _store.Favorites.Remove(fav);
        return true;
    }

    /// <summary>Проверяет, находится ли пользователь в избранном у указанного пользователя.</summary>
    /// <param name="userId">Id пользователя, у которого проверяют избранное.</param>
    /// <param name="targetUserId">Id проверяемого пользователя.</param>
    /// <returns>true, если в избранном.</returns>
    public bool IsFavorite(int userId, int targetUserId)
    {
        return _store.Favorites.Any(f => f.UserId == userId && f.TargetUserId == targetUserId);
    }
}
