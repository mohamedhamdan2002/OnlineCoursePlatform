using Domain.Common.Results;

namespace Application.Common.Errors;

public static class ApplicationErrors
{
    public static Error CategoryNotFound = new Error(400, "Category does not exist.");
    public static Error InstructorNotFound = new Error(400, "Instructor does not exist.");
    public static Error CourseNotFound = new Error(400, "Course does not exist.");
    public static Error SectionNotFound = new Error(400, "Section does not exist.");
    public static Error LectureNotFound = new Error(400, "Lecture does not exist.");
    public static Error UserNotFound = new Error(400, "User does not exist.");

    public static Error InvalidImageFileExtension = new(400, "Image file extension should be \".jpg\", \".jpeg\", \".png\", \".webp ");
    public static Error ImageFileSizeIsBig = new(400, "Image file size should be less than or Equal 5MB");
    public static Error InvalidVideoFileExtension = new(400, "Video file extension should be \".mp4\"");
    public static Error UserAlreadyEnrolled = new(400, "You try to buy course you already bought it before..");
    public static Error InvalidPaymentProcess = new(400, "Invalid Payment , this payment not exist");
    public static Error AlreadyEnrollmentExist = new(400, "Invalid Enrollment , this Enrollment Already exist");

}
