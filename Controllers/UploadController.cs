using Microsoft.AspNetCore.Mvc;

namespace SkillSwap.Api.Controllers;

[ApiController]
[Route("api/upload")]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private const string SkillsFolder = "Skills";
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    public UploadController(IWebHostEnvironment env) => _env = env;

    /// <summary>Загрузить изображение для навыка пользователя. Возвращает URL для поля imageUrls при создании/редактировании скилла.</summary>
    /// <param name="file">Файл изображения (multipart/form-data, имя поля: file).</param>
    /// <returns>URL загруженного файла (подставьте в imageUrls при POST/PUT /api/users/{userId}/skills) или 400 при ошибке.</returns>
    [HttpPost("skill-image")]
    [RequestSizeLimit(MaxFileSizeBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSizeBytes)]
    public IActionResult UploadSkillImage(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "Файл не передан или пустой." });

        if (file.Length > MaxFileSizeBytes)
            return BadRequest(new { error = "Размер файла не должен превышать 5 МБ." });

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
            return BadRequest(new { error = "Допустимые форматы: " + string.Join(", ", AllowedExtensions) + "." });

        var dir = Path.Combine(_env.WebRootPath, SkillsFolder);
        Directory.CreateDirectory(dir);

        var fileName = $"{Guid.NewGuid():N}{ext}";
        var path = Path.Combine(dir, fileName);

        try
        {
            using (var stream = new FileStream(path, FileMode.Create))
                file.CopyTo(stream);
        }
        catch (IOException)
        {
            return StatusCode(500, new { error = "Ошибка при сохранении файла." });
        }

        var url = "/" + SkillsFolder + "/" + fileName;
        return Ok(new { url });
    }
}
