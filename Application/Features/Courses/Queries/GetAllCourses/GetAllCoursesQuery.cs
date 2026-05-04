using Application.Common.Interfaces;
using Application.Common.Utilities;
using Application.Features.Courses.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Features.Courses.Queries.GetAllCourses;

public sealed record GetAllCoursesQuery(
    int PageNumber,
    int PageSize,
    GuidCollection? CategoriesIds = null
) : ICacheRequest<Result<PageList<CourseDto>>>
{
    public string CacheKey => $"courses_pageNumber={PageNumber}&pageSize={PageSize}&categoriesIds={string.Join(',', CategoriesIds?.Values ?? [])}";

    public string[] Tags => ["course"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}
