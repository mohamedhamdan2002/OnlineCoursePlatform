using Domain.Identity;

namespace Application.Common.Interfaces;

public interface IJwtTokenService
{
    Task<string> CreateTokenAsync(User user);
}
