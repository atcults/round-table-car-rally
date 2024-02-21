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

            for (int pIndex = 0; pIndex < marshalPoints.Count; pIndex++)
            {
                MarshalPoint? currentMarshalPoint = marshalPoints[pIndex];
                
                var marshalPointRecord = new CarRallyResult.CarMarshalPointRecord
                {
                    PointName = currentMarshalPoint.PointName,
                    ScannedData = marshalDataRecord.MarshalScan[pIndex].Item2,
                    IsMissed = true,
                    TimePenalty = config.MissedPenalty,
                    ExpectedTimeToReach = currentMarshalPoint.TimeToReach
                };

                carRallyResult.MarshalPointRecords.Add(marshalPointRecord);
            }

            results.Add(carRallyResult);
        }

        return results;
    }
}
