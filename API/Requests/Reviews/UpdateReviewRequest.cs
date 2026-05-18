namespace API.Requests.Reviews;

public sealed record UpdateReviewRequest
{
    public string Comment { get; init; } = null!;
    public int Rating { get; init; }
}