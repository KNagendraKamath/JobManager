namespace JobManager.Framework.Application.Abstractions.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationError> errors) => Errors = errors;

    public IEnumerable<ValidationError> Errors { get; }

    public static string GetExceptionDetails(Exception exception)
    {
        return System.Text.Json.JsonSerializer.Serialize( exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                400,
                "ValidationFailure",
                "Validation error",
                "One or more validation errors has occurred",
                validationException.Errors),
            Exception ex => new ExceptionDetails(
                500,
                "ServerError",
                "Server error",
                "An unexpected error has occurred",
                [ex.Message])
        });
    }

    public sealed record ExceptionDetails(
        int Status,
        string Type,
        string Title,
        string Detail,
        IEnumerable<object>? Errors);
}
