using System.Net;

namespace NewProj.API.Middlewares
{
    public class ExceptionHandlerMiddlware
    {
        private readonly ILogger<ExceptionHandlerMiddlware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddlware(ILogger<ExceptionHandlerMiddlware> logger,
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        // Method um all Exception mit diese Method zu registrieren
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex) 
            {
                var errorId = Guid.NewGuid();
                // Log this Exception
                logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return Error Response (code 500 zuruck)
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Etwas schief gegangen"
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
