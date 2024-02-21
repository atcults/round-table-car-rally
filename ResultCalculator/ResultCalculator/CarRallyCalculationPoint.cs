internal class CarRallyCalculationPoint
{
    // Car Participant Code
    public required string CarCode { get; set; }

    // Actual time the car was scanned
    public TimeOnly? ActualArrivalTime { get; set;}

    // Actual departure time from the marshal point
    public TimeOnly? ActualDepartureTime { get; set; }

    // If the car was missed
    public bool IsMissed { get; set; }

    // Proxy time for the arrival. If the car was missed, this will be the time the car was expected to arrive
    public TimeOnly ProxyArrivalScanTime { get; set; }

    // Proxy time for the departure. If the car was missed, this will be the time the car was expected to depart
    public TimeOnly ProxyDepartureScanTime { get; set; }

    // Break duration at the marshal point
    public int BreakDuration { get; set; }

    // Time the car was expected to be scanned
    public int ExpectedTimeToReach { get; set; }

    // Time the car was actually scanned
    public int ActualTimeToReach { get; set; }

    // Difference between the actual and expected time
    public int TimeDifference { get; set; }

    // Penalty for the time difference
    public int TimePenalty { get; set; }
}
