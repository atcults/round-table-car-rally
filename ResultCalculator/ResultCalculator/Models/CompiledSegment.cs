internal class CompiledSegment
{
    public string? PointName { get; set; }

    public bool IsMarshalPoint { get; set; }
    
    public double CumulativeDistance { get; set; }
    
    public double DistanceFromLastPoint { get; set; }
    
    public double ReferenceSpeed { get; set; }
    
    public double TimeFromLastPoint { get; set; }

    public double? MarshalTime { get; set; }
}
