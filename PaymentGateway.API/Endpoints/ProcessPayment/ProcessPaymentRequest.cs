using System;
using System.ComponentModel.DataAnnotations;
using PaymentGateway.Services.Bank;
using PaymentGateway.Services.DataStore;
using PaymentGateway.Utils.CustomValidationAttributes;

namespace PaymentGateway.API.Endpoints.ProcessPayment
{
    public class ProcessPaymentRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The cardNumber must not be empty")]
        [RegularExpression(@"^(\d{12,19})$", ErrorMessage = "The cardNumber must be of 12 to 19 digits")]
        public string cardNumber { get; set; }


        [CreditCardExpiryDateAttribute(ErrorMessage = "The expiry must be in future and in the formate: MM/YYYY")]
        public string expiry { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The amount must not be empty")]
        public decimal? amount { get; set; }

        [RegularExpression(@"^([a-zA-Z]{3})$", ErrorMessage = "The currency must be 3 letter string")]
        public string currency { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The cvv must not be empty")]
        [RegularExpression(@"^(\d{3})$", ErrorMessage = "The cvv must 3 digits")]
        public string cvv { get; set; }

        public BankPaymentRequest ToBankPaymentRequest()
        {
            return new BankPaymentRequest
            {
                CardNumber = this.cardNumber,
                Expiry = this.expiry,
                Amount = (decimal)this.amount,
                Currency = this.currency,
                CVV = this.cvv

            };
        }
        public Payment ToPayment(Guid newPaymentId, string bankPaymentStatus)
        {
            return new Payment
            {
                paymentId = newPaymentId,
                status = bankPaymentStatus,
                cardNumber = this.cardNumber,
                expiry = this.expiry,
                amount = (decimal)this.amount,
                currency = this.currency
            };
        }
    }
}