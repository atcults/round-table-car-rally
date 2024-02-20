using Microsoft.Extensions.Logging;

internal class SegmentCompiler(ILogger<SegmentCompiler> logger) : CalculatorBase(logger)
{
    public List<CompiledSegment> CompileChart(List<SpeedReferencePoint> speedReferences, List<MarshalPoint> marshalPoints)
    {
        _logger.LogInformation("Compiling the speed chart");

        var compiledSegments = new List<CompiledSegment>();
        double lastDistance = 0;
        double distanceDiff = 0;

        foreach (var speedReference in speedReferences)
        {
            var referenceMarshalPoints = marshalPoints.Where(mp => mp.Distance >= speedReference.FromKM && mp.Distance <= speedReference.ToKM);

            // Check for any MarshalPoints within this speed reference segment
            foreach (var marshalPoint in referenceMarshalPoints.ToList())
            {
                distanceDiff = Math.Round(marshalPoint.Distance - lastDistance, 2);

                if (distanceDiff < 0)
                {
                    _logger.LogError("Marshal chart is not in order");
                    return [];
                }

                var marshalSegment = new CompiledSegment
                {
                    PointName = marshalPoint.PointName,
                    IsMarshalPoint = true,
                    CumulativeDistance = marshalPoint.Distance,
                    DistanceFromLastPoint = distanceDiff,
                    ReferenceSpeed = speedReference.AverageSpeed,
                    TimeFromLastPoint = Math.Round((distanceDiff * 60) / speedReference.AverageSpeed, 2)
                };
                lastDistance = marshalPoint.Distance;
                compiledSegments.Add(marshalSegment);
            }

            distanceDiff = Math.Round(speedReference.ToKM - lastDistance, 2);

            if (distanceDiff < 0)
            {
                _logger.LogError("Speed chart is not in order");
                return [];
            }

            var segment = new CompiledSegment
            {
                PointName = speedReference.Reference,
                IsMarshalPoint = false,
                CumulativeDistance = speedReference.ToKM,
                DistanceFromLastPoint = distanceDiff,
                ReferenceSpeed = speedReference.AverageSpeed,
                TimeFromLastPoint = Math.Round((distanceDiff * 60) / speedReference.AverageSpeed, 2)
            };

            compiledSegments.Add(segment);
        }

        return compiledSegments;
    }
}
