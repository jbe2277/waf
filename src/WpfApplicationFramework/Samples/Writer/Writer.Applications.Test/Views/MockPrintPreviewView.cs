using System;
using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications.Views
{
    [Export(typeof(IPrintPreviewView)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MockPrintPreviewView : MockView, IPrintPreviewView
    {
        public bool FitToWidthCalled { get; set; }
        
        public void FitToWidth()
        {
            FitToWidthCalled = true;
        }
    }
}
