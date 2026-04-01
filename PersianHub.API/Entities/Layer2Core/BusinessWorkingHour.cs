namespace PersianHub.API.Entities.Layer2Core;

public class BusinessWorkingHour
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly? OpenTime { get; set; }
    public TimeOnly? CloseTime { get; set; }
    public bool IsClosed { get; set; }

    // Navigation
    public Business Business { get; set; } = null!;
}
