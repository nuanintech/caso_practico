using client_service.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace client_service.Shared.Validator.FilterValidator {
    public class ModelStateValidationFilter : IActionFilter {
        public void OnActionExecuting(ActionExecutingContext context) {
            if (!context.ModelState.IsValid) {
                var errores = context.ModelState
                    .Where(ms => ms.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new ValidationException("Error de validación", errores);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) {}
    }
}
