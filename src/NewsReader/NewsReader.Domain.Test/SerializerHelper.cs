using System.Runtime.Serialization;

namespace Test.NewsReader.Domain;

public static class SerializerHelper
{
    public static T Clone<T>(T orignalObject)
    {
        var serializer = new DataContractSerializer(typeof(T));
        using var stream = new MemoryStream();
        serializer.WriteObject(stream, orignalObject);
        stream.Position = 0;
        return (T)serializer.ReadObject(stream);
    }
}
