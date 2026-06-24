using Domain.Common.Results;

namespace Domain.Enrollments;

public static class EnrollmentErrors
{
    public static Error IdRequired = new Error(400, "Enrollment Id is Required");

    public static Error StudentIdRequired = new Error(400, "StudentId is Required");

    public static Error CourseIdRequired = new Error(400, "CourseId is Required");

    public static Error ProviderPaymentIdRequired = new Error(400, "Provider Payment Id is Required");
}
