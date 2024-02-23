internal class MarshalDataRecord
{
    public required int CarNumber { get; set; }

    public List<(string, TimeOnly[])> MarshalScan { get; set; } = [];
}
