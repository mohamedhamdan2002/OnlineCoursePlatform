using Application.Features.Courses.Dtos;

namespace Application.Features.Enrollments.Dtos;

public sealed record EnrollmentDto
{
    public Guid Id { get; init; }
    public CourseDto Course { get; init; }
    public DateTime EnrolledAt { get; init; }
}
