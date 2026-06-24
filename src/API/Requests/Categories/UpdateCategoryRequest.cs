using System.ComponentModel.DataAnnotations;

namespace API.Requests.Categories;

public record UpdateCategoryRequest
{
    [Required(ErrorMessage = "Name is Required")]
    public string Name { get; init; }
}