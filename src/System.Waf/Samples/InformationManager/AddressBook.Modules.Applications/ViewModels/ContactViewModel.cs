using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using System.Waf.Applications;
using System.ComponentModel.Composition;
using Waf.InformationManager.AddressBook.Modules.Domain;

namespace Waf.InformationManager.AddressBook.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ContactViewModel : ViewModel<IContactView>
    {
        private Contact? contact;

        [ImportingConstructor]
        public ContactViewModel(IContactView view) : base(view)
        {
        }

        public Contact? Contact
        {
            get => contact;
            set => SetProperty(ref contact, value);
        }
    }
}
