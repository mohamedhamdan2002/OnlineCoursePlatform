using Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Courses.Commands.UploadLectureVideo;

public sealed record UploadLectureVideoCommand(
    Guid LectureId,
    IFormFile VideoFile
) : IRequest<Result>;