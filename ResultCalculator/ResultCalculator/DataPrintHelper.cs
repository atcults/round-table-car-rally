using Spectre.Console;

internal static class DataPrintHelper
{
    public static void PrintConfiguration(RallyConfig config)
    {
        // Create a table
        var table = new Table
        {
            // Add title
            Title = new TableTitle($"[underline red]{config.TableName}[/]")
        };

        // Add some columns
        table.AddColumn("Parameter");
        table.AddColumn("Value");

        // Add some rows
        table.AddRow("Date", $"[green]{config.Date}[/]");
        table.AddRow("Early Penalty", $"[green]{config.EarlyPenalty}[/]");
        table.AddRow("Late Penalty", $"[green]{config.LatePenalty}[/]");
        table.AddRow("Missed Penalty", $"[green]{config.MissedPenalty}[/]");

        // Print the table
        AnsiConsole.Write(table);
    }

    public static void PrintSpeedChart(List<SpeedReferencePoint> speedChart)
    {
        // Create a table
        var table = new Table
        {
            // Add title
            Title = new TableTitle($"[underline blue]Speed Chart[/]")
        };

        // Add column definitions
        table.AddColumn("Reference");
        table.AddColumn("From KM");
        table.AddColumn("To KM");
        table.AddColumn("Average Speed");

        foreach (var item in speedChart)
        {
            table.AddRow(item.Reference, item.FromKM.ToString(), item.ToKM.ToString(), item.AverageSpeed.ToString());
        }

        AnsiConsole.Write(table);
    }

    public static void PrintMarshalChart(List<MarshalPoint> marshalChart)
    {
        // Create a table
        var table = new Table
        {
            // Add title
            Title = new TableTitle($"[underline green]Marshal Chart[/]")
        };

        // Add column definitions
        table.AddColumn("Point Name");
        table.AddColumn("Distance");
        table.AddColumn("Break");
        table.AddColumn("TTR");

        foreach (var item in marshalChart)
        {
            table.AddRow(item.PointName, item.Distance.ToString(), item.BreakDuration.ToString(), item.TimeRequired.HasValue ? item.TimeRequired.Value.ToString() : "");
        }

        AnsiConsole.Write(table);
    }

    public static void PrintCompiledChart(List<CompiledSegment> compiledSegments)
    {
        // Create a table
        var table = new Table
        {
            // Add title
            Title = new TableTitle($"[underline purple]Compiled Chart[/]")
        };

        // Add some columns
        table.AddColumn("PointName");
        table.AddColumn("CD");
        table.AddColumn("RD");
        table.AddColumn("SPEED");
        table.AddColumn("TTR");

        foreach (var item in compiledSegments)
        {
            table.AddRow(item.PointName ?? "",
                item.CumulativeDistance.HasValue ? item.CumulativeDistance.Value.ToString() : "",
                item.DistanceFromLastPoint.HasValue ? item.DistanceFromLastPoint.Value.ToString() : "",
                item.ReferenceSpeed.HasValue ? item.ReferenceSpeed.Value.ToString() : "",
                item.TimeFromLastPoint.HasValue ? item.TimeFromLastPoint.Value.ToString() : "");
        }

        AnsiConsole.Write(table);
    }
}