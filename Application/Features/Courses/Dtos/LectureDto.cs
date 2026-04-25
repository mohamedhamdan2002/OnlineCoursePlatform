namespace Application.Features.Courses.Dtos;

public sealed record LectureDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string VideoUrl { get; init; }
    public DateTime Duration { get; init; }
    public bool IsPreview { get; init; }
}