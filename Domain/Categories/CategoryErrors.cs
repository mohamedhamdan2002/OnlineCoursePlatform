using Domain.Common.Results;

namespace Domain.Categories;

public static class CategoryErrors
{
    public static Error NameRequired = new Error(400, "Category Name is required and not empty");
}
