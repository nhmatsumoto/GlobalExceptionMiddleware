using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace NHMatsumoto_GlobalExceptionMiddleware_Demo
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                HandleException(context, ex);
            }
        }

        private void HandleException(HttpContext context, Exception exception)
        {
            _logger.LogError($"Erro não tratado: {exception}");

            var result = new
            {
                error = "Erro interno do servidor",
                message = exception.Message
            };

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }

    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
