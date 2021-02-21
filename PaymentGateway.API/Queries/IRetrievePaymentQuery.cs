using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Endpoints.RetrievePayment;

namespace PaymentGateway.API.Endpoints.Queries
{
    /// <summary>
    /// IRetrievePaymentQuery to encapsulate Retrieval of Payment
    /// </summary>
    public interface IRetrievePaymentQuery
    {
        /// <summary>
        /// ExecuteAsync to execute and retrieve payment
        /// </summary>
        /// <param name="retrievePaymentRequest"></param>
        /// <returns>Task<ActionResult<RetrievePaymentResponse>></returns>
        Task<ActionResult<RetrievePaymentResponse>> ExecuteAsync(RetrievePaymentRequest retrievePaymentRequest);
    }
}