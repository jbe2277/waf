using System.ComponentModel.Composition;
using System.Waf.Applications;
using System.Windows.Input;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class EditEmailAccountViewModel : ViewModel<IEditEmailAccountView>
    {
        private ICommand backCommand;
        private ICommand nextCommand;
        private object contentView;
        private bool isValid = true;
        private bool isLastPage;
        
        
        [ImportingConstructor]
        public EditEmailAccountViewModel(IEditEmailAccountView view) : base(view)
        {
        }


        public ICommand BackCommand 
        { 
            get { return backCommand; }
            set { SetProperty(ref backCommand, value); }
        }

        public ICommand NextCommand 
        { 
            get { return nextCommand; }
            set { SetProperty(ref nextCommand, value); }
        }

        public object ContentView
        {
            get { return contentView; }
            set { SetProperty(ref contentView, value); }
        }

        public bool IsValid
        {
            get { return isValid; }
            set { SetProperty(ref isValid, value); }
        }

        public bool IsLastPage
        {
            get { return isLastPage; }
            set { SetProperty(ref isLastPage, value); }
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
