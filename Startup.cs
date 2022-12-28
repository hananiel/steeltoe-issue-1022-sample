using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Hosting;
using Steeltoe.Management.Tracing;
using System;
using System.Diagnostics;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace SampleService
{
    public class Startup

    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddRouting();
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson();
            services.AddDistributedTracingAspNetCore();

            // https://github.com/SteeltoeOSS/Steeltoe/issues/726
            var diagnosticContext = new DiagnosticContext(null);
            services.AddSingleton(diagnosticContext);
            services.AddSingleton<IDiagnosticContext>(diagnosticContext);
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
                Serilog.Debugging.SelfLog.Enable(Console.Error);
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member