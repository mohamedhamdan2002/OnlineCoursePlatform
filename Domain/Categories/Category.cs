using Domain.Common;
using Domain.Common.Results;

namespace Domain.Categories;

public sealed class Category : Entity
{
    public string Name { get; private set; } = null!;

    private Category()
    {
    }
    private Category(Guid id, string name)
        : base(id)
    {
        Name = name;
    }

    public static Result<Category> Create(Guid id, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Fail<Category>(CategoryErrors.NameRequired);
        }
        return Result.Success(new Category(id, name));
    }
    public Result Update(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Fail(CategoryErrors.NameRequired);
        }
        Name = name;
        return Result.Success();
    }
}
