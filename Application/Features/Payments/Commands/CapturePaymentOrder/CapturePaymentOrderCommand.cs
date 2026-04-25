using Domain.Common.Results;
using MediatR;

namespace Application.Features.Payments.Commands.CapturePaymentOrder;

public sealed record CapturePaymentOrderCommand(
        string OrderId,
        Guid PaymentId
    ) : IRequest<Result>;
