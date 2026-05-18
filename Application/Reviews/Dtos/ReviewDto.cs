

namespace Application.Reviews.Dtos;

public sealed record ReviewDto
{
    public Guid Id { get; init; }
    public string Student { get; init; } = null!;
    public int Rating { get; init; }
    public string Comment { get; init; } = null!;
    public DateTime CreatedAt { get; init; }
}
