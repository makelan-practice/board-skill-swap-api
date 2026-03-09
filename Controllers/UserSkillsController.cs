using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Models.Dtos;
using SkillSwap.Api.Services;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/users/{userId:int}/skills")]
public class UserSkillsController : ControllerBase
{
    private readonly UserSkillService _userSkillService;

    public UserSkillsController(UserSkillService userSkillService) => _userSkillService = userSkillService;

    /// <summary>Список навыков пользователя (название, категория, подкатегория, описание, фото).</summary>
    [HttpGet]
    public IActionResult GetUserSkills(int userId) => Ok(_userSkillService.GetByUserId(userId));

    /// <summary>Один навык по id.</summary>
    [HttpGet("{id:int}", Name = nameof(GetUserSkill))]
    public IActionResult GetUserSkill(int userId, int id)
    {
        var dto = _userSkillService.GetById(id);
        if (dto == null || dto.UserId != userId) return NotFound();
        return Ok(dto);
    }

    /// <summary>Создать навык пользователя (форма: название, категория, подкатегория, описание, изображения).</summary>
    [HttpPost]
    public IActionResult CreateUserSkill(int userId, [FromBody] CreateUserSkillDto dto)
    {
        var created = _userSkillService.Create(userId, dto);
        if (created == null) return NotFound();
        return CreatedAtRoute(nameof(GetUserSkill), new { userId, id = created.Id }, created);
    }

    /// <summary>Обновить навык пользователя.</summary>
    [HttpPut("{id:int}")]
    public IActionResult UpdateUserSkill(int userId, int id, [FromBody] CreateUserSkillDto dto)
    {
        var updated = _userSkillService.Update(id, userId, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    /// <summary>Удалить навык пользователя.</summary>
    [HttpDelete("{id:int}")]
    public IActionResult DeleteUserSkill(int userId, int id)
    {
        if (!_userSkillService.Delete(id, userId)) return NotFound();
        return NoContent();
    }
}
