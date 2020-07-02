using System.Waf.Foundation;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Presentation.DesignData
{
    public class MockShellService : Model, IShellService
    {
        public object ShellView { get; set; } = null!;

        public string? DocumentName { get; set; } = "Document 1";

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public IEditingCommands ActiveEditingCommands { get; set; } = null!;

        public IZoomCommands ActiveZoomCommands { get; set; } = null!;
#pragma warning restore CS8767
    }
}
