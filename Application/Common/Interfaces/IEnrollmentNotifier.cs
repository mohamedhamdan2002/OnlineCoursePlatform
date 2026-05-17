using Application.Enrollments.Dtos;

namespace Application.Common.Interfaces;

public interface IEnrollmentNotifier
{
    Task SendEnrollmentCreatedAsync(Guid userId, EnrollmentDto enrollment, CancellationToken cancellationToken);
}
