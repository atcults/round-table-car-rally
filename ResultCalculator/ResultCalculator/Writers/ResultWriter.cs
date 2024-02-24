using Spectre.Console;

internal class ResultWriter
{
    public static void CreateRallyResultPdf(List<CarRallyResult> results)
    {
        var values = new List<string>
        {
            "Rank,Car Code,Started,Finished,Total Time,Penalty,Missed"
        };

        int rank = 0;
        var lastPenalty = 0;
        int sameRankCount = 0;
        foreach (var item in results.OrderBy(x => x.GetTotalPenalty))
        {
            var startTime = item.MarshalPointRecords.First().DepartureTime;
            var endTime = item.MarshalPointRecords.Last().ArrivalTime;

            var totalTime = (endTime.HasValue && startTime.HasValue) ? (endTime.Value - startTime.Value).TotalMinutes : 0;

            var totalMissedPoints = item.MarshalPointRecords.Count(x => x.IsMissed);
            if (lastPenalty != item.GetTotalPenalty)
            {
                rank += sameRankCount;
                rank++;
                sameRankCount = 0;
            }

            values.Add($"{rank}," +
                $"{item.CarNumber}," +
                $"{DataExtensions.TimeOnlyString(startTime)}," +
                $"{DataExtensions.TimeOnlyString(endTime)}," +
                $"{TimeSpan.FromMinutes(totalTime)}," +
                $"{item.GetTotalPenalty}," +
                $"{totalMissedPoints}");

            if (lastPenalty == item.GetTotalPenalty)
            {
                sameRankCount++;
            }

            lastPenalty = item.GetTotalPenalty;

            PrintCarResult(rank, item);
        }

        new PdfTableCreator().CreateTablePdf("Rally Result 2024", [.. values], "RallyResult");
    }

    private static void PrintCarResult(int rank, CarRallyResult result)
    {
        var lines = new List<string>
            {
                "Point,Arrival,Departure,Missed,Time Taken,Expected,Penalty"
            };

        foreach (var item in result.MarshalPointRecords)
        {
            lines.Add($"{item.PointName}," +
                $"{DataExtensions.TimeOnlyString(item.ArrivalTime)}," +
                $"{DataExtensions.TimeOnlyString(item.DepartureTime)}," +
                $"{item.IsMissed}," +
                $"{item.ActualTimeFromLastPoint}," +
                $"{item.BestTimeFromLastPoint}," +
                $"{item.TimePenalty}");
        }

        new PdfTableCreator().CreateTablePdf($"Car #{result.CarNumber} Rank:{rank} Penalty:{result.GetTotalPenalty}", [.. lines], "RESULT-" + result.CarNumber.ToString());
    }
}
