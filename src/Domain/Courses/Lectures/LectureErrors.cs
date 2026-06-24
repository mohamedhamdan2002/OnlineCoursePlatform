using Domain.Common.Results;

namespace Domain.Courses.Lectures;

public static class LectureErrors
{
    public static Error IdRequired = new Error(400, "Id is Required");
    public static Error SectionIdRequired = new Error(400, "SectionId is Required");
    public static Error TitleRequired = new Error(400, "Section Title is Required");
    public static Error VideoUrlInvalid = new Error(400, "Section VideoUrl is Invalid");
    public static Error DurationInvalid = new Error(400, "Lecture duration must be great than 00:00:00");
}
