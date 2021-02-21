using System.Threading.Tasks;

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
        /// <param name="bankPaymentRequest"></param>
        /// <returns>BankPayOutResponse</returns>
        Task<BankPayOutResponse> PayOutAsync(BankPayOutRequest bankPaymentRequest);
    }
}