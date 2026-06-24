using FluentValidation;
namespace Application.Reviews.Queries.GetCourseReviews;

public sealed class GetCourseReviewsQueryValidator : AbstractValidator<GetCourseReviewsQuery>
{
    public GetCourseReviewsQueryValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty()
            .WithMessage("CourseId is Required");
    }
}
