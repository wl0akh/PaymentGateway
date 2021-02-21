using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Services;

namespace PaymentGateway.API.Utils.Filters
{
    /// <summary>
    /// Action Filter Class to Add Request Tracking 
    /// and Compute Total time take to execute Action
    /// </summary>
    public class TrackingActionFilter : ActionFilterAttribute
    {
        private Stopwatch _stopwatch;
        private ILogger<TrackingActionFilter> _logger;
        private RequestTrackingService _requestTrackingService;

        /// <summary>
        /// OnActionExecuting to run before Action 
        /// and start stopwatch and set RequestTrackingId
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this._logger = (ILogger<TrackingActionFilter>)context.HttpContext.RequestServices
            .GetService(typeof(ILogger<TrackingActionFilter>));

            //Initialize Request Tracking Id
            this._requestTrackingService = (RequestTrackingService)context.HttpContext.RequestServices
            .GetService(typeof(RequestTrackingService));

            this._stopwatch = new Stopwatch();
            this._stopwatch.Start();

            this._logger.LogInformation($@"RequestId:{this._requestTrackingService.RequestTraceId} 
            Started for controller:{context.RouteData.Values["controller"]} and action:{context.RouteData.Values["action"]}");
        }

        /// <summary>
        /// OnActionExecuted to run after Action and stop the stopwatch 
        /// and set RequestTraceId and total duration of Action in Response Header
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            this._stopwatch.Stop();

            //SET Duration i.e time take to process the request in Response header"
            context.HttpContext.Response.Headers.Add("X-Duration", $"{ this._stopwatch.Elapsed.TotalMilliseconds}");

            //SET Request TraceId in Response header"
            context.HttpContext.Response.Headers.Add("X-RequestTraceId", $"{this._requestTrackingService.RequestTraceId}");

            this._logger.LogInformation($@"RequestId:{this._requestTrackingService.RequestTraceId} 
            Finished in Duration: {this._stopwatch.Elapsed.TotalMilliseconds} Milliseconds", context.RouteData);
        }
    }
}