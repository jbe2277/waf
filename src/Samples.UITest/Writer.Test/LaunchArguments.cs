using System.Runtime.CompilerServices;

namespace UITest.Writer;

public record LaunchArguments(string? UICulture = "en-US", string? Culture = "en-US", bool? DefaultSettings = true, string? AdditionalArguments = null)
{
    private string? CreateArg(object? value, [CallerArgumentExpression(nameof(value))] string propertyName = null!) => value is null ? null : $"--{propertyName}=\"{value}\"";

    public string ToArguments()
    {
        string?[] args = [ CreateArg(UICulture), CreateArg(Culture), CreateArg(DefaultSettings), CreateArg(AdditionalArguments) ];
        return string.Join(" ", args.Where(x => !string.IsNullOrEmpty(x)));
    }
}
