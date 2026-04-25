using Application.Common.Interfaces;
using Application.Features.Categories.Dots;
using Application.Features.Categories.Mappers;
using Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Features.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandler(IAppDbContext context) : IRequestHandler<GetAllCategoriesQuery, Result<List<CategoryDto>>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.Select(category => category.ToDto()).ToListAsync();
        return Result.Success(categories);
    }
}
