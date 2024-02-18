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

        // Get the first line
        var line = lines.First();

        try
        {
            config = new RallyConfig
            {
                TableName = line["Table Name"],
                Date = DateOnly.ParseExact(line["DATE"], "ddMMyy"),
                EarlyPenalty = int.Parse(line["Early Penalty"]),
                LatePenalty = int.Parse(line["Late Penalty"]),
                MissedPenalty = int.Parse(line["Missed Penalty"])
            };

            var results = config.Validate();
            if (results.Count > 0)
            {
                foreach (var validationResult in results)
                {
                    _logger.InvalidDataFormat(string.Join(",", validationResult.MemberNames), validationResult.ErrorMessage);
                }
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.UnhandledException(ex.Message);
            config = null;
            return false;
        }        

        return true;
    }
}

