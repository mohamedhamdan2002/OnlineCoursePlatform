using Application.Features.Payments.Dots;
using Domain.Payments;

namespace Application.Features.Payments.Mappers;

public static class PaymentMapper
{
    public static PaymentOrderDto ToDto(this Payment entity)
    {
        return new PaymentOrderDto
        {
            OrderId = entity.ProviderPaymentId ?? string.Empty,
            Status = entity.Status.ToString(),
            PaymentId = entity.Id
        };
    }

    public static List<PaymentOrderDto> ToListOfDto(this IEnumerable<Payment> entities)
    {
        return [.. entities.Select(entity => entity.ToDto())];
    }

}
