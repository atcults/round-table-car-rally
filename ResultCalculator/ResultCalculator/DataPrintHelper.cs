using Humanizer;
using Spectre.Console;
using System.Globalization;

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
        table.AddRow("When", $"[green]{config.Date.Humanize(culture: new CultureInfo("en-US", false))}[/]");
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
        table.AddColumn("TTR");

        foreach (var item in marshalChart)
        {
            table.AddRow(item.PointName,
                item.Distance.ToString(),
                item.TimeToReach.ToString());
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
        table.AddColumn("MT");

        foreach (var item in compiledSegments)
        {
            table.AddRow(item.PointName ?? "",
                item.CumulativeDistance.ToString(),
                item.DistanceFromLastPoint.ToString(),
                item.ReferenceSpeed.ToString(),
                item.TimeFromLastPoint.ToString(),
                item.MarshalTime.HasValue ? item.MarshalTime.Value.ToString() : "");
        }

        AnsiConsole.Write(table);
    }

    internal static void PrintMarshalData(List<MarshalDataRecord> marshalRecords)
    {
        if (marshalRecords.Count == 0)
        {
            AnsiConsole.Markup("[red]No Marshal Data found[/]");
            return;
        }

        // Create a table
        var table = new Table
        {
            // Add title
            Title = new TableTitle($"[underline yellow]Marshal Data Chart[/]")
        };

        // Add some columns
        table.AddColumn("Car Code");

        foreach (var item in marshalRecords.First().MarshalScan)
        {
            table.AddColumn(item.Item1);
        }

        foreach (var item in marshalRecords)
        {
            List<string> values =
            [
                item.CarCode
            ];

            foreach (var dataPoint in item.MarshalScan)
            {
                values.Add(string.Join(" | ", dataPoint.Item2));    
            }

            table.AddRow(values.ToArray());
        }

        AnsiConsole.Write(table);
    }

    internal static void PrintMarshalDataResults(List<CarRallyResult> results)
    {
        if (results.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No Marshal Data Results found[/]");
            return;
        }

        AnsiConsole.MarkupLine("[underline bold green]Round Table Rally Result[/]");

        foreach (var result in results)
        {
            var table = new Table
            {
                // Add title
                Title = new TableTitle($"[green]{result.CarCode} Penalty: {result.GetTotalTimePenalty}[/]")
            };

            // Add some columns
            table.AddColumn("Point Name");
            table.AddColumn("Is Missed");
            table.AddColumn("Time Penalty");
            table.AddColumn("Data Points");

            foreach (var item in result.MarshalPointRecords)
            {
                table.AddRow(item.PointName, item.IsMissed.ToString(), item.TimePenalty.ToString(), string.Join(" | ", item.ScannedData));
            }

            AnsiConsole.Write(table);
        }
    }
}