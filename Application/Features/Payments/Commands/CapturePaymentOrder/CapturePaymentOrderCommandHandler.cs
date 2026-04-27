using Application.Common.Errors;
using Application.Common.Interfaces;
using Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Payments.Commands.CapturePaymentOrder;

public sealed class CapturePaymentOrderCommandHandler(IAppDbContext context, IPayPalService payPal) : IRequestHandler<CapturePaymentOrderCommand, Result>
{
    private readonly IAppDbContext _context = context;
    private readonly IPayPalService _payPal = payPal;

    public async Task<Result> Handle(CapturePaymentOrderCommand command, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments.Where(payment => payment.Id == command.PaymentId && payment.OrderId == command.OrderId).FirstOrDefaultAsync(cancellationToken);
        if (payment == null)
            return Result.Fail(ApplicationErrors.InvalidPaymentProcess);
        
        var result = await _payPal.CaptureOrderAsync(command.OrderId);
        if (result.IsFailure)
            return result;

        payment.MarkAsProcessing();
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}