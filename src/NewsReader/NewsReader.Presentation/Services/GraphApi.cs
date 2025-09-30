using System.Text.Json.Serialization;

namespace Waf.NewsReader.Presentation.Services;

internal sealed record User(string? UserPrincipalName, string? DisplayName, string? Mail);

internal sealed record DriveItem(string? Name, string? CTag, long Size, DateTime LastModifiedDateTime);

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(DriveItem))]
internal sealed partial class GraphApi : JsonSerializerContext
{
}
