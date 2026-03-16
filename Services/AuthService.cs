using SkillSwap.Api.Models;
using SkillSwap.Api.Models.Dtos;

namespace SkillSwap.Api.Services;

/// <summary>Результат регистрации: успех с пользователем или ошибка.</summary>
public class RegisterResult
{
    public bool Success { get; set; }
    public User? User { get; set; }
    /// <summary>Код ошибки: "EmailAlreadyExists", "PasswordTooShort".</summary>
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}

public class AuthService
{
    private readonly MockDataStore _store;

    public AuthService(MockDataStore store) => _store = store;

    /// <summary>Регистрация по расширенной форме. Проверяет уникальность email и длину пароля (не менее 8 символов).</summary>
    public RegisterResult Register(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            return new RegisterResult { Success = false, ErrorCode = "EmailRequired", ErrorMessage = "Email обязателен" };

        var email = dto.Email.Trim().ToLowerInvariant();
        if (_store.Users.Any(u => u.Email.Trim().ToLowerInvariant() == email))
            return new RegisterResult { Success = false, ErrorCode = "EmailAlreadyExists", ErrorMessage = "Email уже используется" };

        if (string.IsNullOrEmpty(dto.Password) || dto.Password.Length < 8)
            return new RegisterResult { Success = false, ErrorCode = "PasswordTooShort", ErrorMessage = "Пароль должен содержать не менее 8 знаков" };

        if (string.IsNullOrWhiteSpace(dto.Name))
            return new RegisterResult { Success = false, ErrorCode = "NameRequired", ErrorMessage = "Имя обязательно" };

        var user = _store.AddUserByRegistration(
            dto.Email.Trim(),
            dto.Password,
            dto.Name.Trim(),
            dto.DateOfBirth,
            dto.GenderId,
            dto.CityId,
            dto.AvatarUrl,
            dto.About,
            dto.LearningSkillIds
        );
        return new RegisterResult { Success = true, User = user };
    }

    /// <summary>Вход по email и паролю. Возвращает данные пользователя и мок-токен или null.</summary>
    public LoginResponseDto? Login(LoginDto dto)
    {
        var email = dto.Email?.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(email)) return null;

        var user = _store.Users.FirstOrDefault(u =>
            u.Email.Trim().ToLowerInvariant() == email && u.Password == dto.Password);
        if (user == null) return null;

        return new LoginResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Name = user.Name,
            AvatarUrl = user.AvatarUrl,
            Token = $"mock-token-{user.Id}-{Guid.NewGuid():N}"
        };
    }

    /// <summary>Смена пароля. Проверяет текущий пароль и длину нового (не менее 8 символов).</summary>
    /// <returns>Успех или код ошибки: WrongPassword, PasswordTooShort.</returns>
    public (bool Success, string? ErrorCode, string? ErrorMessage) ChangePassword(int userId, string currentPassword, string newPassword)
    {
        var user = _store.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return (false, "UserNotFound", "Пользователь не найден");
        if (user.Password != currentPassword)
            return (false, "WrongPassword", "Неверный текущий пароль");
        if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 8)
            return (false, "PasswordTooShort", "Новый пароль должен содержать не менее 8 знаков");
        user.Password = newPassword;
        return (true, null, null);
    }
}
