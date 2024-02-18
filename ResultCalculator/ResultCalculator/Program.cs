using Microsoft.Extensions.Logging;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Program");

logger.StartingApp("RallyCalculator");

var configReader = new RallyConfigReader(factory.CreateLogger<RallyConfigReader>());

if(!configReader.Read(out RallyConfig? config))
{
    return;
}

logger.RallyConfigReadSuccessful("Rally Configuration");

var speedChartReader = new SpeedChartReader(factory.CreateLogger<SpeedChartReader>());
if(!speedChartReader.Read(out List<SpeedReferencePoint> speedChart))
{
    return;
}

logger.RallyConfigReadSuccessful("Speed Chart");

var marshalReader = new MarshalChartReader(factory.CreateLogger<MarshalChartReader>());
if(!marshalReader.Read(out List<MarshalPoint> marshalChart))
{
    return;
}

logger.RallyConfigReadSuccessful("Marshal Chart");

logger.ShuttingDown();
