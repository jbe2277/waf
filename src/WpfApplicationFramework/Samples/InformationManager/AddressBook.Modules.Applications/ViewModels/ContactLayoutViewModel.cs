using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;
using System.ComponentModel.Composition;

namespace Waf.InformationManager.AddressBook.Modules.Applications.ViewModels
{
    [Export]
    public class ContactLayoutViewModel : ViewModel<IContactLayoutView>
    {
        private object contactListView;
        private object contactView;

        
        [ImportingConstructor]
        public ContactLayoutViewModel(IContactLayoutView view) : base(view)
        {
        }


        public object ContactListView
        {
            get { return contactListView; }
            set { SetProperty(ref contactListView, value); }
        }

        public object ContactView
        {
            get { return contactView; }
            set { SetProperty(ref contactView, value); }
        }
    }
}
