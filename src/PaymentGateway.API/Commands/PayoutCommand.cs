using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Endpoints;
using PaymentGateway.API.Endpoints.ProcessPayment;
using PaymentGateway.API.Services;
using PaymentGateway.Services.Bank;
using PaymentGateway.Services.DataStore;
using PaymentGateway.Utils.Exceptions;

namespace PaymentGateway.API.Commands
{
    /// <summary>
    /// PayoutCommand to implement  IPayoutCommand to encapsulate process of Payment
    /// </summary>
    public class PayoutCommand : IPayoutCommand
    {
        private readonly IDataStoreService _dataStore;
        private readonly IBankService _bank;
        private readonly ILogger<PayoutCommand> _logger;
        private readonly RequestTrackingService _requestTrackingService;

        public PayoutCommand(
            IDataStoreService dataStore,
            IBankService bank,
            ILogger<PayoutCommand> logger,
            RequestTrackingService requestTrackingService
        )
        {
            this._dataStore = dataStore;
            this._bank = bank;
            this._logger = logger;
            this._requestTrackingService = requestTrackingService;
        }

        /// <summary>
        /// ExecuteAsync to execute and process payment
        /// </summary>
        /// <param name="processPaymentRequest"></param>
        /// <returns>Task<ActionResult<ProcessPaymentResponse>></returns>
        public async Task<ActionResult<ProcessPaymentResponse>> ExecuteAsync(ProcessPaymentRequest paymentRequest)
        {
            try
            {
                var bankResponse = await this._bank.PayOutAsync(BankPayOutRequest.FromPaymentRequest(paymentRequest.paymentRequestBody));

                this._logger.LogDebug($@"RequestId:{this._requestTrackingService.RequestTraceId} 
                Bank Payout finished with status:{bankResponse.Status}");

                var newPaymentId = bankResponse.PaymentId;
                await this._dataStore.CreateAsync(Payment.FromPaymentRequest(paymentRequest.paymentRequestBody, bankResponse));

                this._logger.LogDebug($@"RequestId:{this._requestTrackingService.RequestTraceId} 
                Payment stored in DB with PaymentId:{newPaymentId}");

                return new CreatedResult($"/api/payments/{newPaymentId}", new ProcessPaymentResponse { PaymentId = newPaymentId });
            }
            catch (BankServiceException ex)
            {
                this._logger.LogError($@"RequestId:{this._requestTrackingService.RequestTraceId} 
                Bank Service Exception", ex);

                //GENERATE StandardErrorResponse with request tracking id
                var errorResponse = new StandardErrorResponse
                {
                    Type = ex.GetType().ToString(),
                    Error = ex.Message,
                    RequestTraceId = this._requestTrackingService.RequestTraceId.ToString()
                };

                return new ObjectResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.ServiceUnavailable
                };
            }
        }
    }
}