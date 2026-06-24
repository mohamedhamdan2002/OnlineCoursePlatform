using Application.Common.Interfaces;
using Application.Common.Settings;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtSettings _jwtSettings;
    public JwtTokenService(IOptions<JwtSettings> options, UserManager<User> userManager)
    {
        _jwtSettings = options.Value;
        _userManager = userManager;
    }
    public  async Task<string> CreateTokenAsync(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaimsAsync(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
        var serectKey = new SymmetricSecurityKey(key);
        return new SigningCredentials(serectKey, SecurityAlgorithms.HmacSha256);
    }
    private async Task<List<Claim>> GetClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims) 
    {
        return new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpireOn),
            signingCredentials: signingCredentials,
            claims: claims
        );
    }


}
