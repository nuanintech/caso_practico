using System.Net;
using System.Text.Json;
using client_service.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace client_service.API.Middleware {
    public class ErrorHandlingMiddleware {
        #region Atributos
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;
        #endregion

        #region Constructores
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger) {
            this.next = next;
            this.logger = logger;
        }
        #endregion
        #region Métodos
        public async Task Invoke(HttpContext context) {
            try {
                await next(context);
            }
            catch (ValidationException ex) {
                logger.LogWarning(ex, "more2286: Error de validación en la solicitud.");

                // Si tu ValidationException tiene un diccionario de errores
                var errores = ex.Errors?.SelectMany(e => e.Value).ToArray() ?? new[] { ex.Message };

                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, "Se encontraron errores de validación.",errores);
            }
            catch (NotFoundException ex) {
                logger.LogWarning($"more2286: Recurso no encontrado: {ex.Message} | Ruta: {context.Request.Path}");
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, "Recurso no encontrado", new[] { ex.Message });
            }
            catch (ConflictException ex) {
                logger.LogWarning(ex, "more2286: Conflicto");
                await HandleExceptionAsync(context, HttpStatusCode.Conflict, "Dato inválido", new[] { ex.Message });
            }
            catch (Exception ex) {
                logger.LogError(ex, "more2286: Error inesperado");
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Error inesperado", new[] { "Ocurrió un error inesperado" });
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string title, IEnumerable<string> errors)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new {
                title,
                errors
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        #endregion
    }
}
