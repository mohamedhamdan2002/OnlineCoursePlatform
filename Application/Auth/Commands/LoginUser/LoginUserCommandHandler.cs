using Application.Common.Interfaces;
using Application.Dtos.User;
using Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Commands.LoginUser;

public sealed class LoginUserCommandHandler(UserManager<User> userManager, IJwtTokenService jwt)
    : IRequestHandler<LoginUserCommand, AuthDto>
{
    public async Task<AuthDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.UserForLoginDto.Email);
        if (user is null)
            throw new Exception("please enter a valid email or password");
        var result = await userManager.CheckPasswordAsync(user, request.UserForLoginDto.Password);
        if (!result)
            throw new Exception("please enter a valid email or password");
        var token = await jwt.CreateTokenAsync(user);
        return new AuthDto { Email = user.Email!, Token = token };
    }
}
