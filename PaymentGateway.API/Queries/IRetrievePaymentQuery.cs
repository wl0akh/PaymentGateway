using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Endpoints.RetrievePayment;

namespace PaymentGateway.API.Endpoints.Queries
{
    public interface IRetrievePaymentQuery
    {
        Task<ActionResult<RetrievePaymentResponse>> ExecuteAsync(RetrievePaymentRequest retrievePaymentRequest);
    }
}