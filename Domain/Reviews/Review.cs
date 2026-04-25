using Domain.Courses;
using Domain.Identity;

namespace Domain.Reviews;

public class Review
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public User Student { get; set; } = null!;
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;
}
