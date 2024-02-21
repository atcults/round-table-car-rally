using Microsoft.Extensions.Logging;

internal abstract class DataCompilerBase(ILogger logger)
{
    protected readonly ILogger _logger = logger;
}
