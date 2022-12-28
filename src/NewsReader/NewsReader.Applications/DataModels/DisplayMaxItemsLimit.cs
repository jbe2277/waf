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
    public static uint? ToValue(this DisplayMaxItemsLimit enumValue) => enumValue switch
    {
        DisplayMaxItemsLimit._25 => 25,
        DisplayMaxItemsLimit._50 => 50,
        DisplayMaxItemsLimit._100 => 100,
        DisplayMaxItemsLimit._250 => 250,
        DisplayMaxItemsLimit._1000 => 1000,
        DisplayMaxItemsLimit.Unlimited => null,
        _ => throw new NotSupportedException($"The value {enumValue} is not supported."),
    };

    public static DisplayMaxItemsLimit FromValue(uint? value) => value switch
    {
        25 => DisplayMaxItemsLimit._25,
        50 => DisplayMaxItemsLimit._50,
        100 => DisplayMaxItemsLimit._100,
        250 => DisplayMaxItemsLimit._250,
        1000 => DisplayMaxItemsLimit._1000,
        null => DisplayMaxItemsLimit.Unlimited,
        _ => throw new NotSupportedException($"The value {value} is not supported."),
    };
}
