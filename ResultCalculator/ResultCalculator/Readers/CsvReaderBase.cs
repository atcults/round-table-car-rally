using Csv;
using Microsoft.Extensions.Logging;

internal abstract class CsvReaderBase(ILogger logger)
{
    protected readonly ILogger _logger = logger;

    protected readonly CsvOptions _csvOptions = new()
    {
        //Allows skipping of initial rows without csv data
        //SkipRow = (row, idx) => string.IsNullOrEmpty(row) || row[0] == '#',
        //Separator = '\0', // Autodetects based on first row
        RowsToSkip = 0,

        // Autodetects based on first row
        Separator = ',',

        // Can be used to trim each cell
        TrimData = true,

        // Can be used for case-insensitive comparison for names
        Comparer = null,

        // Assumes first row is a header row
        HeaderMode = HeaderMode.HeaderPresent,

        // Checks each row immediately for column count
        ValidateColumnCount = false,

        // Allows for accessing invalid column names
        ReturnEmptyForMissingColumn = false,

        // A collection of alternative column names
        Aliases = null,

        // Respects new line (either \r\n or \n) characters inside field values enclosed in double quotes.
        AllowNewLineInEnclosedFieldValues = true,

        // Allows the sequence "\"" to be a valid quoted value (in addition to the standard """")
        AllowBackSlashToEscapeQuote = false,

        // Allows the single-quote character to be used to enclose field values
        AllowSingleQuoteToEncloseFieldValues = false,

        // The new line string to use when multiline field values are read (Requires "AllowNewLineInEnclosedFieldValues" to be set to "true" for this to have any effect.)
        NewLine = Environment.NewLine
    };
}

