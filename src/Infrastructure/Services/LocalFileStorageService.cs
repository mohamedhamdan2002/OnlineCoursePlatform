using Application.Common.Errors;
using Application.Common.Interfaces;
using Domain.Common.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
namespace Infrastructure.Services;
public sealed class LocalFileStorageService(IWebHostEnvironment env) : IFileStorageService
{
    private readonly IWebHostEnvironment _env = env;
    private readonly string[] _allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
    private readonly string[] _allowedVideoExtensions = new[] { ".mp4" };


    public async Task<Result<string>> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(file, nameof(file));

        var uploadFolder = Path.Combine(_env.WebRootPath, "images");
        if(!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);     
        }

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_allowedExtensions.Contains(extension))
            return Result.Fail<string>(ApplicationErrors.InvalidImageFileExtension);

        if (file.Length > 5 * 1024 * 1024) // 5MB
            return Result.Fail<string>(ApplicationErrors.ImageFileSizeIsBig);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadFolder, uniqueFileName);
        using var fileStream = new FileStream(filePath, FileMode.CreateNew);
        await file.CopyToAsync(fileStream, cancellationToken);
        return Result.Success($"images/{uniqueFileName}");
    }

    public async Task<Result<string>> UploadVideoAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var uploadFolder = Path.Combine(_env.WebRootPath, "temp");
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_allowedVideoExtensions.Contains(extension))
            return Result.Fail<string>(ApplicationErrors.InvalidVideoFileExtension);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadFolder, uniqueFileName);
        using var fileStream = new FileStream(filePath, FileMode.CreateNew);
        await file.CopyToAsync(fileStream, cancellationToken);
        return Result.Success($"temp/{uniqueFileName}");
    }
}
