using Application.Common.Interfaces;
using System.Security.Claims;

namespace API.Services;

public sealed class CurrentUser(IHttpContextAccessor httpAccessor) : ICurrentUser
{
    public Guid UserId
    {
        get
        {
            var claim = httpAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrWhiteSpace(claim) || !Guid.TryParse(claim, out var userId))
                throw new UnauthorizedAccessException("User is not authenticated.");
            return userId;
        }
    }
}
