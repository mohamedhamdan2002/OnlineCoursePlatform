using Application.Features.Courses.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Features.Courses.Commands.CreateLecture;

public sealed record CreateLectureCommand(
    Guid SectionId,
    string Title
): IRequest<Result<LectureDto>>;
