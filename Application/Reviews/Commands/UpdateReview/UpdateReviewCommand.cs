using Application.Reviews.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Reviews.Commands.UpdateReview;

public sealed record UpdateReviewCommand(
    Guid ReviewId,
    string Comment,
    int Rating
) : IRequest<Result>;
