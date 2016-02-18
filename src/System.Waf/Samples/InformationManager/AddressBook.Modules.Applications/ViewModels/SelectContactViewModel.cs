using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.AddressBook.Modules.Applications.Views;

namespace Waf.InformationManager.AddressBook.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class SelectContactViewModel : ViewModel<ISelectContactView>
    {
        private ICommand okCommand;
        private object contactListView;

        
        [ImportingConstructor]
        public SelectContactViewModel(ISelectContactView view) : base(view)
        {
        }


        public ICommand OkCommand
        {
            get { return okCommand; }
            set { SetProperty(ref okCommand, value); }
        }

        public object ContactListView
        {
            get { return contactListView; }
            set { SetProperty(ref contactListView, value); }
        }


        public void ShowDialog(object owner)
        {
            ViewCore.ShowDialog(owner);
        }

        public void Close()
        {
            ViewCore.Close();
        }
    }
}
