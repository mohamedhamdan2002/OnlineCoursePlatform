using Application.Features.Payments.Dots;
using Domain.Common.Results;
using MediatR;

namespace Application.Features.Payments.Commands.CreatePaymentOrder;

public sealed record CreatePaymentOrderCommand(
        Guid UserId, 
        Guid CourseId
    ) : IRequest<Result<PaymentOrderDto>>;
