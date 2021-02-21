using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Endpoints.RetrievePayment;
using PaymentGateway.API.Services;
using PaymentGateway.Services.DataStore;

namespace PaymentGateway.API.Endpoints.Queries
{
    /// <summary>
    /// RetrievePaymentQuery implements IRetrievePaymentQuery to encapsulate Retrieval of Payment
    /// </summary>
    public class RetrievePaymentQuery : IRetrievePaymentQuery
    {

        private readonly ILogger<RetrievePaymentController> _logger;
        private readonly IDataStoreService _dataStore;
        private readonly RequestTrackingService _requestTrackingService;

        public RetrievePaymentQuery(
            RequestTrackingService requestTrackingService,
            ILogger<RetrievePaymentController> logger,
            IDataStoreService dataStore
        )
        {
            this._requestTrackingService = requestTrackingService;
            this._logger = logger;
            this._dataStore = dataStore;
        }

        /// <summary>
        /// ExecuteAsync to execute and retrieve payment
        /// </summary>
        /// <param name="retrievePaymentRequest"></param>
        /// <returns>Task<ActionResult<RetrievePaymentResponse>></returns>
        public async Task<ActionResult<RetrievePaymentResponse>> ExecuteAsync(RetrievePaymentRequest retrievePaymentRequest)
        {
            var payment = await this._dataStore.GetPaymentAsync(retrievePaymentRequest.PaymentId);
            if (payment is null)
            {
                this._logger.LogWarning($@"RequestId:{this._requestTrackingService.RequestTraceId} 
                Payment details Not Found in DB for PaymentId:{retrievePaymentRequest.PaymentId}");

                //Generate StandardErrorResponse with request tracking id
                var errorResponse = new StandardErrorResponse
                {
                    Type = HttpStatusCode.NotFound.ToString(),
                    RequestTraceId = this._requestTrackingService.RequestTraceId.ToString(),
                    Error = $"No Record found for PaymentId : {retrievePaymentRequest.PaymentId}"
                };

                return new NotFoundObjectResult(errorResponse);
            }

            this._logger.LogDebug($@"RequestId:{this._requestTrackingService.RequestTraceId} 
            Payment details retrived from DataStore for PaymentId:{payment.PaymentId}");

            return new OkObjectResult(payment.ToRetrievePaymentResponse());
        }
    }
}