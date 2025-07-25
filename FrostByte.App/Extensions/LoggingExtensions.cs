using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace FrostByte.App.Extensions;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddLogging(this ILoggingBuilder builder)
    {
        var logFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FrostByte", "Logs");
        Directory.CreateDirectory(logFileLocation);
        const string logTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Debug(
                outputTemplate: logTemplate
            )
            .WriteTo.Console(
                restrictedToMinimumLevel: LogEventLevel.Information,
                outputTemplate: logTemplate
            )
            .WriteTo.File(
                path: logFileLocation,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7,
                outputTemplate: logTemplate
            )
            .CreateLogger();

        builder.ClearProviders();
        builder.AddSerilog(dispose: true);

        return builder;
    }
}
