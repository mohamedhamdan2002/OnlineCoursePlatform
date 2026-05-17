using Application.Courses.Mappers;
using Application.Enrollments.Dtos;
using Application.Enrollments.Mappers;
using Domain.Enrollments;

namespace Application.Enrollments.Mappers;

public static class EnrollmentMapper
{
    public static EnrollmentDto ToDto(this Enrollment entity)
    {
        ArgumentNullException.ThrowIfNull(nameof(entity));
        return new EnrollmentDto
        {
            Id = entity.Id,
            Course = entity.Course.ToDto(),
            EnrolledAt = entity.EnrolledAt
        };
    }
    public static List<EnrollmentDto> ToListOfDto(this IEnumerable<Enrollment> entities)
    {
        return [.. entities.Select(entity => entity.ToDto())];
    }
}
