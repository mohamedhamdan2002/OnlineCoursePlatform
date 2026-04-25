using Application.Features.Categories.Dots;

namespace Application.Features.Courses.Dtos;

public sealed record CourseDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string ImageUrl { get; init; }
    public string Instructor { get; init; }
    public string Level { get; init; }
    public decimal Price { get; init; }
    public CategoryDto Category { get; init; }
    public double Rating { get; init; }
    public int ReviewsCount { get; init; }
    public int StudentsCount { get; init; }
    public List<SectionDto>? Sections { get; init; }
}
