using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public AppRole Role { get; set; } 
}

public enum AppRole
{
    Instructor,
    Student,
    Admin
}