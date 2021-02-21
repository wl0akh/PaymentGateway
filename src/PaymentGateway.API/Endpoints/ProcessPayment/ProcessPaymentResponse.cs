using System;

namespace PaymentGateway.API.Endpoints.ProcessPayment
{
    /// <summary>
    /// ProcessPaymentResponse to encapsulate http process payment response body
    /// </summary>
    public class ProcessPaymentResponse
    {
        public Guid PaymentId { get; set; }
    }
}