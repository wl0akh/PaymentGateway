using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Endpoints.Queries;
using PaymentGateway.API.Utils.Filters;

namespace PaymentGateway.API.Endpoints.RetrievePayment
{
    /// <summary>
    /// Controller class for RetrievePayment
    /// </summary>
    [ApiController]
    public class RetrievePaymentController : ControllerBase
    {
        private readonly IRetrievePaymentQuery _retrievePaymentQuery;
        public RetrievePaymentController(IRetrievePaymentQuery retrievePaymentQuery)
        {
            this._retrievePaymentQuery = retrievePaymentQuery;
        }

        /// <summary>
        /// RetrievePayment to show payment by PaymentId
        /// </summary>
        /// <param name="retrievePaymentRequest"></param>
        /// <returns>Task<ActionResult<RetrievePaymentResponse>></returns>
        [HttpGet("/api/payments/{PaymentId}")]
        [Produces("application/json")]
        [TrackingActionFilter]
        public async Task<ActionResult<RetrievePaymentResponse>> RetrievePaymentAsync(RetrievePaymentRequest retrievePaymentRequest)
        {
            return await _retrievePaymentQuery.ExecuteAsync(retrievePaymentRequest);
        }
    }
}
