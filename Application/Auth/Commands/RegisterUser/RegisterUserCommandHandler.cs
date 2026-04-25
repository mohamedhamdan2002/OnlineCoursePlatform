using Application.Common.Interfaces;
using Application.Dtos.User;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler(UserManager<User> userManager, IJwtTokenService jwt) 
    : IRequestHandler<RegisterUserCommand, AuthDto>
{
    public async Task<AuthDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        
        if(!Enum.TryParse(typeof(AppRole), request.RegistrationDto.Role, out var role))
        {
            throw new Exception("bad Request this is invalid Role");
        }
        if (await userManager.Users.AnyAsync(user => user.Email == request.RegistrationDto.Email))
            throw new Exception("There is an user taken the email");
        var user = new User
        {
            Email = request.RegistrationDto.Email,
            UserName = $"{request.RegistrationDto.Email.Split('@')[0]}{Guid.NewGuid().ToString()[..4]}",
            FirstName = request.RegistrationDto.FirstName,
            LastName = request.RegistrationDto.LastName,
            Role = (AppRole) role
        };
        var result = await userManager.CreateAsync(user, request.RegistrationDto.Password);
        if (!result.Succeeded)
            throw new Exception("Bad Request");
        await userManager.AddToRoleAsync(user, user.Role.ToString());
        var token = await jwt.CreateTokenAsync(user);
        return new AuthDto { Email = user.Email, Token = token };
    }
}
