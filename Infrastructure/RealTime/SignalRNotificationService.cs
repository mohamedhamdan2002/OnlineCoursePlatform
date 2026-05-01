using Application.Common.Interfaces;
using Application.Features.Enrollments.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.RealTime;
public sealed class SignalRNotificationService(IHubContext<NotificationHub> hubContext) : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

    public async Task SendEnrollmentCreatedAsync(Guid userId, EnrollmentDto enrollment, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.User(userId.ToString()).SendAsync("EnrollmentCreated", enrollment, cancellationToken);
    }
}
