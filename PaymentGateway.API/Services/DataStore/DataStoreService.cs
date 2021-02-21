using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Services;

namespace PaymentGateway.Services.DataStore
{
    /// <summary>
    /// DataStoreService to implement IDataStoreService abstraction 
    /// </summary>
    public class DataStoreService : IDataStoreService
    {
        private DataStoreDbContext _context;
        private ILogger<DataStoreService> _logger;
        private RequestTrackingService _requestTrackingService;
        public DataStoreService(
            DataStoreDbContext dataStoreDbContext,
            ILogger<DataStoreService> logger,
            RequestTrackingService requestTrackingService)
        {
            this._context = dataStoreDbContext;
            this._logger = logger;
            this._requestTrackingService = requestTrackingService;
        }

        /// <summary>
        /// CreateAsync to create Payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns>Guid</returns>
        public async Task<Guid> CreateAsync(Payment payment)
        {
            var createdPayment = this._context.Payments.Add(payment);
            await this._context.SaveChangesAsync();

            _logger.LogDebug($@"RequestId:{this._requestTrackingService.RequestTraceId} 
            Payment with PaymentId:{payment.PaymentId} saved to DB");

            return createdPayment.Entity.PaymentId;
        }

        /// <summary>
        /// GetPaymentAsync to retrive Payment
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        public async Task<Payment> GetPaymentAsync(Guid paymentId)
        {
            _logger.LogDebug($@"RequestId:{this._requestTrackingService.RequestTraceId} 
            Payment with PaymentId:{paymentId} retrived from DB");

            return await this._context.Payments.Where(r => r.PaymentId == paymentId)
            .FirstOrDefaultAsync<Payment>();
        }
    }
}