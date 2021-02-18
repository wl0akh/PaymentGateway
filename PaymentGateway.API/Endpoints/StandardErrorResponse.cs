using System;
using PaymentGateway.Utils.Exceptions;

namespace PaymentGateway.API.Endpoints
{
    public class StandardErrorResponse
    {
        public StandardErrorResponse(Exception ex)
        {
            if (ex is BankServiceException) RequestTraceId = ((BankServiceException)ex).RequestTraceId;
            Type = ex.GetType().ToString();
            Error = ex.Message;
        }

        public StandardErrorResponse()
        {
        }

        public string RequestTraceId { get; set; }
        public string Type { get; set; }
        public string Error { get; set; }
    }
}