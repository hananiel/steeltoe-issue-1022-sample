using SampleService.Logging.Formatters;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using System;
using ILogger = Microsoft.Extensions.Logging.ILogger;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace SampleService.Logging
{
    public class LoggerBuilder
    {
        private const string STARTUP_LOGGING_LEVEL_ENV_VAR = "STARTUP_LOGGING_LEVEL";
        private const string STARTUP_LOGGING_FORMAT_ENV_VAR = "STARTUP_LOGGING_FORMAT";
        private const string STEELTOE_AUTOCONFIG_LOGGING_LEVEL_ENV_VAR = "STEELTOE_AUTOCONFIG_LOGGING_LEVEL";
        private const string STEELTOE_AUTOCONFIG_LOGGING_FORMAT_ENV_VAR = "STEELTOE_AUTOCONFIG_LOGGING_FORMAT";

        public static ILogger CreateStartupLogger()
        {
            var envLogLevel = Environment.GetEnvironmentVariable(STARTUP_LOGGING_LEVEL_ENV_VAR);
            var logLevel = ParseLogLevel(envLogLevel, LogEventLevel.Information);

            var envFormatter = Environment.GetEnvironmentVariable(STARTUP_LOGGING_FORMAT_ENV_VAR);
            var formatter = ParseFormatter(envFormatter);

            var logConfig = new LoggerConfiguration()
                .MinimumLevel.Is(logLevel)
                .WriteTo.Console(formatter);

            return new LoggerFactory().AddSerilog(logConfig.CreateLogger()).CreateLogger<Program>();
        }

        public static ILoggerFactory CreateAutoconfigLoggerFactory()
        {
            var envLogLevel = Environment.GetEnvironmentVariable(STEELTOE_AUTOCONFIG_LOGGING_LEVEL_ENV_VAR);
            var logLevel = ParseLogLevel(envLogLevel);

            var envFormatter = Environment.GetEnvironmentVariable(STEELTOE_AUTOCONFIG_LOGGING_FORMAT_ENV_VAR);
            var formatter = ParseFormatter(envFormatter);

            var logConfig = new LoggerConfiguration()
                .MinimumLevel.Is(logLevel)
                .WriteTo.Console(formatter);

            return new LoggerFactory().AddSerilog(logConfig.CreateLogger());
        }

        private static ITextFormatter ParseFormatter(string formatter)
        {
            var type = (formatter ?? "").ToLower();
            switch (type)
            {
                case "json":
                    return new JsonFormatter();
                case "fmt":
                default:
                    return new LogfmtFormatter();
            }
        }

        private static LogEventLevel ParseLogLevel(string logLevel, LogEventLevel defaultLevel = LogEventLevel.Error)
        {
            return Enum.TryParse<LogEventLevel>(logLevel, true, out var parsedLevel) ? parsedLevel : defaultLevel;
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member