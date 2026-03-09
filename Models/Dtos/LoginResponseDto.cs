namespace SkillSwap.Api.Models.Dtos;

/// <summary>Ответ после успешного входа.</summary>
public class LoginResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    /// <summary>Мок-токен (в боевом API — JWT).</summary>
    public string Token { get; set; } = string.Empty;
}
