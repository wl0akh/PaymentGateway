using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PaymentGateway.API.Services;
using PaymentGateway.Services.DataStore;

namespace PaymentGateway.Tests.Services.DataStore
{
    [TestFixture]
    public class DataStoreServiceTests
    {
        [Test]
        //TEST FOR RESPONSES OF DATASTORE SERVICE WHEN DB ACCESSIBLE 
        public void CreatePaymentWhenDBRunningAsync()
        {
            var options = new DbContextOptionsBuilder<DataStoreDbContext>()
                .UseInMemoryDatabase(databaseName: "DataStoreDbContext").Options;
            var expectedPayment = GeneratePayment();
            Assert.DoesNotThrowAsync(
                async () => await CreateRecord(options, expectedPayment)
            );
        }


        [Test]
        //TEST FOR RESPONSES OF DATASTORE SERVICE WHEN DB INACCESSIBLE 
        public void CreatePaymentWhenNoDbAsync()
        {
            var expectedPayment = GeneratePayment();
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await CreateRecord(Mock.Of<DbContextOptions<DataStoreDbContext>>(), expectedPayment)
            );
        }

        //TEST FOR RESPONSES OF DATASTORE SERVICE WHEN DB ACCESSIBLE 
        [Test]
        public async Task GetPaymentWhenExistAsync()
        {
            var options = new DbContextOptionsBuilder<DataStoreDbContext>()
                .UseInMemoryDatabase(databaseName: "DataStoreDbContext").Options;
            var expectedPayment = GeneratePayment();
            await CreateRecord(options, expectedPayment);
            var actualPayment = await GetRecord(options, expectedPayment.PaymentId);

            Assert.AreEqual(expectedPayment.PaymentId, actualPayment.PaymentId);
            Assert.AreEqual(expectedPayment.CardNumber, actualPayment.CardNumber);
            Assert.AreEqual(expectedPayment.Status, actualPayment.Status);
            Assert.AreEqual(expectedPayment.Expiry, actualPayment.Expiry);
            Assert.AreEqual(expectedPayment.Amount, actualPayment.Amount);
            Assert.AreEqual(expectedPayment.Currency, actualPayment.Currency);
            Assert.IsInstanceOf<DateTime>(actualPayment.CreatedAt);
        }

        [Test]
        //TEST FOR RESPONSES OF DATASTORE SERVICE WHEN DB INACCESSIBLE 
        public async Task GetPaymentWhenNotExistAsync()
        {
            var options = new DbContextOptionsBuilder<DataStoreDbContext>()
                .UseInMemoryDatabase(databaseName: "DataStoreDbContext").Options;

            Assert.IsNull(await GetRecord(options, Guid.NewGuid()));
        }

        private static async Task<Payment> GetRecord(DbContextOptions<DataStoreDbContext> options, Guid paymentId)
        {
            using (var context = new DataStoreDbContext(options))
            {
                var dataStoreService = new DataStoreService(
                    context,
                    Mock.Of<ILogger<DataStoreService>>(),
                    Mock.Of<RequestTrackingService>()
                    );
                return await dataStoreService.GetPaymentAsync(paymentId);
            }
        }

        private static async Task CreateRecord(DbContextOptions<DataStoreDbContext> options, Payment expectedPayment)
        {
            using (var context = new DataStoreDbContext(options))
            {
                var dataStoreService = new DataStoreService(
                    context,
                    Mock.Of<ILogger<DataStoreService>>(),
                    Mock.Of<RequestTrackingService>()
                    );
                await dataStoreService.CreateAsync(expectedPayment);
            }
        }

        private static Payment GeneratePayment()
        {
            return new Payment
            {
                PaymentId = Guid.NewGuid(),
                CardNumber = "5000000000000000004",
                Status = "successful",
                Expiry = "12/24",
                Amount = 24.31m,
                Currency = "GBP"
            };
        }
    }
}