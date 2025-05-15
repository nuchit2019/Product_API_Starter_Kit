 
using ProductAPI.Common;
using System.Text.Json;
using System.Diagnostics;
using System.Text;

namespace ProductAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                await _next(context);
            }
            catch (Exception ex)
            {
                // Get Action Name from endpoint
                var endpoint = context.GetEndpoint();
                var actionName = endpoint?.DisplayName ?? "UnknownAction";

                // Read Request Body
                var requestBody = await ReadRequestBodyAsync(context.Request);

                // StackTrace
                var st = new StackTrace(ex, true);
                var frame = st.GetFrames()?.FirstOrDefault(f => f.GetFileLineNumber() > 0);
                var methodInfo = frame?.GetMethod();
                var declaringType = methodInfo?.DeclaringType;

                // Extract original method name (handle async state machine)
                var originalMethodName = declaringType?.Name;
                string methodName;
                if (!string.IsNullOrWhiteSpace(originalMethodName) && originalMethodName.Contains("<") && originalMethodName.Contains(">"))
                {
                    var start = originalMethodName.IndexOf("<") + 1;
                    var end = originalMethodName.IndexOf(">");
                    methodName = originalMethodName.Substring(start, end - start);
                }
                else
                {
                    methodName = methodInfo?.Name ?? "UnknownMethod";
                }

                // Get class name (real class, not compiler generated)
                var className = declaringType?.DeclaringType?.Name ?? declaringType?.Name ?? "UnknownClass";
                var lineNumber = frame?.GetFileLineNumber();

                // Compose Error Detail
                var errorDetail = $"Class: {className}, Method: {methodName}, Line: {lineNumber}, Action: {actionName}, Request: {requestBody}";

                // Log
                _logger.LogError(ex, "Exception occurred: {ErrorDetail}", errorDetail);

                // Return API JSON Response

                context.Items["ExceptionHandled"] = true;

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = ApiResponse<object>.FailResponse("Internal Server Error", 500, errorDetail);               

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            try
            {
                request.Body.Position = 0;
                using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                return body;
            }
            catch
            {
                return "Unable to read request body.";
            }
        }

    }
}

