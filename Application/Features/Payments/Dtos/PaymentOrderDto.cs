using Domain.Enrollments;

namespace Application.Features.Payments.Dots;

public sealed record PaymentOrderDto
{
    public Guid PaymentId { get; init; }
    public string Status { get; init; }
    public string OrderId { get; init; }
}
