using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Services;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsController : ControllerBase
{
    private readonly SkillService _skillService;

    public SkillsController(SkillService skillService) => _skillService = skillService;

    /// <summary>Все категории навыков.</summary>
    /// <returns>Список категорий.</returns>
    [HttpGet("categories")]
    public IActionResult GetCategories() => Ok(_skillService.GetCategories());

    /// <summary>Категория по id.</summary>
    /// <param name="id">Id категории.</param>
    /// <returns>Категория или 404.</returns>
    [HttpGet("categories/{id:int}")]
    public IActionResult GetCategory(int id)
    {
        var cat = _skillService.GetCategoryById(id);
        if (cat == null) return NotFound();
        return Ok(cat);
    }

    /// <summary>Список навыков (по категории и/или поиск по названию)</summary>
    [HttpGet]
    public IActionResult GetSkills([FromQuery] int? categoryId, [FromQuery] string? search) =>
        Ok(_skillService.GetSkills(categoryId, search));

    /// <summary>Навык по id.</summary>
    /// <param name="id">Id навыка.</param>
    /// <returns>Навык или 404.</returns>
    [HttpGet("{id:int}")]
    public IActionResult GetSkill(int id)
    {
        var skill = _skillService.GetSkillById(id);
        if (skill == null) return NotFound();
        return Ok(skill);
    }
}
