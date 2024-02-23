using System.ComponentModel.DataAnnotations;

internal class MarshalPoint
{
    public required string PointName { get; set; }

    public double Distance { get; set; }

    public int TimeToReach { get; set; }

    /// <summary>
    /// PointName is required
    /// Distance is required and non-negative
    /// </summary>
    /// <returns></returns>
    public List<ValidationResult> Validate()
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrEmpty(PointName))
        {
            results.Add(new ValidationResult("TableName is required", [nameof(PointName)]));
        }

        if (Distance < 0)
        {
            results.Add(new ValidationResult("Distance is required and non-negative", [nameof(Distance)]));
        }

        return results;
    }
}
