using System;

namespace PaymentGateway.Services.Bank
{
    public class BankPayOutResponse
    {
        public Guid PaymentId { get; set; }
        public string Status { get; set; }
    }
}