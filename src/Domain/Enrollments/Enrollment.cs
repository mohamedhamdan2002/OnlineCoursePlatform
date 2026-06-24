using Domain.Common;
using Domain.Common.Results;
using Domain.Courses;
namespace Domain.Enrollments;
public class Enrollment : Entity
{
    public Guid StudentId { get; private set; }
    public Guid CourseId { get; private set; }
    public Guid PaymentId { get; private set; }
    public DateTime EnrolledAt { get; private set; }
    public Course Course { get; private set; }
    private Enrollment() { }

    private Enrollment(
        Guid id,
        Guid studentId,
        Guid courseId,
        Guid paymentId,
        DateTime enrolledAt
    ) : base(id)
    {
        StudentId = studentId;
        CourseId = courseId;
        PaymentId = paymentId;
        EnrolledAt = enrolledAt;
    }

    public static Result<Enrollment> Create(
        Guid id,
        Guid studentId,
        Guid courseId,
        Guid paymentId)
    {
        if (id == Guid.Empty)
            return Result.Fail<Enrollment>(EnrollmentErrors.IdRequired);

        if (studentId == Guid.Empty)
            return Result.Fail<Enrollment>(EnrollmentErrors.StudentIdRequired);

        if (courseId == Guid.Empty)
            return Result.Fail<Enrollment>(EnrollmentErrors.CourseIdRequired);

        if (paymentId == Guid.Empty)
            return Result.Fail<Enrollment>(EnrollmentErrors.ProviderPaymentIdRequired);

        return Result.Success(new Enrollment(
            id,
            studentId,
            courseId,
            paymentId,
            DateTime.UtcNow
        ));
    }
}

