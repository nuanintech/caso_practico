﻿using FluentValidation;
using MediatR;

namespace task_service.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request,RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken) {
            if (!validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count > 0) {
                var errorDict = failures
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                throw new Domain.Exceptions.ValidationException("Errores de validacion", errorDict);
            }

            return await next();
        }
    }
}
