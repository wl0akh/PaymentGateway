using System;

namespace PaymentGateway.Services.Bank
{
    public class BankPaymentResponse
    {
        public Guid paymentId { get; set; }
        public string status { get; set; }
    }
}