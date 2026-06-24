using Domain.Common.Results;

namespace Domain.Reviews;

public static class ReviewErrors
{
    public static Error IdRequired = new Error(400, "Review Id is Required");

    public static Error StudentIdRequired = new Error(400, "StudentId is Required");

    public static Error CourseIdRequired = new Error(400, "CourseId is Required");

    public static Error InvalidRating = new Error(400, "Rating must be between 1 and 5");

    public static Error CommentRequired = new Error(400, "Review comment is Required");
    public static Error InvalidReview = new Error(400, "You not allow to review this course because you not enrolled in this course");

}
