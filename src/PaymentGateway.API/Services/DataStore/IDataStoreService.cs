using System;
using System.Threading.Tasks;

namespace PaymentGateway.Services.DataStore
{
    /// <summary>
    /// IDataStoreService Interface to Abstract DataStore 
    /// </summary>
    public interface IDataStoreService
    {
        /// <summary>
        /// CreateAsync to create Payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns>Guid</returns>
        Task<Guid> CreateAsync(Payment payment);

        /// <summary>
        /// GetPaymentAsync to retrive Payment
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        Task<Payment> GetPaymentAsync(Guid paymentId);
    }
}