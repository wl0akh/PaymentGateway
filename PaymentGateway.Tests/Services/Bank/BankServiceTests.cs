using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PaymentGateway.Services.Bank;
using PaymentGateway.Utils.Exceptions;

namespace PaymentGateway.Tests.Services
{
    [TestFixture]
    public class BankServiceTests
    {
        [Test]
        public async Task ExecutePaymentSuccessfulAsync()
        {
            var body = GenerateRequestBody(Guid.NewGuid(), "successful");
            Mock<IHttpClientFactory> httpClientFactory = MockHttpClientFactoryWithResponse(body);
            var bankService = new BankService(new Uri("http://BankService_url.io"), httpClientFactory.Object);
            BankPaymentResponse result = await ExecuteBankService(bankService);
            Assert.IsInstanceOf<BankPaymentResponse>(result);
            Assert.IsNotNull(result.PaymentId);
            Assert.AreEqual("successful", result.Status);
        }

        [Test]
        public async Task ExecutePaymentUnSuccessfullAsync()
        {
            var body = GenerateRequestBody(Guid.NewGuid(), "unsuccessful");
            Mock<IHttpClientFactory> httpClientFactory = MockHttpClientFactoryWithResponse(body);
            var bankService = new BankService(new Uri("http://BankService_url.io"), httpClientFactory.Object);
            BankPaymentResponse result = await ExecuteBankService(bankService);
            Assert.IsInstanceOf<BankPaymentResponse>(result);
            Assert.IsNotNull(result.PaymentId);
            Assert.AreEqual("unsuccessful", result.Status);
        }

        [Test]
        public void ExecutePaymentWhenResponseInInvalidformate()
        {
            var body = new StringContent("invalid Formate");
            var httpClientFactory = MockHttpClientFactoryWithResponse(body);
            var bankService = new BankService(new Uri("http://BankService_url.io"), httpClientFactory.Object);
            Assert.ThrowsAsync<BankServiceException>(
                async () => await ExecuteBankService(bankService)
            );
        }

        [Test]
        public void ExecutePaymentWhenBankServiceInaccessible()
        {
            var url = new Uri("http://BankService_url.io");
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                 "SendAsync",
                 ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>()).Throws(new HttpRequestException());


            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var bankService = new BankService(url, httpClientFactory.Object);

            // Check if it throws BankServiceException when unable to connect
            Assert.ThrowsAsync<BankServiceException>(
            async () => await ExecuteBankService(bankService));
        }

        private static StringContent GenerateRequestBody(Guid guid, string status)
        {
            return new StringContent(JsonSerializer.Serialize<BankPaymentResponse>(
                    new BankPaymentResponse
                    {
                        PaymentId = guid,
                        Status = status
                    }
                    ));
        }

        private static Task<BankPaymentResponse> ExecuteBankService(BankService bankService)
        {
            var futureDate = DateTime.Now.AddDays(30);
            return bankService.ExecutePaymentAsync(new BankPaymentRequest
            {
                CardNumber = "",
                Expiry = $"{futureDate.Month:00}/{futureDate.Year}",
                Amount = 25,
                Currency = "GBP",
                CVV = "123"
            });
        }

        private static Mock<IHttpClientFactory> MockHttpClientFactoryWithResponse(StringContent content)
        {
            var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                 "SendAsync",
                 ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(httpResponse);

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            return httpClientFactory;
        }

    }
}