using Application.Features.Categories.Mappers;
using Application.Features.Courses.Dtos;
using Domain.Courses;

namespace Application.Features.Courses.Mappers;

public static class CourseMapper
{
    public static CourseDto ToDto(this Course entity, bool isEnrolled = false)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        return new CourseDto
        {
            Id = entity.Id,
            Title = entity.Title,
            ImageUrl = entity.ThumbnailUrl ?? string.Empty,
            Level = entity.Level.ToString(),
            Instructor = $"{entity.Instructor.FirstName} {entity.Instructor.LastName}",
            Price = entity.Price,
            IsEnrolled = isEnrolled,
            Rating = entity.AverageRating,
            ReviewsCount = entity.ReviewsCount,
            StudentsCount = entity.StudentsCount,
            Category = entity.Category.ToDto(),
            Sections = entity.Sections.ToListOfDto()
        };
    }
    public static List<CourseDto> ToListOfDto(this IEnumerable<Course> entities)
    {
        return [.. entities.Select(entity => entity.ToDto())];
    }
}
