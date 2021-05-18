using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Services.Bank
{
    /// <summary>
    /// BankPayOutRequest to encapsulate payout request to bank 
    /// </summary>
    public class BankPayOutRequest
    {
        public string CardNumber { get; set; }
        public string Expiry { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; }
        public string CVV { get; set; }

        /// <summary>
        /// BankPayOutRequest to translate PaymentRequestBody to BankPayOutRequest
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public static BankPayOutRequest FromPayment(Payment payment)
        {
            return new BankPayOutRequest
            {
                CardNumber = payment.CardNumber,
                Expiry = payment.Expiry,
                Amount = payment.Amount,
                Currency = payment.Currency,
                CVV = payment.CVV
            };
        }
    }
}