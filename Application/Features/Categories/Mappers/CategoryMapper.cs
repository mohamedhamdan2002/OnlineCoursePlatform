using Application.Features.Categories.Dots;
using Domain.Categories;

namespace Application.Features.Categories.Mappers;

public static class CategoryMapper
{
    public static CategoryDto ToDto(this Category entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        return new CategoryDto(
            Id: entity.Id,
            Name: entity.Name
        );
    }
    public static List<CategoryDto> ToListOfDtos(this IEnumerable<Category> entities) => [..entities.Select(entity => ToDto(entity)).ToList()];
    
}
