namespace API.Requests.Reviews;

public sealed record CreateReviewRequest
{
    public Guid CourseId { get; init; }
    public string Comment { get; init; } = null!;
    public int Rating { get; init; }
}
