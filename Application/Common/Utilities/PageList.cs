namespace Application.Common.Utilities;

public class PageList<T>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }

    public IReadOnlyCollection<T>? PageItems { get; init; }
}
