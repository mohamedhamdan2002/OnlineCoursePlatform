using Application.Features.Categories.Dots;
using Domain.Common.Results;
using MediatR;

namespace Application.Features.Categories.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery() : IRequest<Result<List<CategoryDto>>>;
