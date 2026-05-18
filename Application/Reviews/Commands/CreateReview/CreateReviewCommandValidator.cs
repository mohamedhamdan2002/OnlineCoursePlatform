using FluentValidation;

namespace Application.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty()
            .WithMessage("CourseId is Required");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("Comment is Required")
            .MaximumLength(1000)
            .WithMessage("Comment must not exceed 1000 characters");

        RuleFor(x => x.Rateing)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");
    }
}