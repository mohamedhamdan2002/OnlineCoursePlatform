using API.Requests.Reviews;
using Application.Reviews.Commands.CreateReview;
using Application.Reviews.Commands.DeleteReview;
using Application.Reviews.Commands.UpdateReview;
using Application.Reviews.Dtos;
using Application.Reviews.Queries.GetCourseReviews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Authorize]
public class ReviewsController(ISender sender) : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("/api/courses/{courseId:guid}/reviews")]
    public async Task<ActionResult<List<ReviewDto>>> GetCourseReviews(
        Guid courseId,
        CancellationToken cancellationToken)
    {
        var query = new GetCourseReviewsQuery(courseId);

        var result = await sender.Send(query, cancellationToken);

        return HandleResult(result, () => Ok(result.Data));
    }

    [HttpPost]
    public async Task<ActionResult<ReviewDto>> Create(
        CreateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateReviewCommand(
            request.CourseId,
            request.Comment,
            request.Rating
        );

        var result = await sender.Send(command, cancellationToken);

        return HandleResult(
            result,
            () => StatusCode(StatusCodes.Status201Created, result.Data));
    }

    [HttpPut("{reviewId:guid}")]
    public async Task<IActionResult> Update(
        Guid reviewId,
        UpdateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateReviewCommand(
            reviewId,
            request.Comment,
            request.Rating
        );

        var result = await sender.Send(command, cancellationToken);

        return HandleResult(result, () => NoContent());
    }

    [HttpDelete("{reviewId:guid}")]
    public async Task<IActionResult> Delete(
        Guid reviewId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteReviewCommand(reviewId);

        var result = await sender.Send(command, cancellationToken);

        return HandleResult(result, () => NoContent());
    }
}