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

                //await HandleExceptionAsync(context, ex);
            }
        }
    }
}
