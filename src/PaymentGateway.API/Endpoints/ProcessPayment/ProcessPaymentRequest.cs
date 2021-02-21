using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.API.Endpoints.ProcessPayment
{
    /// <summary>
    /// ProcessPaymentRequest to encapsulate http process payment request body
    /// </summary>
    public class ProcessPaymentRequest
    {
        [FromBody]
        public PaymentRequestBody paymentRequestBody { get; set; }
    }
}