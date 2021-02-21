using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.API.Endpoints.ProcessPayment
{
    public class ProcessPaymentRequest
    {
        [FromBody]
        public PaymentRequestBody paymentRequestBody { get; set; }
    }
}