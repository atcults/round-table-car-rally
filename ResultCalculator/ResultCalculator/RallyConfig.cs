internal class RallyConfig
{
    public required string TableName { get; set; }
    
    public required int Year { get; set; }
    
    public required DateOnly Date { get; set; }
    
    public required int EarlyPenalty { get; set; }
    
    public required int LatePenalty { get; set; }
    
    public required int MissedPenalty { get; set; }
}
