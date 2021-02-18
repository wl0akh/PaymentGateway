namespace PaymentGateway.Services.Bank
{
    public class BankPaymentRequest
    {
        public string CardNumber { get; set; }
        public string Expiry { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CVV { get; set; }
    }
}