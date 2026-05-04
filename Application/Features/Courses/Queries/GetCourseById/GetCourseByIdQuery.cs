using Application.Common.Interfaces;
using Application.Features.Courses.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Features.Courses.Queries.GetCourseById;

public sealed record GetCourseByIdQuery(Guid CourseId) : ICacheRequest<Result<CourseDto>>
{
    public string CacheKey => $"course_{CourseId}";

    public string[] Tags => ["course"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}
