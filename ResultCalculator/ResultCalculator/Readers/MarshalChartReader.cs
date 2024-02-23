using Csv;
using Microsoft.Extensions.Logging;

internal sealed class MarshalChartReader(ILogger<MarshalChartReader> logger) : CsvReaderBase(logger)
{
    /// <summary>
    /// PointName is required
    /// Distance cannot be negative
    /// BreakTime must be positive
    /// </summary>
    /// <param name="marshalChart"></param>
    /// <returns></returns>
    public bool Read(out List<MarshalPoint> marshalChart)
    {
        var configPath = ConfigProvider.GetMarshalChartPath();

        marshalChart = [];

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
                var pointName = item["Point Name"];

                if(string.IsNullOrEmpty(pointName))
                {
                    _logger.InvalidDataFormat("Point Name", "Point Name is required");
                    return false;
                }

                if (!double.TryParse(item["Distance"], out double distance))
                {
                    _logger.InvalidDataFormat("Distance", "Distance must be a non-negative number");
                    return false;
                }

                var marshalPoint = new MarshalPoint
                {
                    PointName = pointName,
                    Distance = distance,
                };

                var results = marshalPoint.Validate();
                if (results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        _logger.InvalidDataFormat(string.Join(",", result.MemberNames), result.ErrorMessage);
                    }
                    return false;
                }

                marshalChart.Add(marshalPoint);
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

