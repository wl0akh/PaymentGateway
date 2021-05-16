using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Commands;
using PaymentGateway.API.Services;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Utils.Helpers;
using System.Text.Json;
using System.Net;
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
        private readonly RequestTrackingService _requestTrackingService;
        public ProcessPaymentController(IPayoutCommand payoutCommand,  RequestTrackingService requestTrackingService)
        {
            this._payoutCommand = payoutCommand;
            this._requestTrackingService = requestTrackingService;
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
            var payment = new Payment(
                                paymentRequest.paymentRequestBody.Currency,
                                paymentRequest.paymentRequestBody.CVV,
                                paymentRequest.paymentRequestBody.Amount,
                                paymentRequest.paymentRequestBody.Expiry,
                                paymentRequest.paymentRequestBody.CardNumber
                                );
            var validationHelper = new ValidationHelper();
            if (!validationHelper.isValid(payment)){
                var errorResponse = new StandardErrorResponse
                    {
                        Type = HttpStatusCode.BadRequest.ToString(),
                        RequestTraceId = this._requestTrackingService.RequestTraceId.ToString(),
                        Error = $"Invalid Payment request : {JsonSerializer.Serialize(validationHelper.Error)}"
                    };

                return new BadRequestObjectResult(errorResponse);
            }

            return await _payoutCommand.ExecuteAsync(payment);
        }
    }
}
