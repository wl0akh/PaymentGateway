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
            var actualPayment = await GetRecord(options, expectedPayment.paymentId);

            Assert.AreEqual(expectedPayment.paymentId, actualPayment.paymentId);
            Assert.AreEqual(expectedPayment.cardNumber, actualPayment.cardNumber);
            Assert.AreEqual(expectedPayment.status, actualPayment.status);
            Assert.AreEqual(expectedPayment.expiry, actualPayment.expiry);
            Assert.AreEqual(expectedPayment.amount, actualPayment.amount);
            Assert.AreEqual(expectedPayment.currency, actualPayment.currency);
            Assert.IsInstanceOf<DateTime>(actualPayment.createdAt);
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
                paymentId = Guid.NewGuid(),
                cardNumber = "5000000000000000004",
                status = "successful",
                expiry = "12/24",
                amount = 24.31m,
                currency = "GBP"
            };
        }
    }
}