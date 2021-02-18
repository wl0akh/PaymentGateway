using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Services.DataStore
{
    public class DataStoreService : IDataStoreService
    {
        private DataStoreDbContext _context;

        public DataStoreService(DataStoreDbContext dataStoreDbContext)
        {
            this._context = dataStoreDbContext;
        }

        public async Task<Guid> CreateAsync(Payment payment)
        {
            var createdPayment = this._context.Payments.Add(payment);
            await this._context.SaveChangesAsync();
            return createdPayment.Entity.paymentId;
        }

        public async Task<Payment> GetPaymentAsync(Guid paymentId)
        {
            return await this._context.Payments.SingleAsync(p => (p.paymentId == paymentId));
        }
    }
}