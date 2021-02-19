using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Services.Bank;
using PaymentGateway.Services.DataStore;
using PaymentGateway.Utils.Exceptions;

namespace PaymentGateway.API.Endpoints.ProcessPayment
{
    [ApiController]
    public class ProcessPaymentController : ControllerBase
    {
        private readonly IDataStoreService _dataStoreService;
        private readonly IBankService _bankService;
        private readonly ILogger<ProcessPaymentController> _logger;

        public ProcessPaymentController(IBankService _bankService, IDataStoreService dataStoreService, ILogger<ProcessPaymentController> _logger)
        {
            this._logger = _logger;
            this._bankService = _bankService;
            this._dataStoreService = dataStoreService;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Route("/api/payments")]
        public async Task<ActionResult<ProcessPaymentResponse>> ProcessPaymentAsync(ProcessPaymentRequest paymentRequest)
        {
            try
            {
                var bankServiceResponse = await this._bankService.ExecutePaymentAsync(paymentRequest.ToBankPaymentRequest());
                var newPaymentId = bankServiceResponse.PaymentId;
                await this._dataStoreService.CreateAsync(paymentRequest.ToPayment(newPaymentId, bankServiceResponse.Status));
                return Created($"/api/payments/{newPaymentId}", new ProcessPaymentResponse { PaymentId = newPaymentId });
            }
            catch (BankServiceException ex)
            {
                _logger.LogError("Bank Service", ex);
                var result = new ObjectResult(new StandardErrorResponse(ex));
                result.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                return result;
            }
        }

    }
}
