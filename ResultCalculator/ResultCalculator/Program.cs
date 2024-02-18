using Microsoft.Extensions.Logging;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Program");

logger.StartingApp("RallyCalculator");

var reader = new RallyConfigReader(factory.CreateLogger<RallyConfigReader>());

if(!reader.Read(out var config))
{
    return;
}

logger.RallyConfigRead();


logger.ShuttingDown();
