using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Services.Bank;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.API.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Commands;
using PaymentGateway.API.Endpoints.Queries;
using PaymentGateway.API.Services;
using PaymentGateway.Services;

namespace PaymentGateway.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<RequestTrackingService>();
            services.AddControllers()
            .AddNewtonsoftJson(options => options.UseMemberCasing())
            .ConfigureApiBehaviorOptions(
                options =>
                {
                    options.SuppressInferBindingSourcesForParameters = true;
                    options.InvalidModelStateResponseFactory = ((c) => new BadRequestObjectResult(c.ModelState));
                }
            );
            services.AddDbContext<DataStoreDbContext>(
                optionBuilder => optionBuilder.UseMySQL(Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING"))
            );
            services.AddScoped<IPayoutCommand, PayoutCommand>();
            services.AddScoped<IRetrievePaymentQuery, RetrievePaymentQuery>();
            services.AddScoped<IBankService>(
                s => new BankService(
                    s.GetService<RequestTrackingService>(),
                    s.GetService<ILogger<BankService>>(),
                        new Uri(Environment.GetEnvironmentVariable("BANK_SERVICE_URL")),
                        s.GetService<IHttpClientFactory>()
                     )
            );
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataStoreDbContext dataStoreDbContext, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                dataStoreDbContext.Database.EnsureCreated();
            }
            dataStoreDbContext.Database.Migrate();
            app.UseRouting();
            app.UseExceptionHandler(
                a => a.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var requestTrackingService = context.RequestServices.GetRequiredService<RequestTrackingService>();
                    var ex = exceptionHandlerPathFeature.Error;
                    logger.LogError(ex, ex.Message);
                    var result = Newtonsoft.Json.JsonConvert.SerializeObject(new StandardErrorResponse
                    {
                        Type = ex.GetType().ToString(),
                        Error = ex.Message,
                        RequestTraceId = requestTrackingService.RequestTraceId.ToString()
                    });
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result);
                })
            );
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
