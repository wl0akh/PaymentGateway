using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using PaymentGateway.API.Services;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Services.Bank;
using PaymentGateway.Services.Bank.Exceptions;
using static PaymentGateway.Domain.Entities.Payment;

namespace PaymentGateway.Tests.Services
{
    [TestFixture]
    public class BankServiceTests
    {
        [TestCase(Status.APPROVED)]
        [TestCase(Status.DECLINED)]
        /// TEST FOR RESPONSES OF BANK SERVICE WHEN IT'S ACCESSIBLE
        public async Task ExecutePaymentWhenBankServiCeRunningAsync(Status responseStatus)
        {
            var body = GenerateRequestBody(Guid.NewGuid(), responseStatus);
            var httpClientFactory = MockHttpClientFactoryWithResponse(body);
            var bankService = GetBankService(body, httpClientFactory);
            BankPayOutResponse result = await ExecuteBankService(bankService);
            Assert.IsInstanceOf<BankPayOutResponse>(result);
            Assert.IsNotNull(result.PaymentId);
            Assert.AreEqual(responseStatus, result.PaymentStatus);
        }

        /// TEST FOR RESPONSES OF BANK SERVICE WHEN IT'S ACCESSIBLE RESPONSE BODY IN INVALID
        [Test]
        public void ExecutePaymentWhenResponseInInvalidformate()
        {
            var body = new StringContent("invalid Formate");
            var httpClientFactory = MockHttpClientFactoryWithResponse(body);
            var bankService = GetBankService(body, httpClientFactory);
            Assert.ThrowsAsync<BankServiceException>(
                async () => await ExecuteBankService(bankService)
            );
        }

        [Test]
        /// TEST FOR RESPONSES OF BANK SERVICE WHEN IT'S INACCESSIBLE 
        public void ExecutePaymentWhenBankServiceInaccessible()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            httpMessageHandlerMock
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                 "SendAsync",
                 ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>()).Throws(new HttpRequestException());


            var httpClient = new HttpClient(httpMessageHandlerMock.Object);
            var httpClientFactory = new Mock<IHttpClientFactory>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
            var body = GenerateRequestBody(Guid.NewGuid(), Status.APPROVED);
            var bankService = GetBankService(body, httpClientFactory);

            // Check if it throws BankServiceException when unable to connect
            Assert.ThrowsAsync<BankServiceException>(
            async () => await ExecuteBankService(bankService));
        }


        private static BankService GetBankService(StringContent body, Mock<IHttpClientFactory> httpClientFactory)
        {
            var requestTrackingService = new RequestTrackingService();
            var logger = Mock.Of<ILogger<BankService>>();
            var bankService = new BankService(
                requestTrackingService,
                logger,
                new Uri("http://BankService_url.io"),
                httpClientFactory.Object);
            return bankService;
        }
        private static StringContent GenerateRequestBody(Guid guid, Status status)
        {
            return new StringContent(JsonConvert.SerializeObject(
                    new BankPayOutResponse
                    {
                        PaymentId = guid,
                        PaymentStatus = status
                    }
                    ));
        }

        private static Task<BankPayOutResponse> ExecuteBankService(BankService bankService)
        {
            var futureDate = DateTime.Now.AddDays(30);
            return bankService.PayOutAsync(
                new Payment("GBP", "123", 25, $"{futureDate.Month:00}/{futureDate.Year}", "")
                );
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