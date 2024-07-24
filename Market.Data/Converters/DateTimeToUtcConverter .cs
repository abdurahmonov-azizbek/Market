using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Market.Data.Converters
{
    public class DateTimeToUtcConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeToUtcConverter()
            : base(
                  v => v.Kind == DateTimeKind.Local ? v.ToUniversalTime() : v,
                  v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
        { }
    }
}
