namespace Market.Domain.Extensions;

public static class Helper
{
    public static DateTime GetCurrentDateTime()
    {
        return DateTime.UtcNow.AddHours(5);
    }
}
