using System;

namespace Waf.NewsReader.Applications.DataModels;

public enum DisplayMaxItemsLimit
{
    _25,
    _50,
    _100,
    _250,
    _1000,
    Unlimited
}

public static class DisplayMaxItemsLimitHelper
{
    public static uint? ToValue(this DisplayMaxItemsLimit enumValue)
    {
        switch (enumValue)
        {
            case DisplayMaxItemsLimit._25:
                return 25;
            case DisplayMaxItemsLimit._50:
                return 50;
            case DisplayMaxItemsLimit._100:
                return 100;
            case DisplayMaxItemsLimit._250:
                return 250;
            case DisplayMaxItemsLimit._1000:
                return 1000;
            case DisplayMaxItemsLimit.Unlimited:
                return null;
            default:
                throw new NotSupportedException($"The value {enumValue} is not supported.");
        }
    }

    public static DisplayMaxItemsLimit FromValue(uint? value)
    {
        switch (value)
        {
            case 25:
                return DisplayMaxItemsLimit._25;
            case 50:
                return DisplayMaxItemsLimit._50;
            case 100:
                return DisplayMaxItemsLimit._100;
            case 250:
                return DisplayMaxItemsLimit._250;
            case 1000:
                return DisplayMaxItemsLimit._1000;
            case null:
                return DisplayMaxItemsLimit.Unlimited;
            default:
                throw new NotSupportedException($"The value {value} is not supported.");
        }
    }
}
