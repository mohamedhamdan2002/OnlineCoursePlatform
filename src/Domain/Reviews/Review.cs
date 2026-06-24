using Domain.Common;
using Domain.Common.Results;
using Domain.Courses;
using Domain.Identity;

namespace Domain.Reviews;
public class Review : Entity
{
    public Guid StudentId { get; private set; }
    public User Student { get; private set; } = null!;

    public Guid CourseId { get; private set; }
    public Course Course { get; private set; } = null!;

    public int Rating { get; private set; }

    public string Comment { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    private Review()
    {
    }

    private Review(
        Guid id,
        Guid studentId,
        Guid courseId,
        int rating,
        string comment,
        DateTime createdAt
    ) : base(id)
    {
        StudentId = studentId;
        CourseId = courseId;
        Rating = rating;
        Comment = comment;
        CreatedAt = createdAt;
    }

    public static Result<Review> Create(
        Guid id,
        Guid studentId,
        Guid courseId,
        int rating,
        string comment)
    {
        if (id == Guid.Empty)
            return Result.Fail<Review>(ReviewErrors.IdRequired);

        if (studentId == Guid.Empty)
            return Result.Fail<Review>(ReviewErrors.StudentIdRequired);

        if (courseId == Guid.Empty)
            return Result.Fail<Review>(ReviewErrors.CourseIdRequired);

        if (rating < 1 || rating > 5)
            return Result.Fail<Review>(ReviewErrors.InvalidRating);

        if (string.IsNullOrWhiteSpace(comment))
            return Result.Fail<Review>(ReviewErrors.CommentRequired);

        return Result.Success(new Review(
            id,
            studentId,
            courseId,
            rating,
            comment,
            DateTime.UtcNow
        ));
    }

    public Result Update(string comment, int rating)
    {
        if (rating < 1 || rating > 5)
            return Result.Fail(ReviewErrors.InvalidRating);

        if (string.IsNullOrWhiteSpace(comment))
            return Result.Fail(ReviewErrors.CommentRequired);

        Rating = rating;
        Comment = comment;

        return Result.Success();
    }
}
