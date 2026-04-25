using Application.Features.Courses.Dtos;
using Domain.Categories;
using Domain.Common.Results;
using Domain.Courses.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Courses.Commands.CreateCourse;

public sealed record CreateCourseCommand(
    Guid CategoryId,
    string Title,
    string Description,
    decimal Price,
    CourseLevel Level,
    IFormFile Image,
    Guid InstructorId
) : IRequest<Result<CourseDto>>;
