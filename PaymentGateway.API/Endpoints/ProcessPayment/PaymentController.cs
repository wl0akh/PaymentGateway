using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Commands;
using PaymentGateway.Services.Bank;
using PaymentGateway.Services.DataStore;
using PaymentGateway.Utils.Exceptions;

namespace PaymentGateway.API.Endpoints.ProcessPayment
{
    [ApiController]
    public class PaymentController : ControllerBase
    {

        private readonly ILogger<PaymentController> _logger;
        private readonly IPayoutCommand _payoutCommand;
        public PaymentController(IPayoutCommand _payoutCommand, ILogger<PaymentController> _logger)
        {
            this._logger = _logger;
            this._payoutCommand = _payoutCommand;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Route("/api/payments")]
        public async Task<ActionResult<PaymentResponse>> ProcessPaymentAsync(PaymentRequest paymentRequest)
        {
            return await _payoutCommand.ExecuteAsync(paymentRequest);
        }
    }
}
