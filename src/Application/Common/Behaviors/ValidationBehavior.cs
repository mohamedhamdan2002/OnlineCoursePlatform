using Domain.Common.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validator is null)
            return await next(cancellationToken);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if(validationResult.IsValid)
            return await next(cancellationToken);

        var errors = validationResult.Errors.ConvertAll(
                    error => new Error(StatusCodes.Status400BadRequest, error.ErrorMessage)
                 );

        return (dynamic) errors;
    }
}
