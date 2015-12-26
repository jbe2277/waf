using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Waf.InformationManager.AddressBook.Interfaces.Applications;
using System.ComponentModel.Composition;
using Waf.InformationManager.AddressBook.Interfaces.Domain;

namespace Test.InformationManager.AddressBook.Modules.Applications.Services
{
    [Export(typeof(IAddressBookService)), Export]
    public class MockAddressBookService : IAddressBookService
    {
        public Func<object, ContactDto> ShowSelectContactViewAction { get; set; }
        

        public ContactDto ShowSelectContactView(object ownerView)
        {
            return ShowSelectContactViewAction(ownerView);
        }
    }
}
