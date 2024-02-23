using Microsoft.Extensions.Logging;

internal class MarshalDataCompiler(ILogger<MarshalDataCompiler> logger) : DataCompilerBase(logger)
{
    public List<CarRallyResult> CompileMarshalData(RallyConfig config, List<MarshalPoint> marshalPoints, List<MarshalDataRecord> marshalDataRecords)
    {
        _logger.LogInformation("Compiling the marshal data");

        var results = new List<CarRallyResult>();

        foreach (var marshalDataRecord in marshalDataRecords)
        {
            var carRallyResult = new CarRallyResult
            {
                CarCode = marshalDataRecord.CarCode
            };

            // Initiate START point.
            var startPoint = marshalPoints[0];
            TimeOnly[] startMdp = marshalDataRecord.MarshalScan[0].Item2;

            var startPointRecord = new CarRallyResult.CarMarshalPointRecord
            {
                PointName = startPoint.PointName,
                ScannedData = startMdp,
                IsMissed = true,
                TimePenalty = config.MissedPenalty,
                BestTimeFromLastPoint = 0,
                ActualTimeFromLastPoint = 0,
                DepartureTime = config.Time,
                TimeDifference = 0
            };

            if (startMdp.Length > 0)
            {
                startPointRecord.DepartureTime = startMdp.LastOrDefault();
                startPointRecord.IsMissed = false;
                startPointRecord.TimePenalty = 0;
            }

            carRallyResult.MarshalPointRecords.Add(startPointRecord);

            // Loop through the marshal points
            for (int pIndex = 1; pIndex < marshalPoints.Count; pIndex++)
            {
                MarshalPoint? currentMarshalPoint = marshalPoints[pIndex];

                TimeOnly[] currentMdp = marshalDataRecord.MarshalScan[pIndex].Item2;

                var marshalPointRecord = new CarRallyResult.CarMarshalPointRecord
                {
                    PointName = currentMarshalPoint.PointName,
                    ScannedData = currentMdp,
                    IsMissed = true,
                    TimePenalty = config.MissedPenalty,
                    BestTimeFromLastPoint = currentMarshalPoint.TimeToReach
                };

                // Calculate time taken from the last point
                var lastMarshalPointRecord = carRallyResult.MarshalPointRecords.Last();

                if (!lastMarshalPointRecord.DepartureTime.HasValue)
                {
                    var errorMsg = "Could not find the departure time for the previous point. This should not happen. Please check the data and try again.";
                    _logger.LogError(errorMsg);
                    throw new InvalidOperationException(errorMsg);
                }

                var lastDepartureTime = lastMarshalPointRecord.DepartureTime.Value;

                // Set the actual arrival and departure time
                if (currentMdp.Length > 0 && currentMdp.First() > lastDepartureTime)
                {
                    marshalPointRecord.IsMissed = false;

                    var arrivalTime = currentMdp.First();
                    var departureTime = currentMdp.Last();

                    marshalPointRecord.ArrivalTime = arrivalTime;
                    marshalPointRecord.DepartureTime = departureTime;
                    
                    marshalPointRecord.ActualTimeFromLastPoint = (int)(marshalPointRecord.ArrivalTime.Value - lastDepartureTime).TotalMinutes;
                    marshalPointRecord.TimeDifference = marshalPointRecord.ActualTimeFromLastPoint - marshalPointRecord.BestTimeFromLastPoint;
                    marshalPointRecord.TimePenalty = marshalPointRecord.TimeDifference == 0 ? 0
                        : marshalPointRecord.TimeDifference > 0 ? config.LatePenalty * marshalPointRecord.TimeDifference
                        : -1 * config.EarlyPenalty * marshalPointRecord.TimeDifference;
                }
                else
                {
                    // Set departure time to the previous point's departure time plus best time to reach the current point
                    marshalPointRecord.DepartureTime = lastDepartureTime.AddMinutes(currentMarshalPoint.TimeToReach);
                }

                carRallyResult.MarshalPointRecords.Add(marshalPointRecord);
            }

            results.Add(carRallyResult);
        }

        return results;
    }
}
