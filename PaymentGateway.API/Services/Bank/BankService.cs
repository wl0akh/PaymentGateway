using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PaymentGateway.Utils.Exceptions;

namespace PaymentGateway.Services.Bank
{
    public class BankService : IBankService
    {
        private Uri _url;
        private IHttpClientFactory _clientFactory;

        public BankService(Uri url, IHttpClientFactory clientFactory)
        {
            this._url = url;
            this._clientFactory = clientFactory;
        }

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
                return JsonSerializer.Deserialize<BankPayOutResponse>(body);
            }
            catch (JsonException jsonException)
            {
                throw new BankServiceException("Bank Service Incompatible", jsonException)
                {
                    RequestTraceId = Guid.NewGuid().ToString()
                };
            }
            catch (HttpRequestException httpRequestException)
            {
                throw new BankServiceException("Bank Service Unavailable", httpRequestException)
                {
                    RequestTraceId = Guid.NewGuid().ToString()
                };
            }
        }
    }
}