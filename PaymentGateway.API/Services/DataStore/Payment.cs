using System;

namespace PaymentGateway.Services.DataStore
{
    public class Payment
    {

        private string _currency;
        public Guid PaymentId { get; set; }
        public string CardNumber { get; set; }
        public string Status { get; set; }
        public string Expiry { get; set; }
        public decimal? Amount { get; set; }
        public string Currency
        {
            get => String.IsNullOrEmpty(_currency) ? "GBP" : _currency;
            set => _currency = value;
        }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}