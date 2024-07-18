namespace Order_Management_System.Errors
{
    public class ApiResponse
    {

        public int? StatusCode { get; set; }
        public string? Message { get; set; }



        public ApiResponse(int? statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int? statusCode)
        {
            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "You Are Not Authorized",
                403 => "You do not have permission.",
                500 => "Internal Server Error ",
                404 => "Resource Not Found",
                _ => null
            };
        }
    }
}
