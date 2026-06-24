using Domain.Common;

namespace Domain.Payments.Events;

public sealed class PaymentSucceededEvent : DomainEvent
{
    public Guid PaymentId { get; set; }
    public Guid UserId { get; set; }
    public Guid CourseId { get; set; }
}
