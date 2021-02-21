using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Endpoints.ProcessPayment;

namespace PaymentGateway.API.Commands
{

    /// <summary>
    /// IPayoutCommand to encapsulate process of Payment
    /// </summary>
    public interface IPayoutCommand
    {
        /// <summary>
        /// ExecuteAsync to execute and process payment
        /// </summary>
        /// <param name="processPaymentRequest"></param>
        /// <returns>Task<ActionResult<ProcessPaymentResponse>></returns>
        Task<ActionResult<ProcessPaymentResponse>> ExecuteAsync(ProcessPaymentRequest paymentRequest);
    }
}