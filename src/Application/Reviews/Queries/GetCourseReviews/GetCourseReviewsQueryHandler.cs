using Application.Common.Interfaces;
using Application.Reviews.Dtos;
using Application.Reviews.Mappers;
using Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Application.Reviews.Queries.GetCourseReviews;

public sealed class GetCourseReviewsQueryHandler(IAppDbContext context)
    : IRequestHandler<GetCourseReviewsQuery, Result<List<ReviewDto>>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<List<ReviewDto>>> Handle(
        GetCourseReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var reviews = await _context.Reviews
            .AsNoTracking()
            .Where(x => x.CourseId == request.CourseId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(review => new ReviewDto
            {
                Id = review.Id,
                Student = $"{review.Student.FirstName} {review.Student.LastName}",
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(reviews);
    }
}