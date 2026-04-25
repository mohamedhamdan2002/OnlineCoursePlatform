namespace Application.Features.Courses.Dtos;

public sealed record SectionDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public int Order { get; init; }
    public List<LectureDto>? Lectures { get; init; }
}
