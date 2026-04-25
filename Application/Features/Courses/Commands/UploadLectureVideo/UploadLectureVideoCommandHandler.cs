using Application.Common.Errors;
using Application.Common.Interfaces;
using Domain.Common.Results;
using MediatR;

namespace Application.Features.Courses.Commands.UploadLectureVideo;

public sealed class UploadLectureVideoCommandHandler(IAppDbContext context, IFileStorageService fileStorage) : IRequestHandler<UploadLectureVideoCommand, Result>
{
    private readonly IAppDbContext _context = context;
    private readonly IFileStorageService _fileStorage = fileStorage;

    public async Task<Result> Handle(
        UploadLectureVideoCommand command, 
        CancellationToken cancellationToken)
    {
        var lecture = await _context.Lectures.FindAsync(command.LectureId, cancellationToken);
        if(lecture is null)
        {
            return Result.Fail(ApplicationErrors.LectureNotFound);
        }
        var uploadVideoResult = await _fileStorage.UploadVideoAsync(command.VideoFile, cancellationToken);
        var updateVideoUrlResult = lecture.UpdateVideoUrl(uploadVideoResult.Data);
        if(updateVideoUrlResult.IsFailure)
        {
            return updateVideoUrlResult;
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
        
    }
} 
