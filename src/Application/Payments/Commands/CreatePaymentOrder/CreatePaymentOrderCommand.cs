using Application.Payments.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Payments.Commands.CreatePaymentOrder;

public sealed record CreatePaymentOrderCommand(
        Guid CourseId
    ) : IRequest<Result<PaymentOrderDto>>;
