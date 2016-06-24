using System;

namespace Jbe.NewsReader.Applications.ViewModels
{
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
        public static TimeSpan? ToValue(this DisplayItemLifetime enumValue)
        {
            switch (enumValue)
            {
                case DisplayItemLifetime._1Month:
                    return TimeSpan.FromDays(30);
                case DisplayItemLifetime._3Month:
                    return TimeSpan.FromDays(90);
                case DisplayItemLifetime._6Month:
                    return TimeSpan.FromDays(180);
                case DisplayItemLifetime._1Year:
                    return TimeSpan.FromDays(365);
                case DisplayItemLifetime.Forever:
                    return null;
                default:
                    throw new NotSupportedException($"The value {enumValue} is not supported.");
            }
        }

        public static DisplayItemLifetime FromValue(TimeSpan? value)
        {
            if (value == null)
            {
                return DisplayItemLifetime.Forever;
            }
            else if (value == TimeSpan.FromDays(30))
            {
                return DisplayItemLifetime._1Month;
            }
            else if (value == TimeSpan.FromDays(90))
            {
                return DisplayItemLifetime._3Month;
            }
            else if (value == TimeSpan.FromDays(180))
            {
                return DisplayItemLifetime._6Month;
            }
            else if (value == TimeSpan.FromDays(365))
            {
                return DisplayItemLifetime._1Year;
            }

            throw new NotSupportedException($"The value {value} is not supported.");
        }
    }
}
