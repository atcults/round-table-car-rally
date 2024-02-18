using Microsoft.Extensions.Logging;
using Spectre.Console;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Program");

logger.StartingApp("RallyCalculator");

var configReader = new RallyConfigReader(factory.CreateLogger<RallyConfigReader>());

if(!configReader.Read(out RallyConfig? config))
{
    return;
}

if(config == null)
{
    logger.RallyConfigReadFailed("Rally Configuration");
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

Console.WriteLine();

AnsiConsole.MarkupLine($"[underline red]{config.TableName}[/]");

// Create a table
var table = new Table();

// Add some columns
table.AddColumn("Parameter");
table.AddColumn("Value");

// Add some rows
table.AddRow("Date", $"[green]{config.Date}[/]");
table.AddRow("Early Penalty", $"[green]{config.EarlyPenalty}[/]");
table.AddRow("Late Penalty", $"[green]{config.LatePenalty}[/]");
table.AddRow("Missed Penalty", $"[green]{config.MissedPenalty}[/]");

// Render the table to the console
AnsiConsole.Write(table);

Console.WriteLine();

logger.ShuttingDown();
