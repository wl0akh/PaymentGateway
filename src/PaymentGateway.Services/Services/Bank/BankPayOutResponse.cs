using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static PaymentGateway.Domain.Entities.Payment;

namespace PaymentGateway.Services.Bank
{
    /// <summary>
    /// BankPayOutResponse to encapsulate payout response from bank 
    /// </summary>
    public class BankPayOutResponse
    {
        public Guid PaymentId { get; set; }
        [JsonProperty("PaymentStatus")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Status PaymentStatus { get; set; }
    }
}