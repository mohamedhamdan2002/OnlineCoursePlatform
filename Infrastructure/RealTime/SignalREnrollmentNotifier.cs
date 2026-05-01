using Application.Common.Interfaces;
using Application.Features.Enrollments.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.RealTime;
public sealed class SignalREnrollmentNotifier(IHubContext<EnrollmentHub> hubContext) : IEnrollmentNotifier
{
    private readonly IHubContext<EnrollmentHub> _hubContext = hubContext;

    public async Task SendEnrollmentCreatedAsync(Guid userId, EnrollmentDto enrollment, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.User(userId.ToString()).SendAsync("EnrollmentCreated", enrollment, cancellationToken);
    }
}
