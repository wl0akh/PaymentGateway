using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Endpoints;
using PaymentGateway.API.Endpoints.ProcessPayment;
using PaymentGateway.API.Services;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Services;
using PaymentGateway.Services.Bank;
using PaymentGateway.Services.Bank.Exceptions;

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
        /// <param name="payment"></param>
        /// <returns>Task<ActionResult<ProcessPaymentResponse>></returns>
        public async Task<ActionResult<ProcessPaymentResponse>> ExecuteAsync(Payment payment)
        {
            try
            {
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