namespace ProductAPI.Common
{
    public record ApiResponse<T>(int StatusCode, bool Success, string Message, T? Data, string? Error = null)
    {
        public static ApiResponse<T> SuccessResponse(T data, string message = "Success", int statusCode = 200) => new(statusCode, true, message, data);

        public static ApiResponse<T> FailResponse(string message = "Failed", int statusCode = 400, string? error = null) => new(statusCode, false, message, default, error);
    }

}
