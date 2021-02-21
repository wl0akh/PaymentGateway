using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Commands;
using PaymentGateway.API.Utils.Filters;

namespace PaymentGateway.API.Endpoints.ProcessPayment
{
    [ApiController]
    public class ProcessPaymentController : ControllerBase
    {

        private readonly ILogger<ProcessPaymentController> _logger;
        private readonly IPayoutCommand _payoutCommand;
        public ProcessPaymentController(IPayoutCommand _payoutCommand, ILogger<ProcessPaymentController> _logger)
        {
            this._logger = _logger;
            this._payoutCommand = _payoutCommand;
        }

        [Consumes("application/json")]
        [HttpPost("/api/payments")]
        [TrackingActionFilter]
        public async Task<ActionResult<ProcessPaymentResponse>> ProcessPaymentAsync(ProcessPaymentRequest paymentRequest)
        {
            return await _payoutCommand.ExecuteAsync(paymentRequest);
        }
    }
}
