namespace PaymentGateway.API.Endpoints.ProcessPayment
{
    /// <summary>
    /// PaymentRequestBody class to encapsulate payment details from http request body
    /// </summary>
    public class PaymentRequestBody
    {
        public string CardNumber { get; set; }
        public string CVV { get; set; }

        public string Expiry { get; set; }

        public decimal? Amount { get; set; }

        public string Currency { get; set; }

    }
}