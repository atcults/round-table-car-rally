using Microsoft.Extensions.Logging;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Program");

logger.StartingApp("RallyCalculator");

var configReader = new RallyConfigReader(factory.CreateLogger<RallyConfigReader>());

if (!configReader.Read(out RallyConfig? config))
{
    return;
}

if (config == null)
{
    logger.RallyConfigReadFailed("Rally Configuration");
    return;
}

logger.RallyConfigReadSuccessful("Rally Configuration");

var speedChartReader = new SpeedChartReader(factory.CreateLogger<SpeedChartReader>());
if (!speedChartReader.Read(out List<SpeedReferencePoint> speedChart))
{
    return;
}

logger.RallyConfigReadSuccessful("Speed Chart");

var marshalReader = new MarshalChartReader(factory.CreateLogger<MarshalChartReader>());
if (!marshalReader.Read(out List<MarshalPoint> marshalChart))
{
    return;
}

logger.RallyConfigReadSuccessful("Marshal Chart");

// Compile the chart
var compiler = new SegmentCompiler(factory.CreateLogger<SegmentCompiler>());

var compiledChart = compiler.CompileChart(speedChart, marshalChart);

Console.WriteLine();

DataPrintHelper.PrintConfiguration(config);
DataPrintHelper.PrintSpeedChart(speedChart);
DataPrintHelper.PrintMarshalChart(marshalChart);
DataPrintHelper.PrintCompiledChart(compiledChart);

// Reader Marshal Data if available
if (File.Exists(".\\data\\marshal_data.csv"))
{
    var marshalDataReader = new MarshalDataReader(factory.CreateLogger<MarshalDataReader>());
    if (!marshalDataReader.Read(marshalChart, out List<MarshalDataRecord> marshalRecords))
    { 
        return; 
    }

    DataPrintHelper.PrintMarshalData(marshalRecords);

    var marshalDataCompiler = new MarshalDataCompiler(factory.CreateLogger<MarshalDataCompiler>());
    var results = marshalDataCompiler.CompileMarshalData(config, marshalChart, marshalRecords);

    DataPrintHelper.PrintMarshalDataResults(results);

    foreach (var item in results.OrderBy(r => r.GetTotalTimePenalty))
    {
        logger.LogInformation($"Car: {item.CarCode}, Penalty: {item.GetTotalTimePenalty}");
    }
}
else
{
    logger.FileNotFound("Marshal Data", ".\\data\\marshal_data.csv");
}


logger.ShuttingDown();
