using Store.APIs.Errors;
using System.Text.Json;

namespace Store.APIs.MiddelWares
{
    public class ExceptionMiddelWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddelWare> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddelWare(RequestDelegate next , ILogger<ExceptionMiddelWare> logger , IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(Exception ex) 
            {
                logger.LogError(ex , ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = env.IsDevelopment() ?
                    new ApiExceptionResponse(StatusCodes.Status500InternalServerError, ex?.Message, ex?.StackTrace?.ToString())
                    : new ApiExceptionResponse(StatusCodes.Status500InternalServerError);

                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json =  JsonSerializer.Serialize(response , options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
