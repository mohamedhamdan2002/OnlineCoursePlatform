using Application.Features.Enrollments.Commands.CreateEnrollment;
using Application.Features.Payments.Commands.ConfirmPayment;
using Domain.Payments.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Features.Payments.EventHandlers;

public sealed class PaymentSucceededEventHandler(IServiceScopeFactory scopeFactory) : INotificationHandler<PaymentSucceededEvent>
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task Handle(PaymentSucceededEvent notification, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var command = new CreateEnrollmentCommand(notification.UserId, notification.CourseId, notification.PaymentId);
        await sender.Send(command, cancellationToken);
    }
}
