namespace TaskManagement.API.Responses
{
    internal class ValidationErrorReponse
    {
        public bool Success { get; set; } = false;
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }=new List<string>();

    }
}
