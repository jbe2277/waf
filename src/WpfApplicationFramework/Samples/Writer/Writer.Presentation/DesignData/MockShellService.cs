using System.Waf.Foundation;
using Waf.Writer.Applications.Services;

namespace Waf.Writer.Presentation.DesignData
{
    public class MockShellService : Model, IShellService
    {
        public MockShellService()
        {
            DocumentName = "Document 1";
        }


        public object ShellView { get; set; }

        public string DocumentName { get; set; }

        public IEditingCommands ActiveEditingCommands { get; set; }

        public IZoomCommands ActiveZoomCommands { get; set; }
    }
}
