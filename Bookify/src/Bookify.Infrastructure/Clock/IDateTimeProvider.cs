using Bookify.Application;

namespace Bookify.Infrastructure;

internal sealed class DateTimeProvider: IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}