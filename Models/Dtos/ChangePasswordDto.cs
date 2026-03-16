namespace SkillSwap.Api.Models.Dtos;

/// <summary>Тело запроса смены пароля.</summary>
public class ChangePasswordDto
{
    /// <summary>Id пользователя (текущий пользователь).</summary>
    public int UserId { get; set; }
    /// <summary>Текущий пароль (для проверки).</summary>
    public string CurrentPassword { get; set; } = string.Empty;
    /// <summary>Новый пароль (не менее 8 символов).</summary>
    public string NewPassword { get; set; } = string.Empty;
}
