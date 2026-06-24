using Application.Payments.Dtos;
using Domain.Payments;

namespace Application.Payments.Mappers;

public static class PaymentMapper
{
    public static PaymentOrderDto ToDto(this Payment entity)
    {
        return new PaymentOrderDto
        {
            OrderId = entity.OrderId ?? string.Empty,
            Status = entity.Status.ToString(),
            PaymentId = entity.Id
        };
    }

    public static List<PaymentOrderDto> ToListOfDto(this IEnumerable<Payment> entities)
    {
        return [.. entities.Select(entity => entity.ToDto())];
    }

}
