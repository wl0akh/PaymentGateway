using System;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;
using PaymentGateway.API.Endpoints;
using TechTalk.SpecFlow;

namespace PaymentGateway.Tests.Endpoints.ProcessPayment
{
    [Binding]
    [Scope(Tag = "ProcessPayment")]
    public class ProcessPaymentSteps
    {
        private readonly EndpointTestContext _context;

        public ProcessPaymentSteps(EndpointTestContext context)
        {
            _context = context;
        }

        [BeforeFeature]
        public static void SetUpRegisteredJwt()
        {
            Environment.SetEnvironmentVariable("MOUNT_BANK_URL", "http://localhost:2525");
            Environment.SetEnvironmentVariable("BANK_SERVICE_URL", "http://localhost:4545/payments");
            Environment.SetEnvironmentVariable("MYSQL_CONNECTION_STRING", "server=localhost;uid=root;pwd=admin;database=TestPaymentGateway");
        }

        [AfterScenario("ProcessPayment")]
        private async Task TearDownAsync()
        {
            await this._context.DropDBAsync();
            await this._context.TearDownBankService();
        }

        [When(@"Bank Service Responds as")]
        public async Task WhenBankTransactionIsAsync(string responseBody)
        {
            await this._context.CreateBankServiceImposterAsync(responseBody);
        }

        [When(@"Bank Service is inaccessible")]
        public async Task WhenBankServiceIsInaccessibleAsync()
        {
            await this._context.TearDownBankService();
        }

        [Then(@"(.*) in data store record (.*) recorded as (.*)")]
        public async Task ThenPaymentIsRecordedInDataStoreWithFieldAsValueAsync(string propName, string isRecorded, string value)
        {
            Guid paymentId = Guid.Parse(this._context.GetPaymentIdFromHeader("PaymentId"));
            var payment = await this._context.GetPaymentAsync(paymentId);
            if (isRecorded == "is")
            {
                var actualValue = payment.GetType().GetProperty(propName).GetValue(payment, null);
                Assert.AreEqual(value, actualValue.ToString());
            }
            if (isRecorded == "is not")
            {
                var actualValue = payment.GetType().GetProperty(propName);
                Assert.AreEqual(null, actualValue);
            }
        }

        [Then(@"(.*) is shown in standard error response body")]
        public async Task ThenResponseBodyContainInErrorKey(string field)
        {
            var result = await this._context.Response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<StandardErrorResponse>(result);
            Assert.AreEqual(field, errorResponse.Error);
        }

        [Then(@"payment (.*) recorded in data store")]
        public async Task ThenPaymentRecordedInDataStoreAsync(string isRecorded)
        {
            var numberOfRecords = await this._context.GetNumberOfRecordsInDBAsync();
            if (isRecorded == "is")
            {
                Assert.AreEqual(1, numberOfRecords);
            }
            if (isRecorded == "is not")
            {
                Assert.AreEqual(0, numberOfRecords);
            }
        }

        [Then(@"payment Id is returned in response header")]
        public void ThenPaymentIdIsReturnedInResponseHeader()
        {
            Guid paymentId = Guid.Parse(this._context.GetPaymentIdFromHeader("PaymentId"));
            Assert.AreNotEqual(default(Guid), paymentId);
            Assert.IsNotNull(paymentId);
        }
    }
}