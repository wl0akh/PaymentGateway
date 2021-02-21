using System;
using PaymentGateway.API.Endpoints.ProcessPayment;

namespace PaymentGateway.Services.Bank
{
    public class BankPayOutRequest
    {
        public string CardNumber { get; set; }
        public string Expiry { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public string CVV { get; set; }

        public static BankPayOutRequest FromPaymentRequest(PaymentRequestBody paymentRequest)
        {
            return new BankPayOutRequest
            {
                CardNumber = paymentRequest.CardNumber,
                Expiry = paymentRequest.Expiry,
                Amount = paymentRequest.Amount,
                Currency = paymentRequest.Currency,
                CVV = paymentRequest.CVV
            };
        }
    }
}