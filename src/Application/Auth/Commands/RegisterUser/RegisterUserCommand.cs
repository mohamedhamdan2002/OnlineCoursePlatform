using Application.Auth.Dtos.User;
using MediatR;

namespace Application.Auth.Commands.RegisterUser;

public sealed record RegisterUserCommand(UserForRegistrationDto RegistrationDto) : IRequest<AuthDto>;