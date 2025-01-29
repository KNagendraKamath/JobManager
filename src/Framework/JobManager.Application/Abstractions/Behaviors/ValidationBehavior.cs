using FluentValidation;
using JobManager.Framework.Application.Abstractions.Exceptions;
using JobManager.Framework.Application.Abstractions.Messaging;
using MediatR;

namespace JobManager.Framework.Application.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);

        List<ValidationError> validationErrors = new List<ValidationError>();

        foreach (IValidator<TRequest> validator in _validators)
        {
            FluentValidation.Results.ValidationResult validationResult = await validator.ValidateAsync(context, cancellationToken);
            if (validationResult.Errors.Any())
            {
                validationErrors.AddRange(validationResult.Errors
                                                          .Select(validationFailure => 
                                                          new ValidationError(
                                                                validationFailure.PropertyName,
                                                                validationFailure.ErrorMessage)));
            }
        }

        if (validationErrors.Any())
            throw new Exceptions.ValidationException(validationErrors);

        return await next();
    }
}
