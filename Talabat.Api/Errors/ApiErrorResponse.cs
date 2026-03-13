namespace Store.APIs.Errors
{
    public class ApiErrorResponse
    {
        public ApiErrorResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }

        private string? GetDefaultMessageForStatusCode (int statusCode)
        {
            var message = statusCode switch
            {
                400 => "a bad request , you have made",
                401 => "Authorized , you r not",
                404 => "Resource was not Found",
                500 => "Server Error",
                _ => null
            };

            return message;
        }
    }
}
