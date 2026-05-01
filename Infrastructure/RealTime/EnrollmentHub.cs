using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.RealTime;
public class EnrollmentHub : Hub
{
    public const string HubUrl = "/hubs/enrollments";
}
