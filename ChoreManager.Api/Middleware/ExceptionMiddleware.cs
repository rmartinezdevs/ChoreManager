using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace ChoreManager.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync (HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException validationEx)
            {
                _logger.LogError(validationEx, "Error en la validacion de los campos en chore");
                await HandleValidationException(context, validationEx);
            }
            catch (Exception generalEx)
            {
                _logger.LogError(generalEx, "Error inesperado");
                await HandleExceptionAsync(context, generalEx);
            }
        }

        private async Task HandleValidationException(HttpContext context, FluentValidation.ValidationException ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

            var errors = ex.Errors.Select(e => new
            {
                Field = e.PropertyName,
                Error = e.ErrorMessage
            });

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Error de validación",
                Details = errors
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Error inesperado",
                Details = ex.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
