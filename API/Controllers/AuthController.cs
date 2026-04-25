using API.Filters;
using Application.Auth.Commands.LoginUser;
using Application.Auth.Commands.RegisterUser;
using Application.Dtos.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[TrackActionTimeFilter]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("login")] 
    public async Task<ActionResult>  Login(UserForLoginDto userForLoginDto)
    {
        return Ok(await sender.Send(new LoginUserCommand(userForLoginDto)));
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(UserForRegistrationDto userForRegistrationDto)
    {
        return Ok(await sender.Send(new RegisterUserCommand(userForRegistrationDto)));
    }
}
