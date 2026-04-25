using Application.Features.Categories.Dots;
using Domain.Common.Results;
using MediatR;

namespace Application.Features.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name) : IRequest<Result<CategoryDto>>;
