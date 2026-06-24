using Application.Categories.Dtos;
using Domain.Common.Results;
using MediatR;

namespace Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name) : IRequest<Result<CategoryDto>>;
