namespace TaskManagement.API.Responses
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }

    }
}
