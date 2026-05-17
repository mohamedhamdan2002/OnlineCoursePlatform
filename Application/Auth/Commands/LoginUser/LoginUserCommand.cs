using Application.Auth.Dtos.User;
using MediatR;

namespace Application.Auth.Commands.LoginUser;

public sealed record LoginUserCommand(UserForLoginDto UserForLoginDto) : IRequest<AuthDto>;
