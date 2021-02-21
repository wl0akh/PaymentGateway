using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace PaymentGateway.Tests.Endpoints
{
    [Binding]
    public class CommonSteps
    {
        private readonly EndpointTestContext _context;
        public CommonSteps(EndpointTestContext context)
        {
            this._context = context;
        }

        [BeforeFeature]
        public static void SetUpRegisteredJwt()
        {
            Environment.SetEnvironmentVariable("MOUNT_BANK_URL", "http://localhost:2525");
            Environment.SetEnvironmentVariable("BANK_SERVICE_URL", "http://localhost:4545/payments");
            Environment.SetEnvironmentVariable("MYSQL_CONNECTION_STRING", "server=localhost;uid=root;pwd=admin;database=TestPaymentGateway");
        }

        [AfterScenario]
        private async Task TearDownAsync()
        {
            await this._context.DropDBAsync();
            await this._context.TearDownBankService();
        }

        [Given(@"a request with body as")]
        public void GivenARequestWithBodyAs(string body)
        {
            this._context.Request.Content = new StringContent(
                    body,
                    Encoding.UTF8,
                    "application/json"
                );
        }

        [When(@"a (.*) is called on (.*)")]
        public async Task WhenAIsCalledOnAsync(string method, string path)
        {
            this._context.Request.Method = new HttpMethod(method);
            this._context.Request.RequestUri = new Uri(path, UriKind.Relative);
            this._context.Response = await this._context.Client.SendAsync(this._context.Request);
            this._context.ResponseBodyString = await this._context.Response.Content.ReadAsStringAsync();
        }

        [Then(@"it returns response with status code (.*)")]
        public void ThenItReturnsResponseWithStatusCode(string code)
        {
            Assert.AreEqual(code, this._context.Response.StatusCode.ToString());
        }

        [Then(@"response body contains")]
        public void ThenResponseBodyContains(string field)
        {
            Assert.IsTrue(this._context.ResponseBodyString.Contains(field));
        }

        [Then(@"response body should not contains")]
        public void ThenResponseBodyShouldNotContains(string field)
        {
            Assert.IsFalse(this._context.ResponseBodyString.Contains(field));
        }
    }
}