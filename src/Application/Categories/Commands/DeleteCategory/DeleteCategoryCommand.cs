using Domain.Common.Results;
using MediatR;

namespace Application.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest<Result>;
