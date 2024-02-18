using Microsoft.Extensions.Logging;

internal static partial class LoggingExtensions
{
    [LoggerMessage(LogLevel.Information, "Starting: {AppName}")]
    public static partial void StartingApp(this ILogger logger, string? appName);
}