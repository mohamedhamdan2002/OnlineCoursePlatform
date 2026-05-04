using Application.Common.Errors;
using Application.Common.Interfaces;
using Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.Features.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandler(IAppDbContext context, HybridCache cache) : IRequestHandler<UpdateCategoryCommand, Result>
{
    private readonly IAppDbContext _context = context;
    private readonly HybridCache _cache = cache;

    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(command.Id, cancellationToken);
        if (category is null)
        {
            return Result.Fail(ApplicationErrors.CategoryNotFound);
        }

        var updateCategoryResult = category.Update(command.Name.Trim());
        if (updateCategoryResult.IsFailure)
        {
            return Result.Fail(updateCategoryResult.Error);
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByTagAsync("category", cancellationToken);
        return Result.Success();
    }
}