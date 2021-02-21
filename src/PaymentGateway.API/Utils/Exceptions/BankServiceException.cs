using System;

namespace PaymentGateway.Utils.Exceptions
{
    /// <summary>
    /// BankServiceException class to encapsulate BankService errors 
    /// </summary>
    public class BankServiceException : Exception
    {
        public string RequestTraceId { get; set; }
        public BankServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}