namespace Domain.Conversors;

public static class DateTimeConversors
{
    public static DateTime NormalizeToUtc(DateTime date) =>
        date.Kind == DateTimeKind.Utc ? date : date.ToUniversalTime();
}
