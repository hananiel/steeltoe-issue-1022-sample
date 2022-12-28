using Serilog.Events;
using Serilog.Formatting;
using Serilog.Logfmt;
using System.IO;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace SampleService.Logging.Formatters
{
    public class LogfmtFormatter : ITextFormatter
    {
        private readonly Serilog.Logfmt.LogfmtFormatter formatter;

        public LogfmtFormatter(bool printStacktraces = true, bool includeProperties = true, bool preserveCase = false, bool forceSingleLine = false, bool escapeDoubleQuotes = false)
        {
            formatter = new Serilog.Logfmt.LogfmtFormatter((options) =>
            {
                if (includeProperties) { options = options.IncludeAllProperties(); }
                if (preserveCase) { options = options.PreserveCase(); }
                if (escapeDoubleQuotes) { options.OnDoubleQuotes(opt => opt.Escape()); }
                if (printStacktraces)
                {
                    options = options.OnException(opt => opt
                        .LogExceptionData(LogfmtExceptionDataFormat.Message | LogfmtExceptionDataFormat.Level)
                        .LogStackTrace(forceSingleLine ? LogfmtStackTraceFormat.SingleLine : LogfmtStackTraceFormat.All)
                    );
                }
            });
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            formatter.Format(logEvent, output);
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
