using Domain.Enrollments;

namespace Application.Payments.Dtos;

public sealed record PaymentOrderDto
{
    public Guid PaymentId { get; init; }
    public string Status { get; init; }
    public string OrderId { get; init; }
}
