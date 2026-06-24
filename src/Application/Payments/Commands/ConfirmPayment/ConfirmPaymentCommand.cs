using Domain.Common.Results;
using MediatR;

namespace Application.Payments.Commands.ConfirmPayment;

public sealed record ConfirmPaymentCommand(string OrderId) : IRequest<Result>;
