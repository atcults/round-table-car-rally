using Microsoft.Extensions.Logging;

internal abstract class CalculatorBase(ILogger logger)
{
    protected readonly ILogger _logger = logger;
}
