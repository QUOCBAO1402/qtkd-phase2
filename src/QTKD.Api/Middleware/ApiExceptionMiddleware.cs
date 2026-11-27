using System.Text.Json;

namespace QTKD.Api.Middleware;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ApiExceptionMiddleware> logger;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted) throw;

            int statusCode = exception switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                InvalidOperationException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            string message = statusCode == StatusCodes.Status500InternalServerError
                ? "Máy chủ gặp lỗi khi xử lý yêu cầu. Vui lòng kiểm tra kết nối PostgreSQL và thử lại."
                : exception.Message;

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                logger.LogError(exception, "Lỗi chưa được xử lý khi gọi API QTKD.");
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json; charset=utf-8";

            string payload = JsonSerializer.Serialize(new
            {
                statusCode,
                message
            });

            await context.Response.WriteAsync(payload);
        }
    }
}
