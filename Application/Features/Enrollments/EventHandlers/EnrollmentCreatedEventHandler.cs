using Application.Common.Interfaces;
using Application.Features.Courses.Mappers;
using Application.Features.Enrollments.Dtos;
using Domain.Enrollments.Events;
using MediatR;

namespace Application.Features.Enrollments.EventHandlers;
public sealed class EnrollmentCreatedEventHandler(IEnrollmentNotifier enrollmentNotifier) : INotificationHandler<EnrollmentCreatedEvent>
{
    private readonly IEnrollmentNotifier _enrollmentNotifier = enrollmentNotifier;

    public async Task Handle(EnrollmentCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _enrollmentNotifier.SendEnrollmentCreatedAsync(notification.UserId,
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
