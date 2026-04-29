using Application.Features.Courses.Mappers;
using Application.Features.Enrollments.Dtos;
using Domain.Enrollments;

namespace Application.Features.Enrollments.Mappers;

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
