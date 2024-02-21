internal class CarRallyResult
{
    public required string CarCode { get; set; }

    public List<CarMarshalPointRecord> MarshalPointRecords { get; set; } = [];

    public int GetTotalTimePenalty => MarshalPointRecords.Sum(x => x.TimePenalty);

    internal class CarMarshalPointRecord
    {
        // Car Participant Code
        public required string PointName { get; set; }

        // Capture Data Points
        public TimeOnly[] ScannedData { get; set; } = [];

        // Actual time the car was scanned
        public TimeOnly? ArrivalTime { get; set; }

        // Actual departure time from the marshal point
        public TimeOnly? DepartureTime { get; set; }

        // If the car was missed
        public bool IsMissed { get; set; }

        // Break duration at the marshal point
        public int BreakDuration()
        {
            if (ArrivalTime.HasValue && DepartureTime.HasValue)
            {
                return (DepartureTime.Value - ArrivalTime.Value).Minutes;
            }

            return 0;
        }

        // Best time to reach the marshal point
        public int BestTimeFromLastPoint { get; set; }

        // Time taken from the last point
        public int ActualTimeFromLastPoint { get; set; }

        // Difference between the actual and expected time
        public int TimeDifference { get; set; }

        // Penalty for the time difference
        public int TimePenalty { get; set; }
    }
}
