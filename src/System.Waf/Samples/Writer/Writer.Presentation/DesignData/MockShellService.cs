using System.Diagnostics.CodeAnalysis;
using System.Waf.Foundation;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Presentation.DesignData
{
    public class MockShellService : Model, IShellService
    {
        public object ShellView { get; set; } = null!;

        public string? DocumentName { get; set; } = "Document 1";

        [AllowNull]
        public IEditingCommands ActiveEditingCommands { get; set; } = null!;

        [AllowNull]
        public IZoomCommands ActiveZoomCommands { get; set; } = null!;
    }
}
