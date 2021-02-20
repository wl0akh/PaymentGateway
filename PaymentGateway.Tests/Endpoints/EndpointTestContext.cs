using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MbDotNet;
using MbDotNet.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.API;
using PaymentGateway.Services.DataStore;

namespace PaymentGateway.Tests.Endpoints
{
    public class EndpointTestContext
    {
        private readonly WebApplicationFactory<Startup> _appFactory;
        public HttpClient Client { get; set; }
        public HttpRequestMessage Request { get; set; }
        public HttpResponseMessage Response { get; set; }
        public DbContextOptions<DataStoreDbContext> DBOptions { get; set; }
        public Uri BankServiceUri { get; set; }
        private MountebankClient _mountebankClient;

        public EndpointTestContext(WebApplicationFactory<Startup> appFactory)
        {
            this._appFactory = appFactory;
            this.Client = this._appFactory.CreateDefaultClient();
            this.DBOptions = new DbContextOptionsBuilder<DataStoreDbContext>()
            .UseMySQL(Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING")).Options;
            this.Request = new HttpRequestMessage();
            this.BankServiceUri = new Uri(Environment.GetEnvironmentVariable("BANK_SERVICE_URL"));
            this._mountebankClient = new MountebankClient(Environment.GetEnvironmentVariable("MOUNT_BANK_URL"));
        }
        public T GetServiceFromApp<T>()
        {
            return (T)this._appFactory.Services.GetService(typeof(T));
        }

        public string GetPaymentIdFromHeader(string key)
        {
            return this.Response.Headers.Location.ToString().Replace("/api/payments/", "");
        }
        public async Task CreateBankServiceImposterAsync(string responseBody)
        {
            await TearDownBankService();
            var imposter = _mountebankClient.CreateHttpImposter(this.BankServiceUri.Port, "Bank Service Imposter");
            imposter.AddStub().OnPathAndMethodEqual("/payments", Method.Post)
              .ReturnsBody(HttpStatusCode.OK, responseBody);
            await Task.Run(() => _mountebankClient.Submit(imposter));
        }

        public async Task<int> GetNumberOfRecordsInDBAsync()
        {
            using var dbContext = new DataStoreDbContext(this.DBOptions);
            return await dbContext.Payments.CountAsync<Payment>();
        }

        public async Task<Payment> GetPaymentAsync(Guid Id)
        {
            using var dbContext = new DataStoreDbContext(this.DBOptions);
            return await dbContext.Payments.FirstAsync<Payment>(r => r.PaymentId == Id);
        }
        public async Task DropDBAsync()
        {
            using var dbContext = new DataStoreDbContext(this.DBOptions);
            await dbContext.Database.EnsureDeletedAsync();
        }
        public async Task TearDownBankService()
        {
            await Task.Run(() => _mountebankClient.DeleteImposter(this.BankServiceUri.Port));
        }
    }
}