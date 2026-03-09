using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Services;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReferencesController : ControllerBase
{
    private readonly ReferenceService _referenceService;

    public ReferencesController(ReferenceService referenceService) => _referenceService = referenceService;

    /// <summary>Справочник полов (Не указан, Мужской, Женский).</summary>
    /// <returns>Список { id, name } для выпадающего списка.</returns>
    [HttpGet("genders")]
    public IActionResult GetGenders() => Ok(_referenceService.GetGenders());

    /// <summary>Справочник городов.</summary>
    /// <returns>Список { id, name } для выпадающего списка.</returns>
    [HttpGet("cities")]
    public IActionResult GetCities() => Ok(_referenceService.GetCities());

    /// <summary>Пол по id.</summary>
    [HttpGet("genders/{id:int}")]
    public IActionResult GetGender(int id)
    {
        var g = _referenceService.GetGenderById(id);
        if (g == null) return NotFound();
        return Ok(g);
    }

    /// <summary>Город по id.</summary>
    [HttpGet("cities/{id:int}")]
    public IActionResult GetCity(int id)
    {
        var c = _referenceService.GetCityById(id);
        if (c == null) return NotFound();
        return Ok(c);
    }
}
