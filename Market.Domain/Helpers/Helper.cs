namespace Market.Domain.Helpers;

public static class Helper
{
    public static DateTime GetCurrentDateTime()
    {
        return DateTime.UtcNow.AddHours(5);
    }
}
