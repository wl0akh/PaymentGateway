using System;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.API.Services
{
    /// <summary>
    /// RequestTrackingService to Encapsulate Request Trace Id 
    /// which flows throws all the tears 
    /// </summary>
    public class RequestTrackingService
    {
        public RequestTrackingService()
        {
            this.RequestTraceId = Guid.NewGuid();
        }
        /// <summary>
        /// Readonly Request Trace Id
        /// </summary>
        /// <value>Guid</value>
        public Guid RequestTraceId { get; }
    }
}