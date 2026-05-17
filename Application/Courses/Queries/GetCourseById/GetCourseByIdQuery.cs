using Application.Common.Interfaces;
using Application.Courses.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Courses.Queries.GetCourseById;

public sealed record GetCourseByIdQuery(Guid CourseId, Guid UserId) : ICacheRequest<Result<CourseDto>>
{
    public string CacheKey => $"course_userId={UserId.ToString()}_courseId={CourseId}";

    public string[] Tags => ["course"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}
