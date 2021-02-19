using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using MbDotNet;
using MbDotNet.Enums;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PaymentGateway.API.Endpoints;
using PaymentGateway.Services.DataStore;
using TechTalk.SpecFlow;

namespace PaymentGateway.Tests.Endpoints.ProcessPayment
{
    [Binding]
    [Scope(Tag = "ProcessPayment")]
    public class ProcessPaymentSteps
    {
        private readonly EndpointTestContext _context;
        private static MountebankClient _mountebankClient;

        public ProcessPaymentSteps(EndpointTestContext context)
        {
            _context = context;

        }

        [BeforeFeature]
        public static void SetUpRegisteredJwt()
        {
            Environment.SetEnvironmentVariable("BANK_SERVICE_URL", "http://localhost:4545/payments");
            Environment.SetEnvironmentVariable("MYSQL_CONNECTION_STRING", "server=localhost;uid=root;pwd=admin;database=TestPaymentGateway");
            _mountebankClient = new MountebankClient("http://localhost:2525");

        }

        [AfterScenario("ProcessPayment")]
        private async Task TearDownAsync()
        {
            await DropDBAsync();
            await TearDownBankService();
        }

        [When(@"Bank Service Responds as")]
        public async Task WhenBankTransactionIsAsync(string responseBody)
        {
            await CreateBankServiceImposterAsync(responseBody);
        }

        [When(@"Bank Service is inaccessible")]
        public void WhenBankServiceIsInaccessible()
        {

        }

        [Then(@"payment is recorded in data store with (.*) as (.*)")]
        public async Task ThenPaymentIsRecordedInDataStoreWithFieldAsValueAsync(string propName, string value)
        {
            Guid paymentId = Guid.Parse(this._context.GetPaymentIdFromHeader("PaymentId"));
            var payment = await GetPaymentAsync(paymentId);
            var actualValue = payment.GetType().GetProperty(propName).GetValue(payment, null);
            Assert.AreEqual(value, actualValue.ToString());
        }

        [Then(@"(.*) is shown in response body")]
        public async Task ThenResponseBodyContainInErrorKey(string field)
        {
            var result = await this._context.Response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<StandardErrorResponse>(result);
            Assert.AreEqual(field, errorResponse.Error);
        }
        [Then(@"payment (.*) recorded in data store")]
        public void ThenPaymentRecordedInDataStoreAsync(string isRecorded)
        {
        }

        [Then(@"payment Id (.*) returned in response header")]
        public void ThenPaymentIdIsReturnedInResponseHeader(string isReturned)
        {
            if (isReturned == "is")
            {
                Assert.IsNotNull(this._context.Response.Headers.Location);
            }
        }
        private static async Task CreateBankServiceImposterAsync(string responseBody)
        {
            await TearDownBankService();
            var bankServiceUri = new Uri(Environment.GetEnvironmentVariable("BANK_SERVICE_URL"));
            var imposter = _mountebankClient.CreateHttpImposter(bankServiceUri.Port, "Bank Service Imposter");
            imposter.AddStub()
              .OnPathAndMethodEqual("/payments", Method.Post)
              .ReturnsBody(
                  HttpStatusCode.OK,
                  responseBody
                );
            await Task.Run(() => _mountebankClient.Submit(imposter));
        }

        private static async Task<Payment> GetPaymentAsync(Guid Id)
        {
            var options = new DbContextOptionsBuilder<DataStoreDbContext>()
                .UseMySQL(Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING")).Options;
            using (var dbContext = new DataStoreDbContext(options))
            {
                return await dbContext.Payments.FirstAsync<Payment>(r => r.PaymentId == Id);
            }
        }
        private static async Task DropDBAsync()
        {
            var options = new DbContextOptionsBuilder<DataStoreDbContext>()
                .UseMySQL(Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING")).Options;
            using (var dbContext = new DataStoreDbContext(options))
            {
                await dbContext.Database.EnsureDeletedAsync();
            }
        }
        private static async Task TearDownBankService()
        {
            var bankServiceUri = new Uri(Environment.GetEnvironmentVariable("BANK_SERVICE_URL"));
            await Task.Run(() => _mountebankClient.DeleteImposter(bankServiceUri.Port));
        }
    }
}