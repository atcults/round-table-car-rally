internal class MarshalDataRecord()
{
    public required string CarCode { get; set; }

    public List<(string, TimeOnly?, TimeOnly?)> TimeCaptured { get; set; } = [];
}
