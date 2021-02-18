using System;

namespace PaymentGateway.Utils.Exceptions
{
    public class BankServiceException : Exception
    {
        public string RequestTraceId { get; set; }
        public BankServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}