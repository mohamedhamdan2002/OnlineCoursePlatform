using Application.Features.Courses.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Features.Courses.Commands.CreateSection;

public sealed record CreateSectionCommand(
    Guid CourseId,
    string Title
) : IRequest<Result<SectionDto>>;
