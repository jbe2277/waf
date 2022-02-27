using System.ComponentModel.Composition;
using System.Waf.UnitTesting.Mocks;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;

namespace Test.InformationManager.AddressBook.Modules.Applications.Views;

[Export(typeof(IContactLayoutView))]
public class MockContactLayoutView : MockView, IContactLayoutView
{
}
