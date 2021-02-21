using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Services;

namespace PaymentGateway.API.Utils.Filters
{
    public class TrackingActionFilter : ActionFilterAttribute
    {
        private Stopwatch _stopwatch;
        private ILogger<TrackingActionFilter> _logger;
        private RequestTrackingService _requestTrackingService;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this._logger = (ILogger<TrackingActionFilter>)context.HttpContext.RequestServices.GetService(typeof(ILogger<TrackingActionFilter>));
            this._requestTrackingService = (RequestTrackingService)context.HttpContext.RequestServices
            .GetService(typeof(RequestTrackingService));
            this._stopwatch = new Stopwatch();
            this._logger.LogInformation($@"RequestId:{this._requestTrackingService.RequestTraceId} 
            Started for controller:{context.RouteData.Values["controller"]} and action:{context.RouteData.Values["action"]}");
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            this._stopwatch.Start();
            context.HttpContext.Response.Headers.Add("X-Duration", $"{ this._stopwatch.Elapsed.TotalMilliseconds}");
            context.HttpContext.Response.Headers.Add("X-RequestTraceId", $"{this._requestTrackingService.RequestTraceId}");

            this._logger.LogInformation($@"RequestId:{this._requestTrackingService.RequestTraceId} 
            Finished in Duration: {this._stopwatch.Elapsed.TotalMilliseconds} Milliseconds", context.RouteData);
        }
    }
}