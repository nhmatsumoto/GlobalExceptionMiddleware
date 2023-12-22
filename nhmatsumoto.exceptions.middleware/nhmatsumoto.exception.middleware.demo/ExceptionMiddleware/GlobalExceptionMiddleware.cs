using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

namespace nhmatsumoto.exception.middleware.demo.ExceptionMiddleware
{
    public class GlobalExceptionMiddleware : IExceptionFilter
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError($"Erro não tratado: {context.Exception.InnerException}");
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "Internal Server Error";
           
            if (exception is ApiException apiException)
            {
                context.Response.StatusCode = apiException.StatusCode;
                message = JsonSerializer.Serialize(new { apiException.StatusCode, apiException.Errors });
            }

            _logger.LogError(message, context.Response);

            return context.Response.WriteAsync(message);
        }
    }
}

