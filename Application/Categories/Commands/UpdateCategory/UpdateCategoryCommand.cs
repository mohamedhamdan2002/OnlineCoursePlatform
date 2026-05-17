using Domain.Common.Results;
using MediatR;

namespace Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(Guid Id, string Name) : IRequest<Result>;
