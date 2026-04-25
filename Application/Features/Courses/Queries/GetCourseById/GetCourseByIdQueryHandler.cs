using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Features.Courses.Dtos;
using Application.Features.Courses.Mappers;
using Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Courses.Queries.GetCourseById;

public sealed class GetCourseByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetCourseByIdQuery, Result<CourseDto>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<CourseDto>> Handle(GetCourseByIdQuery query, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.AsNoTracking()
                                            .Include(course => course.Instructor)
                                            .Include(course => course.Category)
                                            .Include(course => course.Sections)
                                            .ThenInclude(section => section.Lectures)
                                            .FirstOrDefaultAsync(course => course.Id == query.CourseId, cancellationToken);
        if (course is null)
        {
            return Result.Fail<CourseDto>(ApplicationErrors.CourseNotFound);
        }
        return Result.Success(course.ToDto());
    }
}