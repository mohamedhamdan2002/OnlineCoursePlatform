using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Reviews.Dtos;
using Domain.Common.Results;
using Domain.Reviews;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.Reviews.Commands.UpdateReview;

public sealed class UpdateReviewCommandHandler(IAppDbContext context, ICurrentUser currentUser, HybridCache cache)
    : IRequestHandler<UpdateReviewCommand, Result>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result> Handle(
        UpdateReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
                                   .Where(review => review.Id == request.ReviewId)
                                   .FirstOrDefaultAsync(cancellationToken);

        if (review is null)
            return Result.Fail(ApplicationErrors.ReviewNotFound);

        if (review.StudentId != currentUser.UserId)
            return Result.Fail(ApplicationErrors.UnauthorizedAction);

        var result = review.Update(request.Comment, request.Rating);

        if (result.IsFailure)
            return Result.Fail(result.Error);

        await _context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync("review");

        return Result.Success();
    }
}
