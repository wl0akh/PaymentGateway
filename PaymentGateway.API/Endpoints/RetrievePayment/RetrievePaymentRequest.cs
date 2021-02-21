using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace PaymentGateway.API.Endpoints.RetrievePayment
{
    /// <summary>
    /// RetrievePaymentRequest to encapsulate http retrive payment request 
    /// </summary>
    public class RetrievePaymentRequest
    {

        [FromRoute]
        [Required]
        public Guid PaymentId { get; set; }
    }
}