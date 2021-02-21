using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Services;
using PaymentGateway.Utils.Exceptions;

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
        /// <param name="bankPaymentRequest"></param>
        /// <returns>BankPayOutResponse</returns>
        public async Task<BankPayOutResponse> PayOutAsync(BankPayOutRequest bankPaymentRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, this._url);
            request.Headers.Add("Authorization", "Bearer ****");
            request.Content = new StringContent(
                JsonSerializer.Serialize<BankPayOutRequest>(bankPaymentRequest),
                Encoding.UTF8,
                "application/json"
            );

            var client = _clientFactory.CreateClient();

            try
            {
                var response = await client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();

                this._logger.LogInformation($@"RequestId:{this._requestTrackingService.RequestTraceId} 
                Bank PayOut Finished");

                return JsonSerializer.Deserialize<BankPayOutResponse>(body);
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