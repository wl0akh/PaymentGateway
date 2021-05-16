using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Endpoints.RetrievePayment;
using PaymentGateway.API.Services;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Services;

namespace PaymentGateway.API.Endpoints.Queries
{
    /// <summary>
    /// RetrievePaymentQuery implements IRetrievePaymentQuery to encapsulate Retrieval of Payment
    /// </summary>
    public class RetrievePaymentQuery : IRetrievePaymentQuery
    {

        private readonly ILogger<RetrievePaymentController> _logger;
        private readonly DataStoreDbContext _dataStoreDbContext;
        private readonly RequestTrackingService _requestTrackingService;

        public RetrievePaymentQuery(
            RequestTrackingService requestTrackingService,
            ILogger<RetrievePaymentController> logger,
            DataStoreDbContext dataStoreDbContext
        )
        {
            this._requestTrackingService = requestTrackingService;
            this._logger = logger;
            this._dataStoreDbContext = dataStoreDbContext;
        }

        /// <summary>
        /// ExecuteAsync to execute and retrieve payment
        /// </summary>
        /// <param name="retrievePaymentRequest"></param>
        /// <returns>Task<ActionResult<RetrievePaymentResponse>></returns>
        public async Task<ActionResult<RetrievePaymentResponse>> ExecuteAsync(RetrievePaymentRequest retrievePaymentRequest)
        {
            var payment = await this._dataStoreDbContext.Payments
                            .Where(r => r.PaymentId == retrievePaymentRequest.PaymentId)
                            .FirstOrDefaultAsync<Payment>();

            if (payment is null)
            {
                this._logger.LogWarning($@"RequestId:{this._requestTrackingService.RequestTraceId} 
                Payment details Not Found in DB for PaymentId:{retrievePaymentRequest.PaymentId}");

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
            var response = new RetrievePaymentResponse
            {
                PaymentId = payment.PaymentId,
                CardNumber = payment.CardNumber,
                Status = payment.PaymentStatus,
                Expiry = payment.Expiry,
                Amount = (decimal)payment.Amount,
                Currency = payment.Currency
            };
            return new OkObjectResult(response);
        }
    }
}