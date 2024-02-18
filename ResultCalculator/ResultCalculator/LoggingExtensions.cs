using Microsoft.Extensions.Logging;

internal static partial class LoggingExtensions
{
    // Starting: {AppName}
    [LoggerMessage(LogLevel.Information, "Starting: {AppName}")]
    public static partial void StartingApp(this ILogger logger, string? appName);

    // Application is shutting down
    [LoggerMessage(LogLevel.Information, "Application is shutting down")]
    public static partial void ShuttingDown(this ILogger logger);

    // {FileType} file not found at: {FilePath}
    [LoggerMessage(LogLevel.Error, "{FileType} file not found at: {FilePath}")]
    public static partial void FileNotFound(this ILogger logger, string fileType, string filePath);

    // File is empty
    [LoggerMessage(LogLevel.Error, "File is empty: {FilePath}")]
    public static partial void FileIsEmpty(this ILogger logger, string filePath);

    // File is expecting {Number} of lines. Found {Number} lines.
    [LoggerMessage(LogLevel.Error, "File is expecting {Expected} of lines. Found {Actual} lines.")]
    public static partial void FileLineCountMismatch(this ILogger logger, int expected, int actual);

    // Rally Configuration File read successfully
    [LoggerMessage(LogLevel.Information, "Rally Configuration File read successfully")]
    public static partial void RallyConfigRead(this ILogger logger);
}