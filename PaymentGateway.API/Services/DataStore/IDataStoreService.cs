using System;
using System.Threading.Tasks;

namespace PaymentGateway.Services.DataStore
{
    public interface IDataStoreService
    {
        Task<Guid> CreateAsync(Payment payment);
        Task<Payment> GetPaymentAsync(Guid expectedPaymentId);
    }
}