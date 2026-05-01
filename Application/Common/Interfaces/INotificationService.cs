using Application.Features.Enrollments.Dtos;

namespace Application.Common.Interfaces;

public interface INotificationService
{
    Task SendEnrollmentCreatedAsync(Guid userId, EnrollmentDto enrollment, CancellationToken cancellationToken);
}
