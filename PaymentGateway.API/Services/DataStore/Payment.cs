using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway.Services.DataStore
{
    public class Payment
    {

        private string _currency;
        public Guid paymentId { get; set; }
        public string cardNumber { get; set; }
        public string status { get; set; }
        public string expiry { get; set; }
        public decimal? amount { get; set; }
        public string currency
        {
            get => String.IsNullOrEmpty(_currency) ? "GBP" : _currency;
            set => _currency = value;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
    }
}