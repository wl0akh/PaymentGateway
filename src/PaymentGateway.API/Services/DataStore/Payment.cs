using System;
using System.ComponentModel.DataAnnotations;
using PaymentGateway.API.Endpoints.ProcessPayment;
using PaymentGateway.API.Endpoints.RetrievePayment;
using PaymentGateway.Services.Bank;

namespace PaymentGateway.Services.DataStore
{
    /// <summary>
    /// Payment class to Encapsulate payment  
    /// </summary>
    public class Payment
    {

        private string _currency;

        [Required]
        public Guid PaymentId { get; set; }

        [Required]
        [MaxLength(19)]
        public string CardNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string Status { get; set; }

        [Required]
        [MaxLength(19)]
        public string Expiry { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [MaxLength(3)]
        public string Currency
        {
            get => String.IsNullOrEmpty(_currency) ? "GBP" : _currency;
            set => _currency = value;
        }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// method to Translate PaymentRequestBody to  Payment
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <param name="bankResponse"></param>
        /// <returns></returns>
        public static Payment FromPaymentRequest(PaymentRequestBody paymentRequest, BankPayOutResponse bankResponse)
        {
            return new Payment
            {
                PaymentId = bankResponse.PaymentId,
                CardNumber = paymentRequest.CardNumber,
                Status = bankResponse.Status,
                Expiry = paymentRequest.Expiry,
                Amount = (decimal)paymentRequest.Amount,
                Currency = paymentRequest.Currency
            };
        }

        /// <summary>
        /// Method to Translate to RetrievePaymentResponse from Payment
        /// </summary>
        /// <returns></returns>
        public RetrievePaymentResponse ToRetrievePaymentResponse()
        {
            return new RetrievePaymentResponse
            {
                PaymentId = this.PaymentId,
                CardNumber = this.CardNumber,
                Status = this.Status,
                Expiry = this.Expiry,
                Amount = (decimal)this.Amount,
                Currency = this.Currency
            };
        }
    }
}