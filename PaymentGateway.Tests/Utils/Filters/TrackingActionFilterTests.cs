using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using PaymentGateway.API.Services;
using PaymentGateway.API.Utils.Filters;

namespace PaymentGateway.Tests.Utils.Filters
{
    [TestFixture]
    public class TrackingActionFilterTests
    {
        [Test]
        //TEST FOR FILTER AND CHECK IF IT SETS X-RequestTraceId AND X-Duration IN RESPONSE HEADER
        public void OnActionExecutingTest()
        {
            var trackingFilter = new TrackingActionFilter();
            var actionContext = MockActionContext();

            var actionExecutingContext = new ActionExecutingContext(actionContext,
             new List<IFilterMetadata>(), new Dictionary<string, object>(), new { });

            var resultExecutedContext = new ActionExecutedContext(actionContext,
             new List<IFilterMetadata>(), new { });

            // SIMULATING ACTION EXECUTION BY CALL BEFORE AND AFTER EXECUTION 
            trackingFilter.OnActionExecuting(actionExecutingContext);
            trackingFilter.OnActionExecuted(resultExecutedContext);

            // CHECK IF IT SETS X-RequestTraceId 
            Assert.IsNotNull(resultExecutedContext.HttpContext.Response.Headers["X-RequestTraceId"]);

            // CHECK IF IT SETS X-Duration
            Assert.IsNotNull(resultExecutedContext.HttpContext.Response.Headers["X-Duration"]);

        }
        private ActionContext MockActionContext()
        {
            var logger = Mock.Of<ILogger<TrackingActionFilter>>();
            var requestTrackingService = new RequestTrackingService();
            var httpContext = new Mock<HttpContext>();
            var headers = new HeaderDictionary(new Dictionary<string, StringValues>());
            httpContext.Setup(c => c.RequestServices.GetService(typeof(RequestTrackingService))).Returns(requestTrackingService);
            httpContext.Setup(c => c.RequestServices.GetService(typeof(ILogger<TrackingActionFilter>))).Returns(logger);
            httpContext.Setup(c => c.Response.Headers).Returns(headers);
            return new ActionContext
            {
                HttpContext = httpContext.Object,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor(),
            };
        }
    }
}