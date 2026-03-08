using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Services;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchController : ControllerBase
{
    private readonly MatchService _matchService;

    public MatchController(MatchService matchService) => _matchService = matchService;

    /// <summary>Подбор пар для обмена: пользователи, с которыми возможен взаимный обмен навыками.</summary>
    /// <param name="userId">Id пользователя, для которого подбирают пары.</param>
    /// <param name="maxCount">Максимальное количество пар (по умолчанию 20).</param>
    /// <returns>Список пар: пользователь и навыки обмена.</returns>
    [HttpGet]
    public IActionResult GetMatches([FromQuery] int userId, [FromQuery] int maxCount = 20) =>
        Ok(_matchService.GetMatchesForUser(userId, maxCount));
}
