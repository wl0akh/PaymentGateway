using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Endpoints.RetrievePayment;
using PaymentGateway.Services.DataStore;

namespace PaymentGateway.API.Endpoints.Queries
{
    public class RetrievePaymentQuery : IRetrievePaymentQuery
    {

        private readonly ILogger<RetrievePaymentController> _logger;
        private readonly IDataStoreService _dataStore;

        public RetrievePaymentQuery(ILogger<RetrievePaymentController> logger, IDataStoreService dataStore)
        {
            _logger = logger;
            _dataStore = dataStore;
        }

        public async Task<ActionResult<RetrievePaymentResponse>> ExecuteAsync(RetrievePaymentRequest retrievePaymentRequest)
        {
            var payment = await this._dataStore.GetPaymentAsync(retrievePaymentRequest.PaymentId);
            if (payment is null)
            {
                return new NotFoundObjectResult(new StandardErrorResponse
                {
                    Type = HttpStatusCode.NotFound.ToString(),
                    RequestTraceId = "gsdghfjg",
                    Error = $"Not Record found for PaymentId : {retrievePaymentRequest.PaymentId}"
                });
            }

            return new OkObjectResult(payment.ToRetrievePaymentResponse());
        }
    }
}