using System.Net;
using System.Text.Json;
using UniLMS.Application.DTOs.Errors;

namespace UniLMS.WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger; 
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch(Exception ex) 
            {
                _logger.LogError(ex, "Gözlənilməz xəta baş verdi: {Message}", ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponseDTO();

            switch (ex)
            {
                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = ex.Message;
                    break;

                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = ex.Message;
                    break;

                case ArgumentException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = ex.Message;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = ex.Message;
                    break;
                
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase

            };

            var jsonRespone = JsonSerializer.Serialize(response, jsonOptions);

            return context.Response.WriteAsync(jsonRespone);
        }
    }
}
