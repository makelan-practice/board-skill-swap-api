using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Services;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly FavoriteService _favoriteService;

    public FavoritesController(FavoriteService favoriteService) => _favoriteService = favoriteService;

    /// <summary>Избранные пользователи для текущего пользователя.</summary>
    /// <param name="userId">Id пользователя.</param>
    /// <param name="usersOnly">Только пользователи (true) или включая навыки (false).</param>
    /// <returns>Список карточек избранных пользователей.</returns>
    [HttpGet]
    public IActionResult GetFavorites([FromQuery] int userId, [FromQuery] bool usersOnly = true) =>
        Ok(_favoriteService.GetFavoritesByUserId(userId, usersOnly));

    /// <summary>Добавить пользователя в избранное.</summary>
    /// <param name="userId">Id пользователя, который добавляет.</param>
    /// <param name="targetUserId">Id пользователя, которого добавляют.</param>
    /// <param name="targetSkillId">Опционально: id навыка.</param>
    /// <returns>Созданная запись избранного или 409 (уже в избранном).</returns>
    [HttpPost]
    public IActionResult AddFavorite([FromQuery] int userId, [FromQuery] int targetUserId, [FromQuery] int? targetSkillId = null)
    {
        var fav = _favoriteService.AddFavorite(userId, targetUserId, targetSkillId);
        if (fav == null) return Conflict("Уже в избранном");
        return CreatedAtAction(nameof(GetFavorites), new { userId }, fav);
    }

    /// <summary>Удалить из избранного.</summary>
    /// <param name="userId">Id пользователя.</param>
    /// <param name="targetUserId">Id пользователя, которого убирают.</param>
    /// <param name="targetSkillId">Опционально: id навыка.</param>
    /// <returns>204 при успехе или 404.</returns>
    [HttpDelete]
    public IActionResult RemoveFavorite([FromQuery] int userId, [FromQuery] int targetUserId, [FromQuery] int? targetSkillId = null)
    {
        var removed = _favoriteService.RemoveFavorite(userId, targetUserId, targetSkillId);
        if (!removed) return NotFound();
        return NoContent();
    }

    /// <summary>Проверить, в избранном ли пользователь.</summary>
    /// <param name="userId">Id пользователя, у которого проверяют избранное.</param>
    /// <param name="targetUserId">Id проверяемого пользователя.</param>
    /// <returns>Объект { isFavorite: true/false }.</returns>
    [HttpGet("check")]
    public IActionResult IsFavorite([FromQuery] int userId, [FromQuery] int targetUserId) =>
        Ok(new { isFavorite = _favoriteService.IsFavorite(userId, targetUserId) });
}
