internal class MarshalDataRecord
{
    public required string CarCode { get; set; }

    public Dictionary<string, (TimeOnly?, TimeOnly?)> TimeCaptured { get; set; } = [];
}
