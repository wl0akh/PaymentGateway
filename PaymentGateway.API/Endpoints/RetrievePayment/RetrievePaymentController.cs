using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Endpoints.Queries;

namespace PaymentGateway.API.Endpoints.RetrievePayment
{
    [ApiController]
    public class RetrievePaymentController : ControllerBase
    {
        private readonly IRetrievePaymentQuery _retrievePaymentQuery;
        private readonly ILogger<RetrievePaymentController> _logger;
        public RetrievePaymentController(IRetrievePaymentQuery retrievePaymentQuery, ILogger<RetrievePaymentController> logger)
        {
            this._logger = logger;
            this._retrievePaymentQuery = retrievePaymentQuery;
        }

        [HttpGet("/api/payments/{PaymentId}")]
        public async Task<ActionResult<RetrievePaymentResponse>> RetrievePayment(RetrievePaymentRequest retrievePaymentRequest)
        {
            return await _retrievePaymentQuery.ExecuteAsync(retrievePaymentRequest);
        }
    }
}
