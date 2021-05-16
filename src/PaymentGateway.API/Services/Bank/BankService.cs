using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentGateway.API.Services;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Utils.Exceptions;
using PaymentGateway.Utils.Helpers;

namespace PaymentGateway.Services.Bank
{
    /// <summary>
    /// BankService class to IBankService Interface to Abstract Bank
    /// </summary>
    public class BankService : IBankService
    {
        private Uri _url;
        private IHttpClientFactory _clientFactory;
        private ILogger<BankService> _logger;
        private RequestTrackingService _requestTrackingService;

        public BankService(
            RequestTrackingService requestTrackingService,
            ILogger<BankService> logger,
            Uri url,
            IHttpClientFactory clientFactory
            )
        {
            this._clientFactory = clientFactory;
            this._url = url;
            this._logger = logger;
            this._requestTrackingService = requestTrackingService;
        }

        /// <summary>
        /// PayOutAsync method pay the payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns>BankPayOutResponse</returns>
        public async Task<BankPayOutResponse> PayOutAsync(Payment payment)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, this._url);
            request.Content = new StringContent(
                JsonConvert.SerializeObject(payment),
                Encoding.UTF8,
                "application/json"
            );

            var client = _clientFactory.CreateClient();

            try
            {
                var response = await client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();
                var bankResponse = JsonConvert.DeserializeObject<BankPayOutResponse>(body);
                this._logger.LogInformation($@"RequestId:{this._requestTrackingService.RequestTraceId} 
                    Bank Payout finished with status:{bankResponse.PaymentStatus}
                    For Card Ending: {MaskHelper.Mask(payment.CardNumber)}");

                return bankResponse;
            }
            catch (JsonException jsonException)
            {
                this._logger.LogError(jsonException, $@"RequestId:{this._requestTrackingService.RequestTraceId} 
                Bank Service Incompatible");

                throw new BankServiceException("Bank Service Incompatible", jsonException)
                {
                    RequestTraceId = Guid.NewGuid().ToString()
                };
            }
            catch (HttpRequestException httpRequestException)
            {
                this._logger.LogError(httpRequestException, $@"RequestId:{this._requestTrackingService.RequestTraceId} 
                Bank Service Unavailable");

                throw new BankServiceException("Bank Service Unavailable", httpRequestException)
                {
                    RequestTraceId = Guid.NewGuid().ToString()
                };
            }
        }
    }
}