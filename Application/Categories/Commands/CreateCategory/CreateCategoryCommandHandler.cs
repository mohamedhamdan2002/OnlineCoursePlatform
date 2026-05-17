using Application.Categories.Dtos;
using Application.Categories.Mappers;
using Application.Common.Interfaces;
using Domain.Categories;
using Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler(
    IAppDbContext context,
    HybridCache cache
) : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
{
    private readonly IAppDbContext _context = context;
    private readonly HybridCache _cache = cache;

    public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {

        var createCategoryResult = Category.Create(Guid.NewGuid(), command.Name.Trim());

        if (createCategoryResult.IsFailure)
        {
            return Result.Fail<CategoryDto>(createCategoryResult.Error);
        }

        _context.Categories.Add(createCategoryResult.Data);
        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByTagAsync("category", cancellationToken);
        return Result.Success(createCategoryResult.Data.ToDto());
    }
}
