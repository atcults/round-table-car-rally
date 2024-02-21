internal class MarshalDataRecord
{
    public required string CarCode { get; set; }

    public List<(string, TimeOnly[])> MarshalScan { get; set; } = [];
}
