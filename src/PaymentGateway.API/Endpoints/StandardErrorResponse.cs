namespace PaymentGateway.API.Endpoints
{
    /// <summary>
    /// StandardErrorResponse to encapsulate and error as http response body
    /// </summary>
    public class StandardErrorResponse
    {
        public string RequestTraceId { get; set; }
        public string Type { get; set; }
        public string Error { get; set; }
    }
}