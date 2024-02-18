using Csv;
using Microsoft.Extensions.Logging;

// Reading ./data/speed_chart.csv file with "Csv" library
internal class SpeedChartReader(ILogger<SpeedChartReader> logger) : CsvReaderBase(logger)
{
    private const string configPath = "./data/speed_chart.csv";

    public bool Read(out List<SpeedReferencePoint> speedChart)
    {
        speedChart = [];

        // Check if the file exists
        if (!File.Exists(configPath))
        {
            _logger.FileNotFound("SpeedChart", configPath);
            return false;
        }

        var csv = File.ReadAllText(configPath);

        var lines = CsvReader.ReadFromText(csv, _csvOptions);

        if (!lines.Any())
        {
            _logger.FileIsEmpty(configPath);
            return false;
        }

        try
        {
            foreach (var item in lines)
            {
                var reference = item["Reference"];
                if (!double.TryParse(item["From KM"], out double fromKM))
                {
                    _logger.InvalidDataFormat("From KM", "From KM must be a number");
                    return false;
                }
                if (!double.TryParse(item["To KM"], out double toKM))
                {
                    _logger.InvalidDataFormat("To KM", "To KM must be a number");
                    return false;
                }
                if (!double.TryParse(item["Average Speed"], out double averageSpeed))
                {
                    _logger.InvalidDataFormat("Average Speed", "Average Speed must be a number");
                    return false;
                }

                var speedReferencePoint = new SpeedReferencePoint
                {
                    Reference = reference,
                    FromKM = fromKM,
                    ToKM = toKM,
                    AverageSpeed = averageSpeed
                };

                var results = speedReferencePoint.Validate();
                if (results.Count > 0)
                {
                    foreach (var validationResult in results)
                    {
                        _logger.InvalidDataFormat(string.Join(",", validationResult.MemberNames), validationResult.ErrorMessage);
                    }
                    return false;
                }

                speedChart.Add(speedReferencePoint);
            }
        }
        catch (Exception ex)
        {
            _logger.UnhandledException(ex.Message);
            return false;
        }

        return true;
    }
}

