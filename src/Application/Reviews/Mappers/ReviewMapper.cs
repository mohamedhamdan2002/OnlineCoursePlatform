using Application.Reviews.Dtos;
using Domain.Reviews;

namespace Application.Reviews.Mappers;

public static class ReviewMapper
{
    public static ReviewDto ToDto(this Review entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        return new ReviewDto
        {
            Id = entity.Id,
            Student = entity.Student.FirstName + " " + entity.Student.LastName,
            Rating = entity.Rating,
            Comment = entity.Comment,
            CreatedAt = entity.CreatedAt
        };
    }

    public static List<ReviewDto> ToListOfDtos(this IEnumerable<Review> entities) =>
        [.. entities.Select(static entity => entity.ToDto())];
}