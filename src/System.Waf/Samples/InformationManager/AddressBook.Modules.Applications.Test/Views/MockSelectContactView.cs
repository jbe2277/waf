using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;

namespace Test.InformationManager.AddressBook.Modules.Applications.Views;

[Export(typeof(ISelectContactView)), PartCreationPolicy(CreationPolicy.NonShared)]
public class MockSelectContactView : MockDialogView<MockSelectContactView>, ISelectContactView
{
}
