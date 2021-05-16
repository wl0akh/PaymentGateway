using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Endpoints;
using PaymentGateway.API.Endpoints.ProcessPayment;
using PaymentGateway.API.Services;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Services;
using PaymentGateway.Services.Bank;
using PaymentGateway.Utils.Exceptions;
using PaymentGateway.Utils.Helpers;

namespace PaymentGateway.API.Commands
{
    /// <summary>
    /// PayoutCommand to implement  IPayoutCommand to encapsulate process of Payment
    /// </summary>
    public class PayoutCommand : IPayoutCommand
    {
        private readonly IBankService _bank;
        private readonly ILogger<PayoutCommand> _logger;
        private readonly RequestTrackingService _requestTrackingService;
        private DataStoreDbContext _dataStoreDbContext;

        public PayoutCommand(
            IBankService bank,
            DataStoreDbContext dataStoreDbContext,
            ILogger<PayoutCommand> logger,
            RequestTrackingService requestTrackingService
        )
        {
            this._bank = bank;
            this._logger = logger;
            this._requestTrackingService = requestTrackingService;
            this._dataStoreDbContext = dataStoreDbContext;
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
                var payment = new Payment(
                                paymentRequest.paymentRequestBody.Currency,
                                paymentRequest.paymentRequestBody.CVV,
                                paymentRequest.paymentRequestBody.Amount,
                                paymentRequest.paymentRequestBody.Expiry,
                                paymentRequest.paymentRequestBody.CardNumber
                                );
                var validationHelper = new ValidationHelper();
                if (!validationHelper.isValid(payment))
                {
                    var errorResponse = new StandardErrorResponse
                    {
                        Type = HttpStatusCode.BadRequest.ToString(),
                        RequestTraceId = this._requestTrackingService.RequestTraceId.ToString(),
                        Error = $"Invalid Payment request : {JsonSerializer.Serialize(validationHelper.Error)}"
                    };

                    return new BadRequestObjectResult(errorResponse);
                }

                var bankResponse = await this._bank.PayOutAsync(payment);
                if (bankResponse.PaymentStatus == Payment.Status.APPROVED)
                {
                    payment.Approve(bankResponse.PaymentId);
                }

                if (bankResponse.PaymentStatus == Payment.Status.DECLINED)
                {
                    payment.Decline(bankResponse.PaymentId);
                }

                using (var dbContext = this._dataStoreDbContext)
                {
                    var createdPayment = dbContext.Payments.Add(payment);
                    await dbContext.SaveChangesAsync();
                    this._logger.LogDebug($@"RequestId:{this._requestTrackingService.RequestTraceId} 
                        Payment stored in DB with PaymentId:{payment.PaymentId}");
                }

                return new CreatedResult($"/api/payments/{payment.PaymentId}", new ProcessPaymentResponse { PaymentId = payment.PaymentId });
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