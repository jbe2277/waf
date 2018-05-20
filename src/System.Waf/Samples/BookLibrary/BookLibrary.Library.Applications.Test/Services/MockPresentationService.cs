using System.ComponentModel.Composition;
using Waf.BookLibrary.Library.Applications.Services;

namespace Test.BookLibrary.Library.Applications.Services
{
    [Export(typeof(IPresentationService)), Export]
    public class MockPresentationService : IPresentationService
    {
        public bool InitializeCulturesCalled { get; set; }

        public double VirtualScreenWidth { get; set; }

        public double VirtualScreenHeight { get; set; }

        public void InitializeCultures()
        {
            InitializeCulturesCalled = true;
        }
    }
}
