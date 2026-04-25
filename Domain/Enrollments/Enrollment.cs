using Domain.Common;
namespace Domain.Enrollments;

public class Enrollment : Entity
{
    public Guid StudentId { get; private set; }
    public Guid CourseId { get; private set; }
    public string? ProviderPaymentId { get; private set; } 
    public DateTime EnrolledAt { get; private set; }
}

public enum PaymentStatus
{
    Pending,      // بعد create order
    Processing,  // بعد capture
    Succeeded,   // بعد webhook
    Failed
}
