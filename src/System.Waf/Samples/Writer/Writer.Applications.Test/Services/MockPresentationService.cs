using System.ComponentModel.Composition;
using Waf.Writer.Applications.Services;

namespace Test.Writer.Applications.Services
{
    [Export(typeof(IPresentationService)), Export]
    public class MockPresentationService : IPresentationService
    {
        public bool InitializeCulturesCalled { get; set; }
        
        public double VirtualScreenWidth { get; set; }

        public double VirtualScreenHeight { get; set; }

        public void InitializeCultures() => InitializeCulturesCalled = true;
    }
}
