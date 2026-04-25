using Application.Common.Utilities;

namespace API.Requests.Courses;

public sealed record CourseFiltersRequest
{
    
    public string? CategoriesIds { get; set; }

}

