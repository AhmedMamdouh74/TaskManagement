namespace TaskManagement.API.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public ApiResponse() { }
        public ApiResponse(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        public static ApiResponse<T> CreateSuccessResponse(T data, string message = "Request was successful.")
        => new ApiResponse<T>(true, message, data);

        public static ApiResponse<T> CreateErrorResponse(string message)

        => new ApiResponse<T>(false, message);

    }
}
