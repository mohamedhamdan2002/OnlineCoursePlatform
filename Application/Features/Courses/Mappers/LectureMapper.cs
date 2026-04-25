using Application.Features.Courses.Dtos;
using Domain.Courses.Lectures;
namespace Application.Features.Courses.Mappers;

public static class LectureMapper
{
    public static LectureDto ToDto(this Lecture entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        return new LectureDto
        {
            Id = entity.Id,
            Title = entity.Title,
            VideoUrl = entity.VideoUrl ?? string.Empty,
            Duration = entity.Duration,
            IsPreview = entity.IsPreview,
        };
    }
    public static List<LectureDto> ToListOfDto(this IEnumerable<Lecture> entities)
    {
        return [.. entities.Select(entity => entity.ToDto())];
    }
}