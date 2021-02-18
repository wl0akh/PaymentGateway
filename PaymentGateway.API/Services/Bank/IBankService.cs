using System.Threading.Tasks;

namespace PaymentGateway.Services.Bank
{
    public interface IBankService
    {
        Task<BankPaymentResponse> ExecutePaymentAsync(BankPaymentRequest bankPaymentRequest);
    }
}