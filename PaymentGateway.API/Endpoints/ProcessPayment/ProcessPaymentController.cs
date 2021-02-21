using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Commands;
using PaymentGateway.API.Utils.Filters;

namespace PaymentGateway.API.Endpoints.ProcessPayment
{
    /// <summary>
    /// controller class for ProcessPayment
    /// </summary>
    [ApiController]
    public class ProcessPaymentController : ControllerBase
    {

        private readonly IPayoutCommand _payoutCommand;
        public ProcessPaymentController(IPayoutCommand _payoutCommand)
        {
            this._payoutCommand = _payoutCommand;
        }

        /// <summary>
        /// ProcessPaymentAsync  to create payment with  Payment details
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        [Consumes("application/json")]
        [HttpPost("/api/payments")]
        [TrackingActionFilter]
        public async Task<ActionResult<ProcessPaymentResponse>> ProcessPaymentAsync(ProcessPaymentRequest paymentRequest)
        {
            return await _payoutCommand.ExecuteAsync(paymentRequest);
        }
    }
}
