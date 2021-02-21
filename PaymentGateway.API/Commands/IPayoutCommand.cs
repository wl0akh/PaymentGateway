using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Endpoints.ProcessPayment;

namespace PaymentGateway.API.Commands
{
    public interface IPayoutCommand
    {
        Task<ActionResult<ProcessPaymentResponse>> ExecuteAsync(ProcessPaymentRequest paymentRequest);
    }
}