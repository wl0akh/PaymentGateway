using System.Threading.Tasks;

namespace PaymentGateway.Services.Bank
{
    public interface IBankService
    {
        Task<BankPayOutResponse> PayOutAsync(BankPayOutRequest bankPaymentRequest);
    }
}