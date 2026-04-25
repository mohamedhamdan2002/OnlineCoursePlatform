using Domain.Common.Results;

namespace Domain.Courses;

public static class CourseErrors
{
    public static Error IdRequired = new Error(400, "Course Id is Required");
    public static Error TitleRequired = new Error(400, "Course Title is Required");
    public static Error DescriptionShouldNotEmpty = new Error(400, "Course Description should not Empty");
    public static Error PriceInvalid = new Error(400, "Course Price should be greater than zero");
    public static Error LevelInvalid = new Error(400, "Course Level Is Invalid");
    public static Error CategoryIdRequired = new Error(400, "Category Id is Required");
    public static Error InstructorIdRequired = new Error(400, "Instructor Id is Required");
    public static Error ThumbnailUrlInvalid = new Error(400, "Course ThumbnailUrl not be null or empty");
}
