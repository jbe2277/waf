using System;
using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.BookLibrary.Library.Applications.Views;

namespace Test.BookLibrary.Library.Applications.Views
{
    [Export(typeof(ILendToView)), Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class MockLendToView : MockDialogView<MockLendToView>, ILendToView
    {
    }
}
