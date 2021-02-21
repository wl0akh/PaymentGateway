using System;

namespace PaymentGateway.API.Endpoints.RetrievePayment
{
    public class RetrievePaymentResponse
    {
        public Guid PaymentId { get; set; }
        public string CardNumber { get; set; }
        public string Status { get; set; }
        public string Expiry { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}