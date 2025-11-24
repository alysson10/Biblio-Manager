using FluentValidation;

namespace Bib.WebAPI.Filters
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        private readonly IValidator<T>? _validator;

        public ValidationFilter(IValidator<T>? validator = null)
        {
            _validator = validator;
        }

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var argument = context.Arguments
                .OfType<T>()
                .FirstOrDefault();

            if (argument is null)
            {
                return Results.BadRequest($"Unable to find parameter of type {typeof(T).Name}");
            }

            if (_validator is null)
            {
                return await next(context);
            }

            var validationResult = await _validator.ValidateAsync(argument);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );

                return Results.ValidationProblem(
                    errors: errors,
                    detail: "One or more validation errors occurred",
                    title: "Validation Failed",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            return await next(context);
        }
    }
}
