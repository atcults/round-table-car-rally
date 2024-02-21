internal class DataExtensions
{
    public static string TimeOnlyString(TimeOnly? time)
    {
        return time.HasValue ? time.Value.ToString() : "";
    }
    
    public static string DoubleString(double? value)
    {
        return value.HasValue ? value.Value.ToString() : "";
    }
}