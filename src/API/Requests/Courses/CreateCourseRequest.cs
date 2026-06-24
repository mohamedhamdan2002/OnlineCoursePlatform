using Domain.Courses.Enums;

namespace API.Requests.Courses;

public sealed record CreateCourseRequest
{
    public Guid CategoryId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public CourseLevel Level { get; init; }
    public IFormFile Image { get; init; }
    public Guid InstructorId { get; init; }
}

