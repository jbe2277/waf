namespace Waf.NewsReader.Applications.DataModels;

public enum DisplayItemLifetime
{
    _1Month,
    _3Month,
    _6Month,
    _1Year,
    Forever
}

public static class DisplayItemLifetimeHelper
{
    public static TimeSpan? ToValue(this DisplayItemLifetime enumValue) => enumValue switch
    {
        DisplayItemLifetime._1Month => TimeSpan.FromDays(30),
        DisplayItemLifetime._3Month => TimeSpan.FromDays(90),
        DisplayItemLifetime._6Month => TimeSpan.FromDays(180),
        DisplayItemLifetime._1Year => TimeSpan.FromDays(365),
        DisplayItemLifetime.Forever => null,
        _ => throw new NotSupportedException($"The value {enumValue} is not supported."),
    };

    public static DisplayItemLifetime FromValue(TimeSpan? value)
    {
        if (value is null) return DisplayItemLifetime.Forever;
        else if (value == TimeSpan.FromDays(30)) return DisplayItemLifetime._1Month;
        else if (value == TimeSpan.FromDays(90)) return DisplayItemLifetime._3Month;
        else if (value == TimeSpan.FromDays(180)) return DisplayItemLifetime._6Month;
        else if (value == TimeSpan.FromDays(365)) return DisplayItemLifetime._1Year;
        throw new NotSupportedException($"The value {value} is not supported.");
    }
}
