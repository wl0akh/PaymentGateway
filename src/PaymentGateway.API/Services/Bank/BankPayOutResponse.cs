using System;

namespace PaymentGateway.Services.Bank
{
    /// <summary>
    /// BankPayOutResponse to encapsulate payout response from bank 
    /// </summary>
    public class BankPayOutResponse
    {
        public Guid PaymentId { get; set; }
        public string Status { get; set; }
    }
}