using Application.Features.Courses.Dtos;
using Domain.Courses.Sections;

namespace Application.Features.Courses.Mappers;

public static class SectionMapper
{
    public static SectionDto ToDto(this Section entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        return new SectionDto
        {
            Id = entity.Id,
            Title = entity.Title,
            Order = entity.Order,
            Lectures = entity.Lectures.ToListOfDto(),
        };
    }
    public static List<SectionDto> ToListOfDto(this IEnumerable<Section> entities)
    {
        return [.. entities.Select(entity => entity.ToDto())];
    }
}
