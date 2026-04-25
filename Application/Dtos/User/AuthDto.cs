namespace Application.Dtos.User;

public record AuthDto
{
    public string Token { get; init; }
    public string Email { get; init; }
}
