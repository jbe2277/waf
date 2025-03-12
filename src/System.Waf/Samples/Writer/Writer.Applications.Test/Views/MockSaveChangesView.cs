using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications.Views;

public class MockSaveChangesView : MockDialogView<MockSaveChangesView, SaveChangesViewModel>, ISaveChangesView
{
}
