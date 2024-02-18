using Csv;
using Microsoft.Extensions.Logging;

// Reading ./data/config.csv file with "Csv" library
internal class RallyConfigReader(ILogger<RallyConfigReader> logger) : CsvReaderBase(logger)
{
    private const string configPath = "./data/config.csv";
    public bool Read(out RallyConfig? config)
    {
        // Check if the file exists
        if (!File.Exists(configPath))
        {
            _logger.FileNotFound("Configuration", configPath);
            config = null;
            return false;
        }

        var csv = File.ReadAllText(configPath);

        var lines = CsvReader.ReadFromText(csv, _csvOptions);

        if(!lines.Any())
        {
            _logger.FileIsEmpty(configPath);
            config = null;
            return false;
        }

        if(lines.Count() != 1)
        {
            _logger.FileLineCountMismatch(1, lines.Count());
            config = null;
            return false;
        }

        config = new RallyConfig
        {
            TableName = "Rally",
            Year = 2021,
            Date = new DateOnly(2021, 10, 1),
            EarlyPenalty = 3,
            LatePenalty = 1,
            MissedPenalty = 100
        };

        return true;
    }
}

