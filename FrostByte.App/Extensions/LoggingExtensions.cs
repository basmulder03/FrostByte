using Microsoft.Extensions.Logging;
using Serilog;

namespace FrostByte.App.Extensions;

public static class LoggingExtensions
{
    public static ILoggingBuilder AddLogging(this ILoggingBuilder builder)
    {
        var logFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FrostByte", "Logs");
        Directory.CreateDirectory(logFileLocation);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.Debug()
            .WriteTo.File(
                path: logFileLocation,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7
            )
            .CreateLogger();

        builder.ClearProviders();
        builder.AddSerilog(dispose: true);

        return builder;
    }
}
