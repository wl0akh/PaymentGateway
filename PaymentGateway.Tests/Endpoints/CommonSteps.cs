using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
        public async Task WhenAPOSTIsCalledOnAsync(string method, string path)
        {
            this._context.Request.Method = new HttpMethod(method);
            this._context.Request.RequestUri = new Uri(path, UriKind.Relative);
            this._context.Response = await this._context.Client.SendAsync(this._context.Request);
        }

        [Then(@"it returns response with status code (.*)")]
        public void ThenItReturnsResponseWithStatusCodeAsync(string code)
        {
            Assert.AreEqual(code, this._context.Response.StatusCode.ToString());
        }

        [Then(@"response body contain (.*) in error key")]
        public async Task ThenResponseBodyContainInErrorKey(string field)
        {
            var result = await this._context.Response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(result);
            Assert.IsTrue(errorResponse.Keys.Contains(field.Replace("\"", "").Replace("\'", "")));
        }
    }
}