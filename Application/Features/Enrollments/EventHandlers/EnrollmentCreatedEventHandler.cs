using Application.Common.Interfaces;
using Application.Features.Courses.Mappers;
using Application.Features.Enrollments.Dtos;
using Domain.Enrollments.Events;
using MediatR;

namespace Application.Features.Enrollments.EventHandlers;
public sealed class EnrollmentCreatedEventHandler(INotificationService notificationService) : INotificationHandler<EnrollmentCreatedEvent>
{
    private readonly INotificationService _notificationService = notificationService;

    public async Task Handle(EnrollmentCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _notificationService.SendEnrollmentCreatedAsync(notification.UserId,
                new EnrollmentDto
                {
                    Course = notification.Course.ToDto(),
                    EnrolledAt = notification.EnrolledAt,
                    Id = notification.EnrollmentId
                },
                cancellationToken
            );
    }
}
