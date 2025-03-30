using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.AddressBook.Modules.Applications.ViewModels;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;

namespace Test.InformationManager.AddressBook.Modules.Applications.Views;

public class MockSelectContactView : MockDialogView<MockSelectContactView, SelectContactViewModel>, ISelectContactView
{
}
