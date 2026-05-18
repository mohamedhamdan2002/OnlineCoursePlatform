using Application.Reviews.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Reviews.Commands.CreateReview;

public sealed record CreateReviewCommand(
    Guid CourseId,
    string Comment,
    int Rateing
) : IRequest<Result<ReviewDto>>;
