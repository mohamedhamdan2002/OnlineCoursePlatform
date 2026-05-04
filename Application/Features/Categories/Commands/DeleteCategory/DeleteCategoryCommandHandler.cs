using Application.Common.Errors;
using Application.Common.Interfaces;
using Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace Application.Features.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandHandler(IAppDbContext context, HybridCache cache) : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly IAppDbContext _context = context;
    private readonly HybridCache _cache = cache;
    public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(command.Id, cancellationToken);
        if (category is null)
        {
            return Result.Fail(ApplicationErrors.CategoryNotFound);
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveByTagAsync("category", cancellationToken);
        return Result.Success();
    }
}
