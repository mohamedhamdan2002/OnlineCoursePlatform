using System.ComponentModel.DataAnnotations;

namespace API.Requests.Courses;

public sealed record CoursePageRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

}

