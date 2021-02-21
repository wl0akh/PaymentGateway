using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace PaymentGateway.API.Endpoints.RetrievePayment
{
    public class RetrievePaymentRequest
    {

        [FromRoute]
        [Required]
        // [NonEmptyGuid]
        public Guid PaymentId { get; set; }
    }
}