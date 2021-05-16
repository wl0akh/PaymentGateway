using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PaymentGateway.Domain.CustomValidationAttributes;

namespace PaymentGateway.Domain.Entities
{
    /// <summary>
    /// Payment class to Encapsulate payment  
    /// </summary>
    public class Payment
    {
        public enum Status { APPROVED, DECLINED }
        private string _currency;
        private Status _paymentStatus;
        private string _cvv;
        private decimal? _amount;
        private string _expiry;
        private string _cardNumber;
        private Guid _paymentId;

        public Payment(
         string currency,
         string cvv,
         decimal? amount,
         string expiry,
         string cardNumber
        )
        {
            this._currency = currency;
            this._cvv = cvv;
            this._amount = amount;
            this._expiry = expiry;
            this._cardNumber = cardNumber;
        }

        public Payment()
        {
        }

        public Guid PaymentId { get => _paymentId; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The cardNumber must not be empty")]
        [RegularExpression(@"^(\d{12,19})$", ErrorMessage = "The cardNumber must be of 12 to 19 digits")]
        public string CardNumber { get => _cardNumber; }

        [Required]
        public Status PaymentStatus { get => _paymentStatus; }

        [CardExpiryDateAttribute(ErrorMessage = "The expiry must be in future and in the formate: MM/YYYY")]
        public string Expiry { get => _expiry; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The amount must not be empty")]
        public decimal? Amount { get => _amount; }

        [RegularExpression(@"^([a-zA-Z]{3})$", ErrorMessage = "The currency must be 3 letter string")]
        public string Currency
        {
            get => String.IsNullOrEmpty(_currency) ? "GBP" : _currency;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The cvv must not be empty")]
        [RegularExpression(@"^(\d{3})$", ErrorMessage = "The cvv must 3 digits")]
        [NotMapped]
        public string CVV { get => _cvv; }

        public DateTime CreatedAt { get; } = DateTime.UtcNow;

        /// <summary>
        /// Method to set payment status Successful
        /// </summary>
        /// <returns></returns>
        public void Approve(Guid paymentId)
        {
            this._paymentId = paymentId;
            this._paymentStatus = Status.APPROVED;
        }

        /// <summary>
        /// Method to set payment status Decline
        /// </summary>
        /// <returns></returns>
        public void Decline(Guid paymentId)
        {
            this._paymentId = paymentId;
            this._paymentStatus = Status.DECLINED;
        }
    }
}