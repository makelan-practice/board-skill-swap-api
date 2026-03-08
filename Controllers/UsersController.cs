using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Services;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService) => _userService = userService;

    /// <summary>Список пользователей с фильтрами (тип активности, навыки, пол, город, поиск по навыку).</summary>
    /// <param name="activityType">«Хочу научиться» или «Могу научить».</param>
    /// <param name="skillIds">Id навыков.</param>
    /// <param name="gender">Пол: Мужской, Женский, Не имеет значения.</param>
    /// <param name="city">Город.</param>
    /// <param name="search">Поиск по названию навыка.</param>
    /// <returns>Список карточек пользователей.</returns>
    [HttpGet]
    public IActionResult GetUsers(
        [FromQuery] string? activityType,
        [FromQuery] int[]? skillIds,
        [FromQuery] string? gender,
        [FromQuery] string? city,
        [FromQuery] string? search)
    {
        var list = _userService.GetUsers(activityType, skillIds, gender, city, search);
        return Ok(list);
    }

    /// <summary>Карточка пользователя по id.</summary>
    /// <param name="id">Id пользователя.</param>
    /// <returns>Карточка пользователя или 404.</returns>
    [HttpGet("{id:int}")]
    public IActionResult GetUser(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    /// <summary>Популярные пользователи (для блока «Популярное»).</summary>
    /// <param name="count">Количество записей (по умолчанию 6).</param>
    /// <returns>Список карточек пользователей.</returns>
    [HttpGet("popular")]
    public IActionResult GetPopular([FromQuery] int count = 6) => Ok(_userService.GetPopular(count));

    /// <summary>Новые пользователи (для блока «Новое»).</summary>
    /// <param name="count">Количество записей (по умолчанию 6).</param>
    /// <returns>Список карточек пользователей.</returns>
    [HttpGet("new")]
    public IActionResult GetNew([FromQuery] int count = 6) => Ok(_userService.GetNew(count));

    /// <summary>Рекомендуемые пользователи для обмена (для блока «Рекомендуем»).</summary>
    /// <param name="userId">Id текущего пользователя.</param>
    /// <param name="count">Количество записей (по умолчанию 6).</param>
    /// <returns>Список карточек пользователей.</returns>
    [HttpGet("recommended")]
    public IActionResult GetRecommended([FromQuery] int userId, [FromQuery] int count = 6)
    {
        var list = _userService.GetRecommended(userId, count);
        return Ok(list);
    }
}
