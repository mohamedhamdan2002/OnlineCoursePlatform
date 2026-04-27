using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Features.Payments.Dots;
using Application.Features.Payments.Mappers;
using Domain.Common.Results;
using Domain.Payments;
using Domain.Payments.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Payments.Commands.CreatePaymentOrder;

public sealed class CreatePaymentOrderCommandHandler(IAppDbContext context, IPayPalService payPal) : IRequestHandler<CreatePaymentOrderCommand, Result<PaymentOrderDto>>
{
    private readonly IAppDbContext _context = context;
    private readonly IPayPalService _payPal = payPal;

    public async Task<Result<PaymentOrderDto>> Handle(CreatePaymentOrderCommand command, CancellationToken cancellationToken)
    {
        var course = await _context.Courses.FindAsync(command.CourseId, cancellationToken);
        if (course == null)
            return Result.Fail<PaymentOrderDto>(ApplicationErrors.CourseNotFound);
        
        var isAreadyBuyThisCourse = await _context.Enrollments.AnyAsync(e => e.CourseId == command.CourseId && e.StudentId == command.UserId, cancellationToken);
        if (isAreadyBuyThisCourse)
            return Result.Fail<PaymentOrderDto>(ApplicationErrors.UserAlreadyEnrolled);
        
        var paymentResult = Payment.Create(Guid.NewGuid(), command.UserId, command.CourseId, course.Price, PaymentProvider.PayPal);
        if (paymentResult.IsFailure)
            return Result.Fail<PaymentOrderDto>(paymentResult.Error);

        var orderIdResult = await _payPal.CreateOrderAsync(course.Price);
        if(orderIdResult.IsFailure)
            return Result.Fail<PaymentOrderDto>(orderIdResult.Error);
        
        paymentResult.Data.SetOrderId(orderIdResult.Data);

        _context.Payments.Add(paymentResult.Data);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(paymentResult.Data.ToDto());
    }
}