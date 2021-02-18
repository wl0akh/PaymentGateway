using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using PaymentGateway.API;

namespace PaymentGateway.Tests.Endpoints
{
    public class EndpointTestContext
    {
        private readonly WebApplicationFactory<Startup> _appFactory;
        public HttpClient Client { get; set; }
        public HttpRequestMessage Request { get; set; }
        public HttpResponseMessage Response { get; set; }
        public EndpointTestContext(WebApplicationFactory<Startup> appFactory)
        {
            this._appFactory = appFactory;
            this.Client = this._appFactory.CreateDefaultClient();
            this.Request = new HttpRequestMessage();
        }
        public T GetServiceFromApp<T>()
        {
            return (T)this._appFactory.Services.GetService(typeof(T));
        }

        public string GetPaymentIdFromHeader(string key)
        {
            return this.Response.Headers.Location.ToString().Replace("/api/payments/", "");
        }
    }
}