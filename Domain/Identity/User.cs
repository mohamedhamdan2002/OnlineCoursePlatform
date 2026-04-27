using Domain.Enrollments;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public AppRole Role { get; set; }
    private readonly List<Enrollment> _enrollments = [];
    public IEnumerable<Enrollment> Enrollments => _enrollments.AsReadOnly();
}

public enum AppRole
{
    Instructor,
    Student,
    Admin
}