using Domain.Common.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<Result<string>> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default);
    Task<Result<string>> UploadVideoAsync(IFormFile file, CancellationToken cancellationToken = default);
}
