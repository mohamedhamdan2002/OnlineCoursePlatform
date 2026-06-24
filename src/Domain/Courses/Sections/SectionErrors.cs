using Domain.Common.Results;

namespace Domain.Courses.Sections;

public static class SectionErrors
{
    public static Error IdRequired = new Error(400, "Section Id is Required");
    public static Error CourseIdRequired = new Error(400, "Course Id is Required");
    public static Error TitleRequired = new Error(400, "Section Title is Required");
    public static Error InvalidOrder = new Error(400, "Section Order must be greater than zero");
}
