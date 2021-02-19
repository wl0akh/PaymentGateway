using System;

namespace PaymentGateway.Services.Bank
{
    public class BankPaymentResponse
    {
        public Guid PaymentId { get; set; }
        public string Status { get; set; }
    }
}