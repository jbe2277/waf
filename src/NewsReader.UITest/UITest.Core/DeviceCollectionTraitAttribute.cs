using Xunit.v3;

namespace UITest;

public enum DevicePlatform { Android, IOS, Windows }


[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class DeviceCollectionTraitAttribute(DevicePlatform devicePlatform) : Attribute, ICollectionAttribute, ITraitAttribute
{
    public DevicePlatform DevicePlatform { get; } = devicePlatform;

    public string Name { get; } = devicePlatform.ToString();

    public Type? Type => null;

    public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
    {
        return [new("DevicePlatform", Name)];
    }
}


public static class DeviceManager
{
    public static DevicePlatform GetDevicePlatform(Type type)
    {
        var attributeData = type.GetCustomAttributesData().Where(x => x.AttributeType == typeof(DeviceCollectionTraitAttribute)).SingleOrDefault()
            ?? throw new InvalidOperationException("The DeviceCollectionTrait attribute was not found on the test class");
        return (DevicePlatform)(int)attributeData.ConstructorArguments.Single().Value!;
    }
}