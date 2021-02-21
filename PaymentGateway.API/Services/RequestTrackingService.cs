using System;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.API.Services
{
    public class RequestTrackingService
    {
        public RequestTrackingService()
        {
            this.RequestTraceId = Guid.NewGuid();
        }

        public Guid RequestTraceId { get; }
    }
}