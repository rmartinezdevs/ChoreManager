namespace ChoreManager.Api.Common
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Details { get; set; }
    }
}
