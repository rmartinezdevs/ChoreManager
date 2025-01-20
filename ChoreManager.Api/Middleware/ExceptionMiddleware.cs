using ChoreManager.Api.Common;
using FluentValidation;
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
            catch (ValidationException validationEx)
            {
                _logger.LogWarning(validationEx, "Validation error occurred.");
                await HandleExceptionAsync(context, validationEx, HttpStatusCode.BadRequest);
            }
            catch (ArgumentException argEx)
            {
                _logger.LogWarning(argEx, "Argument exception occurred.");
                await HandleExceptionAsync(context, argEx, HttpStatusCode.BadRequest);
            }
            catch (InvalidOperationException opEx)
            {
                _logger.LogWarning(opEx, "Invalid operation exception occurred.");
                await HandleExceptionAsync(context, opEx, HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
        {
            var response = new ErrorResponse
            {
                StatusCode = (int) statusCode,
                Message = ex.Message,
                Details = ex.InnerException?.Message
            };
            await WriteResponseAsync(context, response, statusCode);
        }

        private async Task WriteResponseAsync(HttpContext context, ErrorResponse response, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
