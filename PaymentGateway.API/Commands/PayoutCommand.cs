using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Endpoints;
using PaymentGateway.API.Endpoints.ProcessPayment;
using PaymentGateway.Services.Bank;
using PaymentGateway.Services.DataStore;
using PaymentGateway.Utils.Exceptions;

namespace PaymentGateway.API.Commands
{
    public class PayoutCommand : IPayoutCommand
    {
        private readonly IDataStoreService _dataStore;
        private readonly IBankService _bank;
        private readonly ILogger<PayoutCommand> _logger;

        public PayoutCommand(IDataStoreService dataStore, IBankService bank, ILogger<PayoutCommand> logger)
        {
            _dataStore = dataStore;
            _bank = bank;
            _logger = logger;
        }

        public async Task<ActionResult<ProcessPaymentResponse>> ExecuteAsync(ProcessPaymentRequest paymentRequest)
        {
            try
            {
                var bankResponse = await this._bank.PayOutAsync(BankPayOutRequest.FromPaymentRequest(paymentRequest.paymentRequestBody));
                var newPaymentId = bankResponse.PaymentId;
                await this._dataStore.CreateAsync(Payment.FromPaymentRequest(paymentRequest.paymentRequestBody, bankResponse));
                return new CreatedResult($"/api/payments/{newPaymentId}", new ProcessPaymentResponse { PaymentId = newPaymentId });
            }
            catch (BankServiceException ex)
            {
                _logger.LogError("Bank Service", ex);
                return new ObjectResult(new StandardErrorResponse(ex))
                {
                    StatusCode = (int)HttpStatusCode.ServiceUnavailable
                };
            }
        }
    }
}