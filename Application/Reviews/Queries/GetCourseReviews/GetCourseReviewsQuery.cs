using Application.Common.Interfaces;
using Application.Reviews.Dtos;
using Domain.Common.Results;
using MediatR;
namespace Application.Reviews.Queries.GetCourseReviews;

public sealed record GetCourseReviewsQuery(
    Guid CourseId
) : ICacheRequest<Result<List<ReviewDto>>>
{
    public string CacheKey => $"reviews_courseId={CourseId}";

    public string[] Tags => ["review"];

    public TimeSpan Expiration => TimeSpan.FromSeconds(60);
}
