using Application.Common.Errors;
using Application.Common.Interfaces;
using Domain.Common.Results;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Features.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandHandler(IAppDbContext context) : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly IAppDbContext _context = context;
    public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindAsync(command.Id, cancellationToken);
        if (category is null)
        {
            return Result.Fail(ApplicationErrors.CategoryNotFound);
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
