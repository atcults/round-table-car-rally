using Microsoft.Extensions.Logging;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Program");

logger.StartingApp("RallyCalculator");

var configReader = new RallyConfigReader(factory.CreateLogger<RallyConfigReader>());

if(!configReader.Read(out RallyConfig? config))
{
    return;
}

logger.RallyConfigReadSuccessful("RallyConfig");

var speedChartReader = new SpeedChartReader(factory.CreateLogger<SpeedChartReader>());
if(!speedChartReader.Read(out List<SpeedReferencePoint> speedChart))
{
    return;
}

logger.RallyConfigReadSuccessful("RallyConfig");

logger.ShuttingDown();
