using Domain.Common;
using Domain.Courses;

namespace Domain.Enrollments.Events;

public sealed class EnrollmentCreatedEvent : DomainEvent
{
    public Guid UserId { get; init; }
    public Guid EnrollmentId { get; init; }
    public DateTime EnrolledAt { get; init; }
    public Guid CourseId { get; init; }
}
