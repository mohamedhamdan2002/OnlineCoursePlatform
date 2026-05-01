using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.RealTime;
[Authorize]
public class EnrollmentHub : Hub
{
    public const string HubUrl = "/hubs/enrollments";
}
