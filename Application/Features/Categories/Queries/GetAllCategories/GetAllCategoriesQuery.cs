using Application.Common.Interfaces;
using Application.Features.Categories.Dots;
using Domain.Common.Results;
using MediatR;

namespace Application.Features.Categories.Queries.GetAllCategories;

//public sealed record GetAllCategoriesQuery() : IRequest<Result<List<CategoryDto>>>;
public sealed record GetAllCategoriesQuery() : ICacheRequest<Result<List<CategoryDto>>>
{
    public string CacheKey => "categories";

    public string[] Tags => ["category"];

    public TimeSpan Expiration => TimeSpan.FromMinutes(10);
}

