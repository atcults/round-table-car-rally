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
        }

        new PdfTableCreator().CreateTablePdf("Rally Result 2024", [.. values]);
    }
}
