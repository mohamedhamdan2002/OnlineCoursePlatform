using Application.Courses.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Courses.Commands.CreateSection;

public sealed record CreateSectionCommand(
    Guid CourseId,
    string Title
) : IRequest<Result<SectionDto>>;
