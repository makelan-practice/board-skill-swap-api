using Microsoft.AspNetCore.Mvc;
using SkillSwap.Api.Models.Dtos;
using SkillSwap.Api.Services;


namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService) => _authService = authService;

    /// <summary>Регистрация по расширенной форме (email, пароль, имя, дата рождения, пол, город, аватар, навыки «Учусь»).</summary>
    /// <remarks>Ошибки: Email уже используется — 409 + ErrorCode "EmailAlreadyExists". Пароль &lt; 8 знаков — 400 + "PasswordTooShort".</remarks>
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto dto)
    {
        var result = _authService.Register(dto);
        if (!result.Success)
        {
            if (result.ErrorCode == "EmailAlreadyExists")
                return Conflict(new { errorCode = result.ErrorCode, message = result.ErrorMessage });
            return BadRequest(new { errorCode = result.ErrorCode, message = result.ErrorMessage });
        }
        var user = result.User!;
        return CreatedAtAction(
            nameof(UsersController.GetUser),
            "Users",
            new { id = user.Id },
            new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                Name = user.Name,
                AvatarUrl = user.AvatarUrl,
                Token = $"mock-token-{user.Id}-{Guid.NewGuid():N}"
            });
    }

    /// <summary>Вход по email и паролю. Возвращает данные пользователя и мок-токен.</summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var response = _authService.Login(dto);
        if (response == null)
            return Unauthorized(new { message = "Неверный email или пароль" });
        return Ok(response);
    }

    /// <summary>Смена пароля пользователя.</summary>
    /// <remarks>Ошибки: WrongPassword — 400. PasswordTooShort — 400. UserNotFound — 404.</remarks>
    [HttpPost("change-password")]
    public IActionResult ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var (success, errorCode, errorMessage) = _authService.ChangePassword(dto.UserId, dto.CurrentPassword, dto.NewPassword);
        if (success) return Ok(new { message = "Пароль успешно изменён" });
        if (errorCode == "UserNotFound") return NotFound(new { errorCode, message = errorMessage });
        return BadRequest(new { errorCode, message = errorMessage });
    }
}
