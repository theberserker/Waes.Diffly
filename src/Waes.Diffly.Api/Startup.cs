using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waes.Diffly.Api.Infrastructure;
using Waes.Diffly.Core.Domain;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Interfaces.Domain;
using Waes.Diffly.Core.Interfaces.Repositories;
using Waes.Diffly.Core.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace Waes.Diffly.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                //builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Add framework services.
            //services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc(config =>
                {
                    config.Filters.Add(new GlobalLoggingExceptionFilter(loggerFactory));
                    config.Filters.Add(new CustomExceptionFilterAttribute());
                });
            services.Configure<RouteOptions>(options =>options.ConstraintMap.Add(nameof(DiffSide), typeof(DiffSideRouteConstraint)));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "4C Insights Diffly API", Version = "v1" });
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Waes.Diffly.Api.xml");
                c.IncludeXmlComments(filePath);
            });

            // Application services
            services.AddSingleton<IDiffRepository, DiffRepository>();
            services.AddSingleton<IDiffService, DiffService>(); // this can be a singleton until it will have some state.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //app.UseApplicationInsightsRequestTelemetry();

            //app.UseApplicationInsightsExceptionTelemetry();
            
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "4C Insights Diffly API");
            });
        }
    }
}
