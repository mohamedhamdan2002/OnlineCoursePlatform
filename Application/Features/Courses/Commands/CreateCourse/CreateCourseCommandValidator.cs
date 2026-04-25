using Domain.Courses.Enums;
using FluentValidation;

namespace Application.Features.Courses.Commands.CreateCourse;

public sealed class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required");
       

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Course Price must be greater than 0");

        RuleFor(x => x.Level)
            .IsInEnum()
            .WithMessage("Course Level must be valid, valid values ['Beginner', 'Advanced', 'Intermediate']");

        RuleFor(x => x.InstructorId)
            .NotEmpty()
            .WithMessage("InstructorId is required");

        RuleFor(x => x.Image)
            .NotEmpty()
            .WithMessage("Course Image is required");


    }
}
