using Application.Courses.Dtos;

namespace Application.Enrollments.Dtos;

public sealed record EnrollmentDto
{
    public Guid Id { get; init; }
    public CourseDto Course { get; init; }
    public DateTime EnrolledAt { get; init; }
}
