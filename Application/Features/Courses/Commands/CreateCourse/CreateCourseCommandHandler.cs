using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Features.Courses.Dtos;
using Application.Features.Courses.Mappers;
using Domain.Common.Results;
using Domain.Courses;
using MediatR;

namespace Application.Features.Courses.Commands.CreateCourse;

public sealed class CreateCourseCommandHandler(IAppDbContext context, IFileStorageService fileStorage) : IRequestHandler<CreateCourseCommand, Result<CourseDto>>
{
    private readonly IAppDbContext _context = context;
    private readonly IFileStorageService _fileStorage = fileStorage;

    public async Task<Result<CourseDto>> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(command.CategoryId, cancellationToken);
        if(category is null)
        {
            return Result.Fail<CourseDto>(ApplicationErrors.CategoryNotFound);
        }
        var instructor = await _context.Users.FindAsync(command.InstructorId, cancellationToken);
        if (instructor is null) 
        {
            return Result.Fail<CourseDto>(ApplicationErrors.InstructorNotFound);
        }
        var uploadImageResult = await _fileStorage.UploadImageAsync(command.Image, cancellationToken);
        if (uploadImageResult.IsFailure)
        {
            return Result.Fail<CourseDto>(uploadImageResult.Error);
        }
        var courseResult = Course.Create(Guid.NewGuid(), command.Title.Trim(), command.Description.Trim(), command.Price, uploadImageResult.Data, command.Level, command.CategoryId, command.InstructorId);
        if (courseResult.IsFailure)
        {
            return Result.Fail<CourseDto>(courseResult.Error);
        }
        _context.Courses.Add(courseResult.Data);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success(courseResult.Data.ToDto());

    }
}
