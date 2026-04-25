namespace Application.Dtos.User;

public record UserForRegistrationDto
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string Role { get; init; }
}
