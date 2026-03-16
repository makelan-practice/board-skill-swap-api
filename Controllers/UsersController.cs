using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Models.Dtos;
using SkillSwap.Api.Services;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IWebHostEnvironment _env;

    public UsersController(UserService userService, IWebHostEnvironment env)
    {
        _userService = userService;
        _env = env;
    }

    /// <summary>Список пользователей с фильтрами (тип активности, навыки, пол, город, поиск по навыку).</summary>
    /// <param name="activityType">«Хочу научиться» или «Могу научить».</param>
    /// <param name="skillIds">Id навыков.</param>
    /// <param name="genderId">Id пола из справочника (1=не имеет значения, 2=Мужской, 3=Женский).</param>
    /// <param name="cityId">Id города из справочника.</param>
    /// <param name="search">Поиск по названию навыка.</param>
    /// <returns>Список карточек пользователей.</returns>
    [HttpGet]
    public IActionResult GetUsers(
        [FromQuery] string? activityType,
        [FromQuery] int[]? skillIds,
        [FromQuery] int? genderId,
        [FromQuery] int? cityId,
        [FromQuery] string? search)
    {
        var list = _userService.GetUsers(activityType, skillIds, genderId, cityId, search);
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

    /// <summary>Полный профиль пользователя для экрана «Личные данные» (почта, дата рождения, пол, город, о себе).</summary>
    /// <param name="id">Id пользователя.</param>
    /// <returns>Профиль или 404.</returns>
    [HttpGet("{id:int}/profile")]
    public IActionResult GetProfile(int id)
    {
        var profile = _userService.GetProfile(id);
        if (profile == null) return NotFound();
        return Ok(profile);
    }

    /// <summary>Обновить профиль пользователя (почта, имя, дата рождения, пол, город, о себе, аватар).</summary>
    /// <param name="id">Id пользователя.</param>
    /// <param name="dto">Данные для обновления (все поля опциональны).</param>
    /// <remarks>Ошибки: Email уже используется — 409 + ErrorCode "EmailAlreadyExists".</remarks>
    [HttpPut("{id:int}")]
    public IActionResult UpdateProfile(int id, [FromBody] UpdateProfileDto dto)
    {
        var (profile, errorCode, errorMessage) = _userService.UpdateProfile(id, dto);
        if (profile == null && errorCode == null) return NotFound();
        if (errorCode == "EmailAlreadyExists")
            return Conflict(new { errorCode, message = errorMessage });
        return Ok(profile);
    }

    /// <summary>Возвращает файл аватара пользователя (изображение).</summary>
    /// <param name="id">Id пользователя.</param>
    /// <returns>Файл изображения (image/jpeg или image/png) или 404.</returns>
    [HttpGet("{id:int}/avatar")]
    public IActionResult GetAvatar(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null || string.IsNullOrEmpty(user.AvatarUrl)) return NotFound();

        // AvatarUrl имеет вид "/Users/имя_файла.jpg"
        var fileName = Path.GetFileName(user.AvatarUrl.TrimStart('/'));
        if (string.IsNullOrEmpty(fileName)) return NotFound();

        var path = Path.Combine(_env.WebRootPath, "Users", fileName);
        if (!System.IO.File.Exists(path)) return NotFound();

        var contentType = Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };

        return PhysicalFile(path, contentType, fileName);
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

    /// <summary>Похожие предложения: карточки пользователей, у которых есть такие же навыки «Может научить», как у указанного пользователя.</summary>
    /// <param name="userId">Id пользователя, карточку которого просматривают.</param>
    /// <param name="count">Количество записей (по умолчанию 6).</param>
    /// <returns>Список карточек пользователей.</returns>
    [HttpGet("similar")]
    public IActionResult GetSimilar([FromQuery] int userId, [FromQuery] int count = 6)
    {
        var list = _userService.GetSimilar(userId, count);
        return Ok(list);
    }
}
