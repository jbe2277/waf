using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Waf.UnitTesting.Mocks;
using Waf.Writer.Applications.ViewModels;
using Waf.Writer.Applications.Views;

namespace Test.Writer.Applications.Views;

[Export(typeof(ISaveChangesView)), Export, PartCreationPolicy(CreationPolicy.NonShared)]
public class MockSaveChangesView : MockDialogView<MockSaveChangesView>, ISaveChangesView
{
    public SaveChangesViewModel ViewModel => ViewHelper.GetViewModel<SaveChangesViewModel>(this)!;
}
