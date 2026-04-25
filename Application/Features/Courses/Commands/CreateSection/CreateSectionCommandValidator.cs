using FluentValidation;

namespace Application.Features.Courses.Commands.CreateSection;

public sealed class CreateSectionCommandValidator : AbstractValidator<CreateSectionCommand>
{
    public CreateSectionCommandValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty().WithMessage("CourseId is Required");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is Required");
    }
}
