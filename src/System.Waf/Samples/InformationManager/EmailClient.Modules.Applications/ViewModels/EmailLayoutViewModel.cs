using System.ComponentModel.Composition;
using System.Waf.Applications;
using Waf.InformationManager.EmailClient.Modules.Applications.Views;

namespace Waf.InformationManager.EmailClient.Modules.Applications.ViewModels
{
    [Export]
    public class EmailLayoutViewModel : ViewModel<IEmailLayoutView>
    {
        private object emailListView;
        private object emailView;

        [ImportingConstructor]
        public EmailLayoutViewModel(IEmailLayoutView view) : base(view)
        {
        }

        public object EmailListView
        {
            get { return emailListView; }
            set { SetProperty(ref emailListView, value); }
        }

        public object EmailView
        {
            get { return emailView; }
            set { SetProperty(ref emailView, value); }
        }
    }
}
