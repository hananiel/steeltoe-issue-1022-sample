//#define LOGGING_FIX

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleService.Logging;
using Serilog;
using Steeltoe.Bootstrap.Autoconfig;
using Steeltoe.Extensions.Logging.DynamicSerilog;
using Steeltoe.Management.Endpoint;
using System;
using System.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace SampleService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LoggerBuilder.CreateStartupLogger();

            try
            {
                logger.LogInformation($"{GetAssemblyName()} (v{GetAssemblyVersion()})");

                CreateHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Error detected during service initialization");
                Environment.Exit(-1);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, bool isService = false)
        {
            var builder = Host.CreateDefaultBuilder(args);

#if LOGGING_FIX
            System.Collections.Generic.List<string> exclusions = new()
            {
                SteeltoeAssemblies.Steeltoe_Extensions_Logging_DynamicSerilogCore
            };

            return builder
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .UseSerilog((context, builder) =>
                {
                    builder.ReadFrom.Configuration(context.Configuration);
                })
                .AddSteeltoe(exclusions, loggerFactory: LoggerBuilder.CreateAutoconfigLoggerFactory());
#else
            return builder
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .AddDynamicSerilog()
              //  .AddSteeltoe(loggerFactory: LoggerBuilder.CreateAutoconfigLoggerFactory());
              .AddAllActuators();
#endif
        }

        private static string GetAssemblyName()
        {
            return Assembly.GetEntryAssembly()?.GetName().Name ?? "unknown";
        }

        private static string GetAssemblyVersion(bool fileVersion = false)
        {
            var assembly = Assembly.GetEntryAssembly();

            if (fileVersion)
            {
                var assemblyFileVersionAttribute = assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>();
                return assemblyFileVersionAttribute?.Version ?? "unknown";
            }
            else
            {
                var informationalVersionAttribute = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                return informationalVersionAttribute?.InformationalVersion ?? "unknown";
            }
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member