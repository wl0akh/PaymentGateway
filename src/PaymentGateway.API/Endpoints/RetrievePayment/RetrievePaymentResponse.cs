using System;
using PaymentGateway.Utils.Helpers;

namespace PaymentGateway.API.Endpoints.RetrievePayment
{
    /// <summary>
    /// RetrievePaymentResponse to encapsulate http payment response body
    /// </summary>
    public class RetrievePaymentResponse
    {
        private string _cardNumber;
        public Guid PaymentId { get; set; }
        public string CardNumber
        {
            //Mask cardnumber on get
            get => MaskHelper.Mask(_cardNumber);
            set => _cardNumber = value;
        }
        public string Status { get; set; }
        public string Expiry { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}