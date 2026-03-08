using SkillSwap.Api.Models.Dtos;

namespace SkillSwap.Api.Services;

public class MatchService
{
    private readonly MockDataStore _store;
    private readonly UserService _userService;

    public MatchService(MockDataStore store, UserService userService)
    {
        _store = store;
        _userService = userService;
    }

    /// <summary>Подбор пар для обмена: пользователи, с которыми возможен взаимный обмен навыками (я учу тебя X, ты меня Y).</summary>
    /// <param name="userId">Id пользователя, для которого подбирают пары.</param>
    /// <param name="maxCount">Максимальное количество пар.</param>
    /// <returns>Список пар: пользователь и названия навыков обмена.</returns>
    public IEnumerable<MatchPairDto> GetMatchesForUser(int userId, int maxCount = 20)
    {
        var user = _store.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return Enumerable.Empty<MatchPairDto>();

        var result = new List<MatchPairDto>();
        foreach (var other in _store.Users.Where(u => u.Id != userId))
        {
            // Взаимный обмен: я учу тебя X, ты учишь меня Y
            var swapSkills = user.TeachingSkillIds
                .Where(myTeach => other.LearningSkillIds.Contains(myTeach))
                .SelectMany(myTeach => other.TeachingSkillIds
                    .Where(otherTeach => user.LearningSkillIds.Contains(otherTeach))
                    .Select(otherTeach => (MySkill: myTeach, OtherSkill: otherTeach)))
                .Take(1)
                .ToList();
            if (swapSkills.Count == 0) continue;

            var mySkillName = _store.Skills.FirstOrDefault(s => s.Id == swapSkills[0].MySkill)?.Name ?? "";
            var otherSkillName = _store.Skills.FirstOrDefault(s => s.Id == swapSkills[0].OtherSkill)?.Name ?? "";
            var otherCard = _userService.GetUserById(other.Id);
            if (otherCard == null) continue;

            result.Add(new MatchPairDto
            {
                User = otherCard,
                YouTeachSkill = mySkillName,
                TheyTeachSkill = otherSkillName
            });
            if (result.Count >= maxCount) break;
        }
        return result;
    }
}
