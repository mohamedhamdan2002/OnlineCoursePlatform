using Application.Features.Enrollments.Commands.CreateEnrollment;
using Application.Features.Payments.Commands.ConfirmPayment;
using Domain.Payments.Events;
using MediatR;

namespace Application.Features.Payments.EventHandlers;

public sealed class PaymentSucceededEventHandler(ISender sender) : INotificationHandler<PaymentSucceededEvent>
{
    private readonly ISender _sender = sender;

    public async Task Handle(PaymentSucceededEvent notification, CancellationToken cancellationToken)
    {
        var command = new CreateEnrollmentCommand(notification.UserId, notification.CourseId, notification.PaymentId);
        await _sender.Send(command, cancellationToken);
    }
}
