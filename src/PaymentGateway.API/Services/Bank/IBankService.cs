using System.Threading.Tasks;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Services.Bank
{
    /// <summary>
    /// IBankService Interface to Abstract Bank
    /// </summary>
    public interface IBankService
    {

        /// <summary>
        /// PayOutAsync method pay the payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns>BankPayOutResponse</returns>
        Task<BankPayOutResponse> PayOutAsync(Payment payment);
    }
}