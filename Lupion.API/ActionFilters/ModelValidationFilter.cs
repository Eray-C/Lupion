using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lupion.API.ActionFilters;

public class ModelValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            var validator = serviceProvider.GetService(validatorType);

            if (validator == null) continue;

            var method = validatorType.GetMethod("ValidateAsync", [argument.GetType(), typeof(CancellationToken)]);
            if (method == null) continue;

            var task = (Task)method.Invoke(validator, [argument, CancellationToken.None]);
            await task.ConfigureAwait(false);

            var resultProperty = task.GetType().GetProperty("Result");
            var validationResult = resultProperty.GetValue(task);

            var isValidProperty = validationResult.GetType().GetProperty("IsValid");
            bool isValid = (bool)isValidProperty.GetValue(validationResult);

            if (!isValid)
            {
                var errorsProperty = validationResult.GetType().GetProperty("Errors");
                var errors = (IEnumerable<ValidationFailure>)errorsProperty.GetValue(validationResult);

                var badRequestResult = new
                {
                    Success = false,
                    ValidationErrors = errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                };

                context.Result = new BadRequestObjectResult(badRequestResult);
                return;
            }
        }

        await next();
    }
}
