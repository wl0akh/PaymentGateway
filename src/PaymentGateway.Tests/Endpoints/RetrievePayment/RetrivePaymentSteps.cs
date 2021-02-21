using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using PaymentGateway.Services.DataStore;
using TechTalk.SpecFlow;

namespace PaymentGateway.Tests.Endpoints.RetrievePayment
{
    [Binding]
    [Scope(Tag = "RetrievePayment")]
    public class RetrivePaymentSteps
    {
        private readonly EndpointTestContext _context;
        private Guid _paymentId;

        public RetrivePaymentSteps(EndpointTestContext context)
        {
            this._context = context;
        }

        [Given(@"PaymentId as (.*) is provided")]
        public void GivenPaymentIdAsIsProvided(string paymentId)
        {
            Guid.TryParse(paymentId, out this._paymentId);
        }

        [Given(@"following payment record exists in data store")]
        public async Task GivenRecordWithPaymentIdInDataStoreAsync(string paymentRecord)
        {
            var payment = JsonConvert.DeserializeObject<Payment>(paymentRecord);
            await this._context.CreatePaymentInDataStore(payment);
        }
        [Given(@"record with PaymentId (.*) does not exists in data store")]
        public async Task GivenRecordWithPaymentIdInNotExistsDataStoreAsync(string paymentId)
        {
            var result = await this._context.GetPaymentAsync(Guid.Parse(paymentId));
            Assert.IsNull(result);
        }
    }
}