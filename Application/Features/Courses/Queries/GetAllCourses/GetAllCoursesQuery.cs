using Application.Common.Utilities;
using Application.Features.Courses.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Features.Courses.Queries.GetAllCourses;

public sealed record GetAllCoursesQuery(
    int PageNumber,
    int PageSize,
    GuidCollection? CategoriesIds = null
) : IRequest<Result<PageList<CourseDto>>>;
