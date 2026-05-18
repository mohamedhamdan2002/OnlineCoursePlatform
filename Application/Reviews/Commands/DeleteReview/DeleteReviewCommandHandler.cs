using Application.Common.Errors;
using Application.Common.Interfaces;
using Domain.Common.Results;
using Domain.Reviews;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.Reviews.Commands.DeleteReview;

public sealed class DeleteReviewCommandHandler(IAppDbContext context, ICurrentUser currentUser ,HybridCache cache)
    : IRequestHandler<DeleteReviewCommand, Result>
{
    public async Task<Result> Handle(
        DeleteReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await context.Reviews.Where(review => review.Id == request.ReviewId).FirstOrDefaultAsync(cancellationToken);

        if (review is null)
            return Result.Fail(ApplicationErrors.ReviewNotFound);

        if(review.StudentId != currentUser.UserId)
            return Result.Fail(ApplicationErrors.UnauthorizedAction);

        context.Reviews.Remove(review);

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync("review");
        return Result.Success();
    }
}