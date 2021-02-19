using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PaymentGateway.Services.DataStore;

namespace PaymentGateway.Tests.Services.DataStore
{
    [TestFixture]
    public class DataStoreServiceTests
    {
        [Test]
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
        public void CreatePaymentWhenNoDbAsync()
        {
            var expectedPayment = GeneratePayment();
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await CreateRecord(Mock.Of<DbContextOptions<DataStoreDbContext>>(), expectedPayment)
            );
        }

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
        public void GetPaymentWhenNotExistAsync()
        {
            var options = new DbContextOptionsBuilder<DataStoreDbContext>()
                .UseInMemoryDatabase(databaseName: "DataStoreDbContext").Options;

            Assert.ThrowsAsync<InvalidOperationException>(async () => await GetRecord(options, Guid.NewGuid()));
        }

        private static async Task<Payment> GetRecord(DbContextOptions<DataStoreDbContext> options, Guid paymentId)
        {
            using (var context = new DataStoreDbContext(options))
            {
                var dataStoreService = new DataStoreService(context);
                return await dataStoreService.GetPaymentAsync(paymentId);
            }
        }

        private static async Task CreateRecord(DbContextOptions<DataStoreDbContext> options, Payment expectedPayment)
        {
            using (var context = new DataStoreDbContext(options))
            {
                var dataStoreService = new DataStoreService(context);
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