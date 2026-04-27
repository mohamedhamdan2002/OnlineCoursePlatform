using Application.Common.Errors;
using Application.Common.Interfaces;
using Domain.Common.Results;
using Domain.Enrollments;
using Domain.Payments.Enums;
using Domain.Payments.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Payments.Commands.ConfirmPayment;

public sealed class ConfirmPaymentCommandHandler(IAppDbContext context) : IRequestHandler<ConfirmPaymentCommand, Result>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result> Handle(ConfirmPaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(payment => payment.OrderId == command.OrderId);
        if (payment == null)
            return Result.Fail(ApplicationErrors.InvalidPaymentProcess);

        if (payment.Status == PaymentStatus.Succeeded)
            return Result.Fail(ApplicationErrors.InvalidPaymentProcess); 
        payment.MarkAsSucceeded();
        // fire event to create enrollment
        payment.AddDomainEvent(new PaymentSucceededEvent { CourseId = payment.CourseId, PaymentId = payment.Id, UserId = payment.UserId });
       
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
