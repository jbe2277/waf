using Jbe.NewsReader.Applications.Views;
using System;
using System.Composition;
using System.Waf.UnitTesting.Mocks;

namespace Test.NewsReader.Applications.Views
{
    [Export(typeof(IShellView)), Export, Shared]
    public class MockShellView : MockView, IShellView
    {
        public Action ShowStub { get; set; }

        public void Show()
        {
            ShowStub?.Invoke();
        }
    }
}
