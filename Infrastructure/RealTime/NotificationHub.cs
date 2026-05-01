using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.RealTime;
public class NotificationHub : Hub
{
    public const string HubUrl = "/hubs/notifications";
}
