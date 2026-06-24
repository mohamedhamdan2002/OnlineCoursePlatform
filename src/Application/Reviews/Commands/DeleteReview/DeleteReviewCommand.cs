using Domain.Common.Results;
using MediatR;

namespace Application.Reviews.Commands.DeleteReview;

public sealed record DeleteReviewCommand(
    Guid ReviewId
) : IRequest<Result>;